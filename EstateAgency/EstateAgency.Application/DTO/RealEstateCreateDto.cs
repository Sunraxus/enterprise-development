namespace EstateAgency.Application.DTO;

/// <summary>
/// DTO для создания нового объекта недвижимости.
/// Применяется для передачи данных на добавление.
/// </summary>
public class RealEstateCreateDto
{
    /// <summary>
    /// Тип недвижимости (квартира, дом и т.д.).
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// Назначение недвижимости (жилое, коммерческое и т.д.).
    /// </summary>
    public required string Purpose { get; set; }

    /// <summary>
    /// Кадастровый номер объекта.
    /// </summary>
    public required string CadastralNumber { get; set; }

    /// <summary>
    /// Адрес недвижимости.
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Общее количество этажей.
    /// </summary>
    public required int FloorsTotal { get; set; }

    /// <summary>
    /// Общая площадь недвижимости.
    /// </summary>
    public required double AreaTotal { get; set; }

    /// <summary>
    /// Количество комнат (если применимо).
    /// </summary>
    public int? Rooms { get; set; }

    /// <summary>
    /// Высота потолков (если применимо).
    /// </summary>
    public double? CeilingHeight { get; set; }

    /// <summary>
    /// Номер этажа (если применимо).
    /// </summary>
    public int? FloorNumber { get; set; }

    /// <summary>
    /// Наличие обременений по объекту.
    /// </summary>
    public bool HasEncumbrances { get; set; }
}