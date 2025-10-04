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
            .Select(a => a.Counterparty.FullName)
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
        }.ToList();

        var saleExpected = new[]
        {
            new { FullName = "Иванов Иван Иванович", Count = 1 },
            new { FullName = "Кузнецов Алексей Иванов", Count = 1 },
            new { FullName = "Орлов Артём Александров", Count = 1 },
            new { FullName = "Петров Пётр Петрович", Count = 1 },
            new { FullName = "Попова Елена Викторовна", Count = 1 }
        }.ToList();

        var purchaseTop = fixture.Requests
            .Where(a => a.Type == ApplicationType.Purchase)
            .GroupBy(a => a.Counterparty.FullName)
            .Select(g => new { FullName = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.FullName)
            .Take(5)
            .ToList();

        var saleTop = fixture.Requests
            .Where(a => a.Type == ApplicationType.Sale)
            .GroupBy(a => a.Counterparty.FullName)
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
    /// сортирует по убывание по количеству, затем по типу, и сравнивает с ожидаемым списком.
    /// </summary>
    [Fact]
    public void GetApplicationCountByEstateType()
    {
        var expected = new[]
    {
            new { Type = TypeRealEstate.Apartment,  Count = 6 },
            new { Type = TypeRealEstate.House,      Count = 2 },
            new { Type = TypeRealEstate.Commercial, Count = 2 },
            new { Type = TypeRealEstate.Office,     Count = 4 },
            new { Type = TypeRealEstate.Warehouse,  Count = 1 }
        }.ToList();

        var counts = fixture.Requests
            .GroupBy(a => a.RealEstate.Type)
            .Select(g => new { Type = g.Key, Count = g.Count() })
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
            .Select(a => a.Counterparty.FullName)
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
            .Where(a => a.Type == ApplicationType.Purchase && a.RealEstate.Type == targetType)
            .Select(a => a.Counterparty.FullName)
            .Distinct()
            .Order()
            .ToList();

        Assert.Equal(expected, customers);
    }
}
