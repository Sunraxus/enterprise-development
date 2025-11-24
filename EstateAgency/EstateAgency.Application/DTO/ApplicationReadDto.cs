namespace EstateAgency.Application.DTO;

/// <summary>
/// DTO для чтения информации о заявке.
/// Применяется для возврата полной информации по каждой заявке.
/// </summary>
public class ApplicationReadDto
{
    /// <summary>
    /// Уникальный идентификатор заявки.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор контрагента, связанного с заявкой.
    /// </summary>
    public int CounterpartyId { get; set; }

    /// <summary>
    /// Идентификатор объекта недвижимости.
    /// </summary>
    public int RealEstateId { get; set; }

    /// <summary>
    /// Тип операции (покупка/продажа).
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Сумма сделки.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Дата оформления заявки.
    /// </summary>
    public DateOnly Date { get; set; }
}