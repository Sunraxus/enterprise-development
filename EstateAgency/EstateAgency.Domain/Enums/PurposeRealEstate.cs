namespace EstateAgency.Domain.Enums;

/// <summary>
/// Назначение объекта недвижимости; помогает разделять жилые и нежилые фонды для фильтрации и аналитики.
/// </summary>
public enum PurposeRealEstate
{
    /// <summary>
    /// Жилое назначение (квартира, дом).
    /// </summary>
    Residential,

    /// <summary>
    /// Коммерческое назначение (торговля, услуги).
    /// </summary>
    Commercial,

    /// <summary>
    /// Промышленное назначение (производственные цеха, площадки).
    /// </summary>
    Industrial,

    /// <summary>
    /// Офисное назначение (административные и деловые помещения).
    /// </summary>
    Office,

    /// <summary>
    /// Складское назначение (хранение, логистика, распределение).
    /// </summary>
    Storage
}