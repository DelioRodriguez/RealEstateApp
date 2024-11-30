using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Dtos.SaleType;
using RealEstateApp.Application.Interfaces.Services.Api;
using RealEstateApp.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApp.Presentation.Api5.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Mantenimiento de Tipo de Venta")]
    public class SaleTypeController : BaseApiController
    {
        private readonly ISaleTypeApiService _service;
        private readonly IMapper _mapper;

        public SaleTypeController(ISaleTypeApiService saleTypeService, IMapper mapper)
        {
            _service = saleTypeService;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Crear un tipo de Venta", Description = "Agrega un nuevo tipo de Venta al sistema.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] SaleTypeCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var entity = _mapper.Map<SaleType>(dto);
                var result = await _service.AddAsync(entity);

                if (result > 0)
                    return StatusCode(201);

                return StatusCode(500, $"Error interno del servidor.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Actualizar un tipo de Venta", Description = "Actualiza los datos de un tipo de Venta existente.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(int id, [FromBody] SaleTypeUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var entity = await _service.GetByIdAsync(id);

                if (entity == null)
                    return NotFound($"El tipo de venta con ese id: {id}, no existe");

                entity.Name = dto.Name;
                entity.Description = dto.Description;

                var result = await _service.UpdateAsync(entity);

                if (result > 0)
                    return Ok(dto);

                return StatusCode(500, "Error interno.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("List")]
        [Authorize(Roles = "Admin,Developer")]
        [SwaggerOperation(Summary = "Listar tipos de Venta", Description = "Obtiene una lista de todos los tipos de Venta registrados.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List()
        {
            try
            {
                var entities = await _service.GetAllAsync();

                if (!entities.Any())
                    return NoContent();

                var dtos = _mapper.Map<IEnumerable<SaleTypeDto>>(entities);

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Developer")]
        [SwaggerOperation(Summary = "Obtener un tipo de Venta por ID", Description = "Obtiene los detalles de un tipo de Venta especificado por su ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var entity = await _service.GetByIdAsync(id);

                if (entity == null)
                    return NoContent();

                var dto = _mapper.Map<SaleTypeDto>(entity);

                return Ok(dto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Eliminar un tipo de venta", Description = "Elimina un tipo de venta especificado por su Id y todas sus propiedades asociadas.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);

                if (result > 0)
                    return NoContent();

                return StatusCode(500, $"Error interno del servidor.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
