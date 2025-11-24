using EstateAgency.Domain.Enums;

namespace EstateAgency.Domain.Entities;

/// <summary>
/// Заявка в системе агентства: фиксирует связь контрагента с конкретным объектом недвижимости,
/// тип намерения (покупка/продажа), сумму и дату подачи для последующей аналитики и отчетности.
/// </summary>
public class Application
{
    /// <summary>
    /// Уникальный идентификатор заявки в пределах системы.
    /// Используется для ссылок из внешних контекстов и в тестах.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Внешний ключ на контрагента, оформившего заявку.
    /// Синхронизирован с навигационным свойством Counterparty.
    /// </summary>
    public required int CounterpartyId { get; set; }

    /// <summary>
    /// Навигационное свойство на контрагента — заявителя (покупателя или продавца).
    /// Позволяет получать ФИО и контакты без дополнительных запросов.
    /// </summary>
    public Counterparty? Counterparty { get; set; }

    /// <summary>
    /// Внешний ключ на объект недвижимости, к которому относится заявка.
    /// Синхронизирован с навигационным свойством RealEstate.
    /// </summary>
    public required int RealEstateId { get; set; }

    /// <summary>
    /// Навигационное свойство на объект недвижимости, фигурирующий в заявке.
    /// Содержит тип, назначение и физические характеристики объекта.
    /// </summary>
    public RealEstate? RealEstate { get; set; }

    /// <summary>
    /// Тип заявки: покупка или продажа, определяет логику фильтрации и агрегаций.
    /// </summary>
    public required ApplicationType Type { get; set; }

    /// <summary>
    /// Сумма сделки, указанная в заявке, в денежных единицах.
    /// Хранится в decimal для точности финансовых расчетов.
    /// </summary>
    public required decimal Amount { get; set; }

    /// <summary>
    /// Дата создания/подачи заявки; используется для выборок по периодам.
    /// По умолчанию инициализируется текущей датой.
    /// </summary>
    public required DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);
}