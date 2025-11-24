using AutoMapper;
using EstateAgency.Application.DTO;
using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enums;
using EstateAgency.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

/// <summary>
/// Контроллер для управления сущностями заявок (Application) через REST API.
/// Поддерживает операции создания, получения, обновления и удаления заявок,
/// а также проверку целостности по связанным объектам недвижимости и контрагентам.
/// </summary>
[ApiController]
[Route("api/applications")]
public class ApplicationController(
    IRepository<EstateAgency.Domain.Entities.Application> repository,
    IRepository<RealEstate> realEstateRepository,
    IRepository<Counterparty> counterpartyRepository,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех заявок из базы данных.
    /// </summary>
    /// <returns>HTTP 200 со списком заявок или пустым списком</returns>
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
    /// <param name="id">Идентификатор заявки</param>
    /// <returns>HTTP 200 с DTO заявки или 404, если не найдена</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApplicationReadDto>> GetById(int id)
    {
        var item = await repository.GetByIdAsync(id);
        if (item is null)
            return NotFound("Application not found.");
        return Ok(mapper.Map<ApplicationReadDto>(item));
    }

    /// <summary>
    /// Создаёт новую заявку на основе данных из DTO.
    /// Проверяет наличие связанных сущностей и корректность типа.
    /// </summary>
    /// <param name="dto">Данные для создания заявки</param>
    /// <returns>HTTP 201 с созданной заявкой или код ошибки</returns>
    [HttpPost]
    public async Task<ActionResult<ApplicationReadDto>> Create([FromBody] ApplicationCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var realEstateExists = await realEstateRepository.GetByIdAsync(dto.RealEstateId) != null;
        var counterpartyExists = await counterpartyRepository.GetByIdAsync(dto.CounterpartyId) != null;
        if (!realEstateExists || !counterpartyExists)
            return Conflict("RealEstate or Counterparty not found.");

        if (!Enum.TryParse<ApplicationType>(dto.Type, true, out var typeEnum))
            return BadRequest("Invalid Application type.");

        var model = mapper.Map<EstateAgency.Domain.Entities.Application>(dto);
        model.Type = typeEnum;

        var created = await repository.AddAsync(model);
        var resultDto = mapper.Map<ApplicationReadDto>(created);

        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Обновляет существующую заявку по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    /// <param name="dto">Новые значения полей заявки</param>
    /// <returns>HTTP 204 при успехе или код ошибки</returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ApplicationCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var model = await repository.GetByIdAsync(id);
        if (model is null)
            return NotFound("Application to update not found.");

        var realEstateExists = await realEstateRepository.GetByIdAsync(dto.RealEstateId) != null;
        var counterpartyExists = await counterpartyRepository.GetByIdAsync(dto.CounterpartyId) != null;
        if (!realEstateExists || !counterpartyExists)
            return Conflict("RealEstate or Counterparty not found.");

        if (!Enum.TryParse<ApplicationType>(dto.Type, true, out var typeEnum))
            return BadRequest("Invalid Application type.");

        mapper.Map(dto, model);
        model.Type = typeEnum;

        await repository.UpdateAsync(model);
        return NoContent();
    }

    /// <summary>
    /// Удаляет заявку по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    /// <returns>HTTP 204 при успехе, 404 или 400 при ошибке</returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await repository.GetByIdAsync(id);
        if (model is null) return NotFound("Application to delete not found.");

        var deleted = await repository.DeleteAsync(id);
        return deleted ? NoContent() : BadRequest("Unable to delete Application.");
    }
}
