namespace EstateAgency.Domain.Entities;

/// <summary>
/// Контрагент риэлторского агентства: физическое лицо или представитель, участвующий в заявках на покупку/продажу объектов.
/// Содержит идентификатор, ФИО и базовые реквизиты для идентификации и связи.
/// </summary>
public class Counterparty
{
    /// <summary>
    /// Уникальный числовой идентификатор контрагента в пределах системы.
    /// Используется как внешний ключ в заявках и других связях.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Полное имя контрагента (ФИО).
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Серия и номер паспорта.
    /// Применяется для однозначной идентификации контрагента.
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Контактный телефон контрагента для связи по заявкам и уточнениям.
    /// </summary>
    public required string Phone { get; set; }
}