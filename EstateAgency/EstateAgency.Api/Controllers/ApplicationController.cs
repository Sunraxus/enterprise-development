using AutoMapper;
using EstateAgency.Application.Dto;
using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enums;
using EstateAgency.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

/// <summary>
/// Контроллер для операций CRUD над заявками, включая проверки наличия связанных объектов.
/// Навигация и сохранение выполняется как через FK, так и через объект.
/// </summary>
[ApiController]
[Route("api/applications")]
public class ApplicationController(
    IRepository<Domain.Entities.Application> repository,
    IRepository<RealEstate> realEstateRepository,
    IRepository<Counterparty> counterpartyRepository,
    IMapper mapper
) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех заявок из базы данных.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApplicationReadDto>>> GetAll()
    {
        var items = await repository.GetAllAsync();
        var dto = mapper.Map<IEnumerable<ApplicationReadDto>>(items);
        return Ok(dto);
    }

    /// <summary>
    /// Возвращает заявку по её идентификатору.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApplicationReadDto>> GetById(int id)
    {
        var item = await repository.GetByIdAsync(id);
        if (item is null)
            return NotFound("Application not found.");
        return Ok(mapper.Map<ApplicationReadDto>(item));
    }

    /// <summary>
    /// Создаёт новую заявку, проверяя наличие FK и присоединённых сущностей.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApplicationReadDto>> Create([FromBody] ApplicationCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Проверяем наличие объекта недвижимости и контрагента по ID
        var realEstate = await realEstateRepository.GetByIdAsync(dto.RealEstateId);
        var counterparty = await counterpartyRepository.GetByIdAsync(dto.CounterpartyId);

        if (realEstate is null || counterparty is null)
            return Conflict("RealEstate or Counterparty not found.");

        if (!Enum.TryParse<ApplicationType>(dto.Type, true, out var typeEnum))
            return BadRequest("Invalid Application type.");

        // Маппинг Dto → Entity и привязка навигационных свойств вручную (FK + объект)
        var model = mapper.Map<Domain.Entities.Application>(dto);
        model.Type = typeEnum;
        model.RealEstate = realEstate;
        model.Counterparty = counterparty;
        model.RealEstateId = realEstate.Id;
        model.CounterpartyId = counterparty.Id;

        var created = await repository.AddAsync(model);
        var resultDto = mapper.Map<ApplicationReadDto>(created);

        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Обновляет заявку по идентификатору, корректно синхронизируя FK и навигационные свойства.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ApplicationCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var model = await repository.GetByIdAsync(id);
        if (model is null)
            return NotFound("Application to update not found.");

        var realEstate = await realEstateRepository.GetByIdAsync(dto.RealEstateId);
        var counterparty = await counterpartyRepository.GetByIdAsync(dto.CounterpartyId);

        if (realEstate is null || counterparty is null)
            return Conflict("RealEstate or Counterparty not found.");

        if (!Enum.TryParse<ApplicationType>(dto.Type, true, out var typeEnum))
            return BadRequest("Invalid Application type.");

        // Обновление всех свойств + синхронизация FK и навигации
        mapper.Map(dto, model);
        model.Type = typeEnum;
        model.RealEstate = realEstate;
        model.Counterparty = counterparty;
        model.RealEstateId = realEstate.Id;
        model.CounterpartyId = counterparty.Id;

        await repository.UpdateAsync(model);
        return NoContent();
    }

    /// <summary>
    /// Удаляет заявку по идентификатору.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await repository.DeleteAsync(id);
        return NoContent();
    }
}