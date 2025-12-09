namespace EstateAgency.Grpc.Client;

/// <summary>
/// Параметры конфигурации для Worker сервиса.
/// </summary>
public class WorkerOptions
{
    public const string SectionName = "Worker";

    /// <summary>
    /// Адрес gRPC сервера.
    /// </summary>
    public required string ServerAddress { get; init; }

    /// <summary>
    /// Размер пакета контрактов для одной отправки.
    /// </summary>
    public int BatchSize { get; init; } = 50;

    /// <summary>
    /// Интервал между отправками пакетов (в секундах).
    /// </summary>
    public int BatchIntervalSeconds { get; init; } = 30;

    /// <summary>
    /// Максимальный ID контрагента в БД.
    /// </summary>
    public int MaxCounterpartyId { get; init; } = 10;

    /// <summary>
    /// Максимальный ID объекта недвижимости в БД.
    /// </summary>
    public int MaxRealEstateId { get; init; } = 10;
}
