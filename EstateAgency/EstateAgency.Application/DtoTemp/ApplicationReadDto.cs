namespace EstateAgency.Application.Dto;

/// <summary>
/// Dto для чтения информации о заявке.
/// Применяется для возврата полной информации по каждой заявке.
/// </summary>
public class ApplicationReadDto
{
    /// <summary>
    /// Уникальный идентификатор заявки.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Идентификатор контрагента, связанного с заявкой.
    /// </summary>
    public required int CounterpartyId { get; set; }

    /// <summary>
    /// Идентификатор объекта недвижимости.
    /// </summary>
    public required int RealEstateId { get; set; }

    /// <summary>
    /// Тип операции (покупка/продажа).
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// Сумма сделки.
    /// </summary>
    public required decimal Amount { get; set; }

    /// <summary>
    /// Дата оформления заявки.
    /// </summary>
    public required DateOnly Date { get; set; }
}