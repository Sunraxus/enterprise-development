using EstateAgency.Domain.Enums;

namespace EstateAgency.Test;

/// <summary>
/// Набор юнит‑тестов для LINQ‑запросов по данным агентства недвижимости.
/// Использует TestDataFixture как общий источник инмемори данных через primary constructor.
/// </summary>
public class DomainTests(TestDataFixture fixture) : IClassFixture<TestDataFixture>
{
    /// <summary>
    /// Возвращает всех продавцов, оставивших заявки на продажу в заданном периоде,
    /// и сравнивает результат с ожидаемым упорядоченным списком ФИО.
    /// </summary>
    [Fact]
    public void GetSellersInPeriod()
    {
        var from = new DateOnly(2025, 2, 1);
        var to = new DateOnly(2025, 6, 30);

        var expected = new[]
        {
            "Орлов Артём Александров",
            "Петров Пётр Петрович",
            "Попова Елена Викторовна",
            "Соколов Дмитрий Андреевич"
        };

        var sellers = fixture.Requests
            .Where(a => a.Type == ApplicationType.Sale && a.Date >= from && a.Date <= to)
            .Join(fixture.Counterparties,
                a => a.CounterpartyId,
                c => c.Id,
                (a, c) => c.FullName)
            .Distinct()
            .Order()
            .ToList();

        Assert.Equal(expected, sellers);
    }

    /// <summary>
    /// Определяет топ 5 контрагентов по числу заявок отдельно для покупок и продаж,
    /// сортируя по убыванию по количеству, затем по ФИО по возрастанию, и сравнивает с ожидаемыми парами.
    /// </summary>
    [Fact]
    public void GetTop5CustomersByApplicationsSeparated()
    {
        var purchaseExpected = new[]
        {
            new { FullName = "Волкова Екатерина Мих.", Count = 1 },
            new { FullName = "Иванов Иван Иванович", Count = 1 },
            new { FullName = "Кузнецов Алексей Иванов", Count = 1 },
            new { FullName = "Лебедева Мария Алексеевна", Count = 1 },
            new { FullName = "Морозов Николай Петрович", Count = 1 }
        };

        var saleExpected = new[]
        {
            new { FullName = "Иванов Иван Иванович", Count = 1 },
            new { FullName = "Кузнецов Алексей Иванов", Count = 1 },
            new { FullName = "Орлов Артём Александров", Count = 1 },
            new { FullName = "Петров Пётр Петрович", Count = 1 },
            new { FullName = "Попова Елена Викторовна", Count = 1 }
        };

        var purchaseTop = fixture.Requests
            .Where(a => a.Type == ApplicationType.Purchase)
            .Join(fixture.Counterparties,
                a => a.CounterpartyId,
                c => c.Id,
                (a, c) => c.FullName)
            .GroupBy(name => name)
            .Select(g => new { FullName = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.FullName)
            .Take(5)
            .ToList();

        var saleTop = fixture.Requests
            .Where(a => a.Type == ApplicationType.Sale)
            .Join(fixture.Counterparties,
                a => a.CounterpartyId,
                c => c.Id,
                (a, c) => c.FullName)
            .GroupBy(name => name)
            .Select(g => new { FullName = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.FullName)
            .Take(5)
            .ToList();

        Assert.Equal(purchaseExpected, purchaseTop);
        Assert.Equal(saleExpected, saleTop);
    }

    /// <summary>
    /// Подсчитывает количество заявок по каждому типу недвижимости,
    /// сортирует по убыванию по количеству, затем по типу, и сравнивает с ожидаемым списком.
    /// </summary>
    [Fact]
    public void GetApplicationCountByEstateType()
    {
        var expected = new[]
        {
            new { Type = TypeRealEstate.Apartment,  Count = 6 },
            new { Type = TypeRealEstate.Office,     Count = 4 },
            new { Type = TypeRealEstate.Commercial, Count = 2 },
            new { Type = TypeRealEstate.House,      Count = 2 },
            new { Type = TypeRealEstate.Warehouse,  Count = 1 }
        };

        var counts = fixture.Requests
            .Join(fixture.EstateObjects,
                a => a.RealEstateId,
                e => e.Id,
                (a, e) => e.Type)
            .GroupBy(type => type)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Type.ToString())
            .ToList();

        Assert.Equal(expected, counts);
    }

    /// <summary>
    /// Находит контрагентов, у которых заявки с минимальной суммой, и сравнивает список ФИО с ожидаемым.
    /// </summary>
    [Fact]
    public void GetCustomersWithMinAmount()
    {
        var expected = new[]
        {
            "Лебедева Мария Алексеевна"
        };

        var minAmount = fixture.Requests.Min(a => a.Amount);

        var customers = fixture.Requests
            .Where(a => a.Amount == minAmount)
            .Join(fixture.Counterparties,
                a => a.CounterpartyId,
                c => c.Id,
                (a, c) => c.FullName)
            .Distinct()
            .Order()
            .ToList();

        Assert.Equal(expected, customers);
    }

    /// <summary>
    /// Возвращает всех контрагентов, ищущих недвижимость заданного типа,
    /// упорядочивает по ФИО и сравнивает с ожидаемым списком.
    /// </summary>
    [Fact]
    public void GetCustomersByTargetTypeOrderedByFullName()
    {
        var expected = new[]
        {
            "Иванов Иван Иванович",
            "Лебедева Мария Алексеевна",
            "Петров Пётр Петрович"
        };

        const TypeRealEstate targetType = TypeRealEstate.Apartment;

        var customers = fixture.Requests
            .Where(a => a.Type == ApplicationType.Purchase)
            .Join(fixture.EstateObjects,
                a => a.RealEstateId,
                e => e.Id,
                (a, e) => new { a.CounterpartyId, e.Type })
            .Where(x => x.Type == targetType)
            .Join(fixture.Counterparties,
                x => x.CounterpartyId,
                c => c.Id,
                (x, c) => c.FullName)
            .Distinct()
            .Order()
            .ToList();

        Assert.Equal(expected, customers);
    }
}