namespace EstateAgency.Application.Dto;

/// <summary>
/// Dto для информации о контрагентах с заявками на минимальную сумму.
/// </summary>
public class ClientsWithMinAmountDto
{
    /// <summary>
    /// Минимальная сумма заявки.
    /// </summary>
    public required decimal MinAmount { get; set; }

    /// <summary>
    /// Список контрагентов с заявками на минимальную сумму.
    /// </summary>
    public required List<CounterpartyReadDto> Counterparties { get; set; }
}