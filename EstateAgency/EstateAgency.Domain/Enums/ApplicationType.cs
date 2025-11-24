namespace EstateAgency.Domain.Enums;

/// <summary>
/// Тип заявки в системе агентства недвижимости; определяет намерение контрагента в рамках сделки.
/// </summary>
public enum ApplicationType
{
    /// <summary>
    /// Заявка на покупку объекта недвижимости.
    /// </summary>
    Purchase,

    /// <summary>
    /// Заявка на продажу объекта недвижимости.
    /// </summary>
    Sale
}