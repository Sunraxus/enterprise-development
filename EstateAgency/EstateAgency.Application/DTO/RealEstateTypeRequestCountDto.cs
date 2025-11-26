namespace EstateAgency.Application.Dto;

/// <summary>
/// Dto для информации о количестве заявок по типу недвижимости.
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
    public required int Count { get; set; }
}