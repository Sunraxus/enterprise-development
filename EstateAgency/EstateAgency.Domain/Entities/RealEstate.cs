using EstateAgency.Domain.Enums;

namespace EstateAgency.Domain.Entities;

/// <summary>
/// Объект недвижимости в системе агентства: содержит тип, назначение, кадастровые и адресные данные,
/// а также основные физические характеристики, влияющие на аналитику и фильтрацию.
/// </summary>
public sealed class RealEstate
{
    /// <summary>
    /// Уникальный идентификатор объекта недвижимости в пределах системы.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Тип объекта (квартира, дом, офис, склад и т.п.) согласно доменному перечислению.
    /// </summary>
    public required TypeRealEstate Type { get; set; }

    /// <summary>
    /// Назначение объекта (жилое, коммерческое, промышленное, офисное, складское).
    /// </summary>
    public required PurposeRealEstate Purpose { get; set; }

    /// <summary>
    /// Кадастровый номер, однозначно идентифицирующий объект в госреестре.
    /// </summary>
    public required string CadastralNumber { get; set; }

    /// <summary>
    /// Почтовый адрес расположения объекта для навигации и отчетности.
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Этажность здания (общее число этажей сооружения).
    /// </summary>
    public required int FloorsTotal { get; set; }

    /// <summary>
    /// Общая площадь объекта в квадратных метрах.
    /// </summary>
    public required double AreaTotal { get; set; }

    /// <summary>
    /// Количество комнат (для жилых и некоторых коммерческих помещений); может отсутствовать.
    /// </summary>
    public int? Rooms { get; set; }

    /// <summary>
    /// Высота потолков в сантиметрах или миллиметрах по принятому соглашению; может отсутствовать.
    /// </summary>
    public double? CeilingHeight { get; set; }

    /// <summary>
    /// Этаж расположения объекта внутри здания (для квартир и офисов); может отсутствовать.
    /// </summary>
    public int? FloorNumber { get; set; }

    /// <summary>
    /// Признак наличия обременений (залоги, аресты и др.).
    /// </summary>
    public required bool HasEncumbrances { get; set; }
}
