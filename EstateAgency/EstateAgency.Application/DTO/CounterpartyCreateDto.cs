namespace EstateAgency.Application.DTO;

/// <summary>
/// DTO для создания нового контрагента.
/// Используется при регистрации нового клиента.
/// </summary>
public class CounterpartyCreateDto
{
    /// <summary>
    /// ФИО контрагента.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Номер паспорта контрагента.
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Номер телефона контрагента.
    /// </summary>
    public required string Phone { get; set; }
}