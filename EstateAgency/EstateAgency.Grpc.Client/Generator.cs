using Bogus;
using EstateAgency.Grpc.Protos;

namespace EstateAgency.Grpc.Client;

/// <summary>
/// Генератор контрактов заявок на недвижимость с использованием библиотеки Bogus.
/// </summary>
public class ApplicationContractGenerator
{
    private readonly Faker _faker = new();
    private static readonly string[] _validTypes = ["Purchase", "Sale"];

    /// <summary>
    /// Генерирует один случайный контракт заявки.
    /// </summary>
    /// <param name="maxCounterpartyId">Максимальный ID контрагента в БД.</param>
    /// <param name="maxRealEstateId">Максимальный ID объекта недвижимости в БД.</param>
    public ApplicationContract GenerateRandomContract(
        int maxCounterpartyId = 10,
        int maxRealEstateId = 10)
    {
        var type = _faker.PickRandom(_validTypes);
        var amount = _faker.Finance.Amount(500_000, 15_000_000, 2);
        var date = _faker.Date.PastDateOnly(1);

        return new ApplicationContract
        {
            CounterpartyId = _faker.Random.Int(1, maxCounterpartyId),
            RealEstateId = _faker.Random.Int(1, maxRealEstateId),
            Type = type,
            Amount = amount.ToString("F2"),
            Date = date.ToString("yyyy-MM-dd")
        };
    }

    /// <summary>
    /// Генерирует пакет контрактов.
    /// </summary>
    public IEnumerable<ApplicationContract> GenerateBulk(
        int count,
        int maxCounterpartyId = 10,
        int maxRealEstateId = 10)
    {
        for (var i = 0; i < count; i++)
            yield return GenerateRandomContract(maxCounterpartyId, maxRealEstateId);
    }
}