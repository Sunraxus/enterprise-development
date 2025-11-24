namespace EstateAgency.Application.DTO;

/// <summary>
/// DTO для информации о контрагентах с заявками на минимальную сумму.
/// </summary>
public class ClientsWithMinAmountDto
{
    /// <summary>
    /// Минимальная сумма заявки.
    /// </summary>
    public decimal MinAmount { get; set; }

    /// <summary>
    /// Коллекция ФИО контрагентов с минимальной суммой заявки.
    /// </summary>
    public List<string> FullNames { get; set; } = new();
}