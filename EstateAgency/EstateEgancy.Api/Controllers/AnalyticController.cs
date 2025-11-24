using EstateAgency.Application.DTO;
using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enums;
using EstateAgency.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

/// <summary>
/// Контроллер для аналитических запросов по данным агентства недвижимости.
/// Реализует методы выборок, группировок и агрегирования по заявкам, контрагентам и недвижимости.
/// </summary>
[ApiController]
[Route("api/analytics")]
public class AnalyticController(
    IRepository<EstateAgency.Domain.Entities.Application> applicationRepository,
    IRepository<RealEstate> realEstateRepository
) : ControllerBase
{
    /// <summary>
    /// Продавцы, оформившие заявки на продажу в заданный период.
    /// </summary>
    /// <param name="from">Дата начала периода</param>
    /// <param name="to">Дата конца периода</param>
    /// <returns>Список ФИО продавцов</returns>
    [HttpGet("sellers-by-period")]
    public async Task<ActionResult<List<string>>> GetSellersByPeriod([FromQuery] DateOnly from, [FromQuery] DateOnly to)
    {
        var applications = await applicationRepository.GetAllAsync();
        var sellers = applications
            .Where(a => a.Type == ApplicationType.Sale && a.Date >= from && a.Date <= to)
            .Select(a => a.Counterparty.FullName)
            .Distinct()
            .Order()
            .ToList();

        return Ok(sellers);
    }

    /// <summary>
    /// Топ-5 контрагентов по числу заявок на покупку и продажу.
    /// </summary>
    /// <param name="type">Тип заявки (Purchase/Sale)</param>
    /// <returns>Список контрагентов и количества заявок</returns>
    [HttpGet("top-customers")]
    public async Task<ActionResult<List<RequestCountDto>>> GetTop5CustomersByApplications([FromQuery] string type)
    {
        if (!Enum.TryParse<ApplicationType>(type, true, out var appType))
            return BadRequest("Некорректный тип заявки.");

        var applications = await applicationRepository.GetAllAsync();

        var result = applications
            .Where(a => a.Type == appType)
            .GroupBy(a => a.Counterparty.FullName)
            .Select(g => new RequestCountDto
            {
                Id = g.First().CounterpartyId,
                FullName = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.FullName)
            .Take(5)
            .ToList();

        return Ok(result);
    }

    /// <summary>
    /// Количество заявок по каждому типу недвижимости.
    /// </summary>
    /// <returns>Список DTO с типом и количеством</returns>
    [HttpGet("application-count-by-estate-type")]
    public async Task<ActionResult<List<RealEstateTypeRequestCountDto>>> GetApplicationCountByEstateType()
    {
        var applications = await applicationRepository.GetAllAsync();
        var estates = await realEstateRepository.GetAllAsync();

        var estateTypeById = estates.ToDictionary(e => e.Id, e => e.Type);

        var result = applications
            .Where(a => estateTypeById.ContainsKey(a.RealEstateId))
            .GroupBy(a => estateTypeById[a.RealEstateId])
            .Select(g => new RealEstateTypeRequestCountDto
            {
                RealEstateType = g.Key.ToString(),
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.RealEstateType)
            .ToList();

        return Ok(result);
    }

    /// <summary>
    /// Контрагенты с заявками на минимальную сумму.
    /// </summary>
    /// <returns>DTO с минимальной суммой и списком ФИО</returns>
    [HttpGet("clients-with-min-amount")]
    public async Task<ActionResult<ClientsWithMinAmountDto>> GetClientsWithMinAmount()
    {
        var applications = await applicationRepository.GetAllAsync();

        var minAmount = applications.Min(a => a.Amount);

        var fullNames = applications
            .Where(a => a.Amount == minAmount)
            .Select(a => a.Counterparty.FullName)
            .Distinct()
            .Order()
            .ToList();

        var dto = new ClientsWithMinAmountDto
        {
            MinAmount = minAmount,
            FullNames = fullNames
        };

        return Ok(dto);
    }

    /// <summary>
    /// Контрагенты, совершавшие заявки на покупку недвижимости заданного типа.
    /// </summary>
    /// <param name="type">Тип недвижимости (Apartment, House и т.д.)</param>
    /// <returns>Список DTO контрагентов, отсортированных по ФИО</returns>
    [HttpGet("customers-by-estate-type")]
    public async Task<ActionResult<List<CounterpartyReadDto>>> GetCustomersByEstateType([FromQuery] string type)
    {
        if (!Enum.TryParse<TypeRealEstate>(type, true, out var estateType))
            return BadRequest("Некорректный тип недвижимости.");

        var applications = await applicationRepository.GetAllAsync();

        var customers = applications
            .Where(a => a.Type == ApplicationType.Purchase && a.RealEstate.Type == estateType)
            .Select(a => new CounterpartyReadDto
            {
                Id = a.CounterpartyId,
                FullName = a.Counterparty.FullName,
                PassportNumber = a.Counterparty.PassportNumber,
                Phone = a.Counterparty.Phone
            })
            .Distinct()
            .OrderBy(x => x.FullName)
            .ToList();

        return Ok(customers);
    }
}
