namespace EstateAgency.Api.Grpc;

/// <summary>
/// Результат обработки одного контракта заявки.
/// Используется вместо кортежей для лучшей читаемости и расширяемости.
/// </summary>
public class ContractProcessResult
{
    /// <summary>
    /// Признак успешной обработки контракта.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Описание ошибки, если обработка завершилась неуспешно.
    /// </summary>
    public string? Error { get; init; }

    /// <summary>
    /// Создаёт успешный результат обработки контракта.
    /// </summary>
    public static ContractProcessResult Ok => new() { Success = true, Error = null };

    /// <summary>
    /// Создаёт результат с ошибкой.
    /// </summary>
    /// <param name="error">Описание ошибки.</param>
    public static ContractProcessResult Fail(string error) => new() { Success = false, Error = error };
}