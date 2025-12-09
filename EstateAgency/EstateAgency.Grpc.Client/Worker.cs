using EstateAgency.Grpc.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

namespace EstateAgency.Grpc.Client;

/// <summary>
/// Фоновый сервис для генерации и потоковой отправки контрактов заявок на gRPC сервер.
/// Работает в цикле: генерирует пакет контрактов, отправляет через Client Streaming,
/// получает один итоговый ответ от сервера и повторяет процесс после задержки.
/// </summary>
/// <param name="logger">Логгер для структурированного логирования операций Worker.</param>
/// <param name="options">Параметры конфигурации Worker сервиса.</param>
/// <param name="generator">Генератор случайных контрактов заявок.</param>
public class Worker(
    ILogger<Worker> logger,
    IOptions<WorkerOptions> options,
    ApplicationContractGenerator generator) : BackgroundService
{
    private readonly WorkerOptions _options = options.Value;

    /// <summary>
    /// Основной цикл выполнения Worker: генерирует и отправляет пакеты контрактов на сервер.
    /// </summary>
    /// <param name="stoppingToken">Токен отмены для корректного завершения работы.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "Worker started. Server: {Server}, Batch size: {BatchSize}, Interval: {Interval}s",
            _options.ServerAddress, _options.BatchSize, _options.BatchIntervalSeconds);

        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SendContractBatchAsync(stoppingToken);

                logger.LogInformation(
                    "Waiting {Interval} seconds before next batch...",
                    _options.BatchIntervalSeconds);
                await Task.Delay(TimeSpan.FromSeconds(_options.BatchIntervalSeconds), stoppingToken);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
            {
                logger.LogWarning("Server unavailable. Retrying in 10 seconds... Error: {Message}", ex.Message);
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Critical error while sending contracts. Retrying in 15 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }

        logger.LogInformation("Worker stopped.");
    }

    /// <summary>
    /// Генерирует пакет контрактов и отправляет их на сервер через Client Streaming.
    /// Получает один итоговый ответ после завершения потока.
    /// </summary>
    private async Task SendContractBatchAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Generating {Count} contracts...", _options.BatchSize);

        var contracts = generator.GenerateBulk(
            _options.BatchSize,
            _options.MaxCounterpartyId,
            _options.MaxRealEstateId).ToList();

        logger.LogInformation("Creating gRPC channel to server: {Server}", _options.ServerAddress);

        using var channel = GrpcChannel.ForAddress(_options.ServerAddress);
        var client = new ApplicationReceiver.ApplicationReceiverClient(channel);

        logger.LogInformation("Starting stream of {Count} contracts...", contracts.Count);

        using var call = client.StreamApplications(cancellationToken: cancellationToken);

        var sentCount = 0;
        foreach (var contract in contracts)
        {
            await call.RequestStream.WriteAsync(contract, cancellationToken);
            sentCount++;

            if (sentCount % 10 == 0 || sentCount == contracts.Count)
            {
                logger.LogDebug("Sent {Sent}/{Total} contracts", sentCount, contracts.Count);
            }
        }

        logger.LogInformation("Completing stream...");
        await call.RequestStream.CompleteAsync();

        logger.LogInformation("Waiting for server response...");
        var response = await call.ResponseAsync;

        logger.LogInformation(
            "Response received: Received={Received}, Saved={Saved}, Failed={Failed}",
            response.Received, response.Saved, response.Failed);

        if (response.Failed > 0)
        {
            logger.LogWarning("Server message: {Message}", response.Message);
        }
        else
        {
            logger.LogInformation("Server message: {Message}", response.Message);
        }
    }
}