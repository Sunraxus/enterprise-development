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
    public bool Success { get; }

    /// <summary>
    /// Описание ошибки, если обработка завершилась неуспешно.
    /// В случае успеха — <c>null</c>.
    /// </summary>
    public string? Error { get; }

    /// <summary>
    /// Приватный конструктор. 
    /// Используются статические фабричные методы <see cref="Ok"/> и <see cref="Fail"/>.
    /// </summary>
    private ContractProcessResult(bool success, string? error)
    {
        Success = success;
        Error = error;
    }

    /// <summary>
    /// Создаёт успешный результат обработки контракта.
    /// </summary>
    /// <returns>Экземпляр <see cref="ContractProcessResult"/> с успехом.</returns>
    public static ContractProcessResult Ok() => new(true, null);

    /// <summary>
    /// Создаёт результат с ошибкой.
    /// </summary>
    /// <param name="error">Описание ошибки.</param>
    /// <returns>Экземпляр <see cref="ContractProcessResult"/> с неуспехом и сообщением об ошибке.</returns>
    public static ContractProcessResult Fail(string error) =>
        new(false, error);
}