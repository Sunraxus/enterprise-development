using EstateAgency.Grpc.Protos;
using Grpc.Core;
using Grpc.Net.Client;

namespace EstateAgency.Grpc.Client;

/// <summary>
/// Фоновый сервис для генерации и потоковой отправки контрактов заявок на gRPC сервер.
/// Работает в цикле: генерирует пакет контрактов, отправляет через Client Streaming,
/// получает один итоговый ответ от сервера и повторяет процесс после задержки.
/// </summary>
/// <param name="logger">Логгер для структурированного логирования операций Worker.</param>
/// <param name="configuration">Конфигурация приложения для получения настроек подключения.</param>
/// <param name="generator">Генератор случайных контрактов заявок.</param>
public class Worker(
    ILogger<Worker> logger,
    IConfiguration configuration,
    ApplicationContractGenerator generator) : BackgroundService
{
    private readonly TimeSpan _batchInterval = TimeSpan.FromSeconds(
        configuration.GetValue<int>("Worker:BatchIntervalSeconds", 30));

    private readonly int _batchSize = configuration.GetValue<int>("Worker:BatchSize", 50);
    private readonly int _maxCounterpartyId = configuration.GetValue<int>("Worker:MaxCounterpartyId", 10);
    private readonly int _maxRealEstateId = configuration.GetValue<int>("Worker:MaxRealEstateId", 10);
    private readonly string _serverAddress = configuration["Worker:ServerAddress"]
        ?? throw new InvalidOperationException("Worker:ServerAddress is not configured.");

    /// <summary>
    /// Основной цикл выполнения Worker: генерирует и отправляет пакеты контрактов на сервер.
    /// </summary>
    /// <param name="stoppingToken">Токен отмены для корректного завершения работы.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "Worker started. Server: {Server}, Batch size: {BatchSize}, Interval: {Interval}s",
            _serverAddress, _batchSize, _batchInterval.TotalSeconds);

        // Задержка перед первым запросом (ждём запуска Api)
        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SendContractBatchAsync(stoppingToken);

                logger.LogInformation("Waiting {Interval} seconds before next batch...", _batchInterval.TotalSeconds);
                await Task.Delay(_batchInterval, stoppingToken);
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
        logger.LogInformation("Generating {Count} contracts...", _batchSize);

        var contracts = generator.GenerateBulk(_batchSize, _maxCounterpartyId, _maxRealEstateId).ToList();

        logger.LogInformation("Creating gRPC channel to server: {Server}", _serverAddress);

        using var channel = GrpcChannel.ForAddress(_serverAddress);
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