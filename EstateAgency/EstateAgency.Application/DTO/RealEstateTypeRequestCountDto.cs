namespace EstateAgency.Application.DTO;

/// <summary>
/// DTO для информации о количестве заявок по типу недвижимости.
/// </summary>
public class RealEstateTypeRequestCountDto
{
    /// <summary>
    /// Тип недвижимости (строкой).
    /// </summary>
    public required string RealEstateType { get; set; }

    /// <summary>
    /// Количество заявок.
    /// </summary>
    public int Count { get; set; }
}