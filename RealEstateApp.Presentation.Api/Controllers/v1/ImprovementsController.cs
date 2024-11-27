using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Dtos.Improvements;
using RealEstateApp.Application.Interfaces.Services.Api;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Presentation.Api5.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ImprovementsController : ControllerBase
    {
        private readonly IImprovementsApiService _service;
        private readonly IMapper _mapper;

        public ImprovementsController(IImprovementsApiService improvementsApiService, IMapper mapper)
        {
            _service = improvementsApiService;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ImprovementCreateDto dto)
        {
            try
            {
                if(!ModelState.IsValid) 
                    return BadRequest(ModelState);

                var entity = _mapper.Map<Improvement>(dto);
                var result = await _service.AddAsync(entity);

                if (result > 0)
                    return StatusCode(201);

                return StatusCode(500, "Error interno del servidor.");
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] ImprovementUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                    return NotFound($"La mejora con el Id: {id}, No existe.");

                entity.Name = dto.Name;
                entity.Description = dto.Description;

                var result = await _service.UpdateAsync(entity);

                if(result > 0)
                    return Ok(dto);

                return StatusCode(500, "Error interno del servidor.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpGet("List")]
        [Authorize(Roles = "Admin,Developer")]
        public async Task <IActionResult> List()
        {
            try
            {
                var entity = await _service.GetAllAsync();

                if (!entity.Any())
                    return NoContent();

                var dtos = _mapper.Map<IEnumerable<ImprovementDto>> (entity);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Developer")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var entity = await _service.GetByIdAsync (id);

                if ( entity == null)
                    return NoContent ();

                var dto = _mapper.Map<ImprovementDto>(entity);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync (id);

                if (result > 0 )
                    return NoContent();

                return StatusCode(500, "Error interno.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
