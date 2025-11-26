using EstateAgency.Application.Dto;
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
    IRepository<Domain.Entities.Application> applicationRepository
) : ControllerBase
{
    /// <summary>
    /// Продавцы, оформившие заявки на продажу в заданный период.
    /// </summary>
    /// <param name="from">Дата начала периода</param>
    /// <param name="to">Дата конца периода</param>
    /// <returns>Список контрагентов-продавцов с полной информацией</returns>
    [HttpGet("sellers-by-period")]
    public async Task<ActionResult<List<CounterpartyReadDto>>> GetSellersByPeriod([FromQuery] DateOnly from, [FromQuery] DateOnly to)
    {
        var applications = await applicationRepository.GetAllWithIncludesAsync("Counterparty");

        var sellers = applications
            .Where(a => a.Type == ApplicationType.Sale && a.Date >= from && a.Date <= to && a.Counterparty != null)
            .Select(a => new CounterpartyReadDto
            {
                Id = a.Counterparty!.Id,
                FullName = a.Counterparty!.FullName,
                PassportNumber = a.Counterparty!.PassportNumber,
                Phone = a.Counterparty!.Phone
            })
            .DistinctBy(c => c.Id)
            .OrderBy(c => c.FullName)
            .ToList();

        return Ok(sellers);
    }

    /// <summary>
    /// Топ-5 контрагентов по числу заявок на покупку или продажу.
    /// </summary>
    /// <param name="type">Тип заявки (Purchase/Sale)</param>
    /// <returns>Список контрагентов с количеством заявок</returns>
    [HttpGet("top-customers")]
    public async Task<ActionResult<List<RequestCountDto>>> GetTop5CustomersByApplications([FromQuery] string type)
    {
        if (!Enum.TryParse<ApplicationType>(type, true, out var appType))
            return BadRequest("Некорректный тип заявки.");

        var applications = await applicationRepository.GetAllWithIncludesAsync("Counterparty");

        var result = applications
            .Where(a => a.Type == appType && a.Counterparty != null)
            .GroupBy(a => a.CounterpartyId)
            .Select(g => new RequestCountDto
            {
                Counterparty = new CounterpartyReadDto
                {
                    Id = g.First().Counterparty!.Id,
                    FullName = g.First().Counterparty!.FullName,
                    PassportNumber = g.First().Counterparty!.PassportNumber,
                    Phone = g.First().Counterparty!.Phone
                },
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Counterparty.FullName)
            .Take(5)
            .ToList();

        return Ok(result);
    }

    /// <summary>
    /// Количество заявок по каждому типу недвижимости.
    /// </summary>
    /// <returns>Список Dto с типом и количеством</returns>
    [HttpGet("application-count-by-estate-type")]
    public async Task<ActionResult<List<RealEstateTypeRequestCountDto>>> GetApplicationCountByEstateType()
    {
        var applications = await applicationRepository.GetAllWithIncludesAsync("RealEstate");

        var result = applications
            .Where(a => a.RealEstate != null)
            .GroupBy(a => a.RealEstate!.Type)
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
    /// <returns>Dto с минимальной суммой и списком контрагентов</returns>
    [HttpGet("clients-with-min-amount")]
    public async Task<ActionResult<ClientsWithMinAmountDto>> GetClientsWithMinAmount()
    {
        var applications = await applicationRepository.GetAllWithIncludesAsync("Counterparty");

        var minAmount = applications.Min(a => a.Amount);

        var counterparties = applications
            .Where(a => a.Amount == minAmount && a.Counterparty != null)
            .Select(a => new CounterpartyReadDto
            {
                Id = a.Counterparty!.Id,
                FullName = a.Counterparty!.FullName,
                PassportNumber = a.Counterparty!.PassportNumber,
                Phone = a.Counterparty!.Phone
            })
            .DistinctBy(c => c.Id)
            .OrderBy(c => c.FullName)
            .ToList();

        var dto = new ClientsWithMinAmountDto
        {
            MinAmount = minAmount,
            Counterparties = counterparties
        };

        return Ok(dto);
    }

    /// <summary>
    /// Контрагенты, совершавшие заявки на покупку недвижимости заданного типа.
    /// </summary>
    /// <param name="type">Тип недвижимости (Apartment, House и т.д.)</param>
    /// <returns>Список Dto контрагентов, отсортированных по ФИО</returns>
    [HttpGet("customers-by-estate-type")]
    public async Task<ActionResult<List<CounterpartyReadDto>>> GetCustomersByEstateType([FromQuery] string type)
    {
        if (!Enum.TryParse<TypeRealEstate>(type, true, out var estateType))
            return BadRequest("Некорректный тип недвижимости.");

        var applications = await applicationRepository.GetAllWithIncludesAsync("Counterparty", "RealEstate");

        var customers = applications
            .Where(a => a.Type == ApplicationType.Purchase && a.RealEstate != null && a.RealEstate.Type == estateType && a.Counterparty != null)
            .Select(a => new CounterpartyReadDto
            {
                Id = a.Counterparty!.Id,
                FullName = a.Counterparty!.FullName,
                PassportNumber = a.Counterparty!.PassportNumber,
                Phone = a.Counterparty!.Phone
            })
            .DistinctBy(c => c.Id)
            .OrderBy(x => x.FullName)
            .ToList();

        return Ok(customers);
    }
}
