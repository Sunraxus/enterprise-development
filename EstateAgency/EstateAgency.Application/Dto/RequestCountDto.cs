namespace EstateAgency.Application.Dto;

/// <summary>
/// Dto для статистики по контрагенту с полной информацией.
/// </summary>
public class RequestCountDto
{
    /// <summary>
    /// Полная информация о контрагенте.
    /// </summary>
    public required CounterpartyReadDto Counterparty { get; set; }

    /// <summary>
    /// Количество заявок контрагента.
    /// </summary>
    public required int Count { get; set; }
}