using EstateAgency.Domain.Enums;

namespace EstateAgency.Test;

public class DomainTests(TestDataFixture fixture) : IClassFixture<TestDataFixture>
{
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
            .OrderBy(n => n)
            .ToList();

        Assert.Equal(expected, sellers);
    }

    [Fact]
    public void GetTop5CustomersByApplicationsSeparated()
    {
        var purchaseExpected = new[]
        {
        new { FullName = "Волкова Екатерина Мих.",    Count = 1 },
        new { FullName = "Иванов Иван Иванович",      Count = 1 },
        new { FullName = "Кузнецов Алексей Иванов",   Count = 1 },
        new { FullName = "Лебедева Мария Алексеевна", Count = 1 },
        new { FullName = "Морозов Николай Петрович",  Count = 1 }
    }.ToList();

        var saleExpected = new[]
        {
        new { FullName = "Иванов Иван Иванович",      Count = 1 },
        new { FullName = "Кузнецов Алексей Иванов",   Count = 1 },
        new { FullName = "Орлов Артём Александров",   Count = 1 },
        new { FullName = "Петров Пётр Петрович",      Count = 1 },
        new { FullName = "Попова Елена Викторовна",   Count = 1 }
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

    [Fact]
    public void GetApplicationCountByEstateType()
    {
        var expected = new[]
        {
        new { Type = TypeRealEstate.Apartment,  Count = 6 },
        new { Type = TypeRealEstate.Office,     Count = 4 },
        new { Type = TypeRealEstate.House,      Count = 2 },
        new { Type = TypeRealEstate.Commercial, Count = 2 },
        new { Type = TypeRealEstate.Warehouse,  Count = 1 }
    }
        .OrderByDescending(x => x.Count)
        .ThenBy(x => x.Type)
        .ToList();

        var counts = fixture.Requests
            .GroupBy(a => a.RealEstate.Type)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Type)
            .ToList();

        Assert.Equal(expected, counts);
    }

    [Fact]
    public void GetCustomersWithMinAmount()
    {
        var expected = new[] { "Лебедева Мария Алексеевна" };

        var customers = fixture.Requests
            .OrderBy(a => a.Amount)
            .Take(1)
            .Select(a => a.Counterparty.FullName)
            .ToList();

        Assert.Equal(expected, customers);
    }

    [Fact]
    public void GetCustomersByTargetTypeOrderedByFullName()
    {
        var expected = new[]
        {
            "Иванов Иван Иванович",
            "Лебедева Мария Алексеевна",
            "Петров Пётр Петрович"
        };

        var customers = fixture.Requests
            .Where(a => a.Type == ApplicationType.Purchase && a.RealEstate.Type == TypeRealEstate.Apartment)
            .Select(a => a.Counterparty.FullName)
            .Distinct()
            .OrderBy(n => n)
            .ToList();

        Assert.Equal(expected, customers);
    }
}
