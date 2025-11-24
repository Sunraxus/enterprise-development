using AutoMapper;
using EstateAgency.Application.DTO;
using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enums;
using EstateAgency.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

/// <summary>
/// Контроллер для управления объектами недвижимости (RealEstate) через REST API.
/// Обеспечивает создание, получение, обновление и удаление объектов недвижимости.
/// </summary>
[ApiController]
[Route("api/real-estates")]
public class RealEstateController(IRepository<RealEstate> repository, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Возвращает полный список объектов недвижимости.
    /// </summary>
    /// <returns>HTTP 200 с коллекцией DTO недвижимости</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RealEstateReadDto>>> GetAll()
    {
        var items = await repository.GetAllAsync();
        var dto = mapper.Map<IEnumerable<RealEstateReadDto>>(items);
        return Ok(dto);
    }

    /// <summary>
    /// Возвращает объект недвижимости по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <returns>HTTP 200 с DTO или 404 при отсутствии</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RealEstateReadDto>> GetById(int id)
    {
        var item = await repository.GetByIdAsync(id);
        if (item is null) return NotFound("RealEstate not found.");
        return Ok(mapper.Map<RealEstateReadDto>(item));
    }

    /// <summary>
    /// Добавляет новый объект недвижимости.
    /// Валидирует тип и назначение.
    /// </summary>
    /// <param name="dto">Модель создания объекта</param>
    /// <returns>HTTP 201 и созданный объект, либо ошибка</returns>
    [HttpPost]
    public async Task<ActionResult<RealEstateReadDto>> Create([FromBody] RealEstateCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!Enum.TryParse<TypeRealEstate>(dto.Type, true, out var typeEnum))
            return BadRequest("Invalid Type for RealEstate.");
        if (!Enum.TryParse<PurposeRealEstate>(dto.Purpose, true, out var purposeEnum))
            return BadRequest("Invalid Purpose for RealEstate.");

        var model = mapper.Map<RealEstate>(dto);
        model.Type = typeEnum;
        model.Purpose = purposeEnum;

        var created = await repository.AddAsync(model);
        var resultDto = mapper.Map<RealEstateReadDto>(created);

        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Изменяет существующий объект недвижимости.
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <param name="dto">Обновлённые данные</param>
    /// <returns>HTTP 204, если обновлено, иначе ошибка</returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RealEstateCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var model = await repository.GetByIdAsync(id);
        if (model is null) return NotFound("RealEstate to update not found.");

        if (!Enum.TryParse<TypeRealEstate>(dto.Type, true, out var typeEnum))
            return BadRequest("Invalid Type for RealEstate.");
        if (!Enum.TryParse<PurposeRealEstate>(dto.Purpose, true, out var purposeEnum))
            return BadRequest("Invalid Purpose for RealEstate.");

        mapper.Map(dto, model);
        model.Type = typeEnum;
        model.Purpose = purposeEnum;
        await repository.UpdateAsync(model);

        return NoContent();
    }

    /// <summary>
    /// Удаляет объект недвижимости.
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <returns>HTTP 204 или ошибка</returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await repository.GetByIdAsync(id);
        if (model is null) return NotFound("RealEstate to delete not found.");

        var deleted = await repository.DeleteAsync(id);
        return deleted ? NoContent() : BadRequest("Unable to delete RealEstate.");
    }
}