using EstateAgency.Application.Dto;
using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

/// <summary>
/// Контроллер для управления сущностями контрагентов (Counterparty) через REST API.
/// Предоставляет возможность создавать, получать, обновлять и удалять контрагентов.
/// </summary>
[ApiController]
[Route("api/counter-parties")]
public class CounterpartyController(IRepository<Counterparty> repository, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех контрагентов.
    /// </summary>
    /// <returns>HTTP 200 с коллекцией контрагентов</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CounterpartyReadDto>>> GetAll()
    {
        var items = await repository.GetAllAsync();
        var dto = mapper.Map<IEnumerable<CounterpartyReadDto>>(items);
        return Ok(dto);
    }

    /// <summary>
    /// Возвращает контрагента по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор контрагента</param>
    /// <returns>HTTP 200 с данными или 404, если не найден</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CounterpartyReadDto>> GetById(int id)
    {
        var item = await repository.GetByIdAsync(id);
        if (item is null) return NotFound("Counterparty not found.");
        return Ok(mapper.Map<CounterpartyReadDto>(item));
    }

    /// <summary>
    /// Создаёт нового контрагента.
    /// </summary>
    /// <param name="dto">Модель для создания контрагента</param>
    /// <returns>HTTP 201 с Dto нового контрагента</returns>
    [HttpPost]
    public async Task<ActionResult<CounterpartyReadDto>> Create([FromBody] CounterpartyCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var model = mapper.Map<Counterparty>(dto);
        var created = await repository.AddAsync(model);
        var resultDto = mapper.Map<CounterpartyReadDto>(created);

        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Обновляет данные существующего контрагента.
    /// </summary>
    /// <param name="id">Идентификатор контрагента</param>
    /// <param name="dto">Новые параметры</param>
    /// <returns>HTTP 204 при успехе, 404 или 400 при ошибке</returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CounterpartyCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var model = await repository.GetByIdAsync(id);
        if (model is null) return NotFound("Counterparty to update not found.");

        mapper.Map(dto, model);
        await repository.UpdateAsync(model);

        return NoContent();
    }

    /// <summary>
    /// Удаляет контрагента по идентификатору.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await repository.DeleteAsync(id);
        return NoContent();
    }
}