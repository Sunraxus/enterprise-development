using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enums;
using EstateAgency.Domain.Interface;
using EstateAgency.Grpc.Protos;
using Grpc.Core;

namespace EstateAgency.Api.Grpc;

/// <summary>
/// gRPC сервис для приёма потока контрактов заявок от генератора.
/// Валидирует данные, сохраняет в БД через репозиторий и возвращает итоговую статистику.
/// </summary>
public class ApplicationReceiverService(
    IRepository<Domain.Entities.Application> applicationRepository,
    IRepository<Counterparty> counterpartyRepository,
    IRepository<RealEstate> realEstateRepository,
    ILogger<ApplicationReceiverService> logger)
    : ApplicationReceiver.ApplicationReceiverBase
{
    /// <summary>
    /// Обрабатывает поток контрактов от клиента (Client Streaming).
    /// Возвращает один итоговый ответ после получения всех данных.
    /// </summary>
    public override async Task<ApplicationsStreamResponse> StreamApplications(
        IAsyncStreamReader<ApplicationContract> requestStream,
        ServerCallContext context)
    {
        var received = 0;
        var saved = 0;
        var failed = 0;
        var errors = new List<string>();

        logger.LogInformation("Starting to receive application contracts stream...");

        try
        {
            await foreach (var contract in requestStream.ReadAllAsync(context.CancellationToken))
            {
                received++;

                logger.LogDebug("Received contract #{Number}: CounterpartyId={CId}, RealEstateId={RId}",
                    received, contract.CounterpartyId, contract.RealEstateId);

                var result = await ProcessContractAsync(contract);

                if (result.Success)
                {
                    saved++;
                }
                else
                {
                    failed++;
                    errors.Add($"Contract #{received}: {result.Error}");
                    logger.LogWarning("Failed to save contract #{Number}: {Error}", received, result.Error);
                }
            }

            logger.LogInformation("Stream completed. Received: {Received}, Saved: {Saved}, Failed: {Failed}",
                received, saved, failed);

            return new ApplicationsStreamResponse
            {
                Received = received,
                Saved = saved,
                Failed = failed,
                Message = failed > 0
                    ? $"Processing completed with {failed} errors. First errors: {string.Join("; ", errors.Take(3))}"
                    : $"Successfully processed all {saved} contracts."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Critical error during stream processing");

            return new ApplicationsStreamResponse
            {
                Received = received,
                Saved = saved,
                Failed = failed,
                Message = $"Stream processing failed: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Обрабатывает один контракт: валидирует, парсит и сохраняет в БД.
    /// </summary>
    private async Task<ContractProcessResult> ProcessContractAsync(ApplicationContract contract)
    {
        try
        {
            if (contract.CounterpartyId <= 0 || contract.RealEstateId <= 0)
            {
                return ContractProcessResult.Fail("CounterpartyId and RealEstateId must be positive");
            }

            var counterparty = await counterpartyRepository.GetByIdAsync(contract.CounterpartyId);
            if (counterparty == null)
            {
                return ContractProcessResult.Fail($"Counterparty with ID {contract.CounterpartyId} not found");
            }

            var realEstate = await realEstateRepository.GetByIdAsync(contract.RealEstateId);
            if (realEstate == null)
            {
                return ContractProcessResult.Fail($"RealEstate with ID {contract.RealEstateId} not found");
            }

            if (!Enum.TryParse<ApplicationType>(contract.Type, ignoreCase: true, out var applicationType))
            {
                return ContractProcessResult.Fail($"Invalid ApplicationType: {contract.Type}. Expected 'Purchase' or 'Sale'");
            }

            if (!decimal.TryParse(contract.Amount, out var amount) || amount <= 0)
            {
                return ContractProcessResult.Fail($"Invalid Amount: {contract.Amount}. Must be positive decimal");
            }

            if (!DateOnly.TryParseExact(contract.Date, "yyyy-MM-dd", out var date))
            {
                return ContractProcessResult.Fail($"Invalid Date: {contract.Date}. Expected format: yyyy-MM-dd");
            }

            var application = new Domain.Entities.Application
            {
                Id = 0,
                CounterpartyId = contract.CounterpartyId,
                Counterparty = counterparty,
                RealEstateId = contract.RealEstateId,
                RealEstate = realEstate,
                Type = applicationType,
                Amount = amount,
                Date = date
            };

            await applicationRepository.AddAsync(application);

            return ContractProcessResult.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing contract");
            return ContractProcessResult.Fail($"Internal error: {ex.Message}");
        }
    }
}
