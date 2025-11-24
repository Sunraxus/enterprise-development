namespace EstateAgency.Application.DTO;

/// <summary>
/// DTO для информации о количестве заявок по контрагенту.
/// </summary>
public class RequestCountDto
{
    /// <summary>
    /// Идентификатор контрагента.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ФИО контрагента.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Количество заявок.
    /// </summary>
    public int Count { get; set; }
}