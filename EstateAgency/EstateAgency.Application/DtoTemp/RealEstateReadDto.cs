namespace EstateAgency.Application.Dto;

/// <summary>
/// Dto для чтения информации об объекте недвижимости.
/// Используется для передачи полной информации по объекту.
/// </summary>
public class RealEstateReadDto
{
    /// <summary>
    /// Уникальный идентификатор объекта недвижимости.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Тип недвижимости.
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// Назначение объекта.
    /// </summary>
    public required string Purpose { get; set; }

    /// <summary>
    /// Кадастровый номер.
    /// </summary>
    public required string CadastralNumber { get; set; }

    /// <summary>
    /// Адрес объекта.
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Количество этажей.
    /// </summary>
    public required int FloorsTotal { get; set; }

    /// <summary>
    /// Общая площадь.
    /// </summary>
    public required double AreaTotal { get; set; }

    /// <summary>
    /// Количество комнат.
    /// </summary>
    public int? Rooms { get; set; }

    /// <summary>
    /// Высота потолков.
    /// </summary>
    public double? CeilingHeight { get; set; }

    /// <summary>
    /// Номер этажа.
    /// </summary>
    public int? FloorNumber { get; set; }

    /// <summary>
    /// Признак наличия обременений.
    /// </summary>
    public required bool HasEncumbrances { get; set; }
}