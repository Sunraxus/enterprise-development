namespace EstateAgency.Application.Dto;

/// <summary>
/// Dto для создания новой заявки.
/// Используется при отправке данных на добавление через API.
/// </summary>
public class ApplicationCreateDto
{
    /// <summary>
    /// Идентификатор контрагента, связанного с заявкой.
    /// </summary>
    public required int CounterpartyId { get; set; }

    /// <summary>
    /// Идентификатор недвижимости, связанной с заявкой.
    /// </summary>
    public required int RealEstateId { get; set; }

    /// <summary>
    /// Тип операции (например, покупка или продажа).
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// Сумма сделки по заявке.
    /// </summary>
    public required decimal Amount { get; set; }

    /// <summary>
    /// Дата создания или проведения заявки.
    /// </summary>
    public required DateOnly Date { get; set; }
}