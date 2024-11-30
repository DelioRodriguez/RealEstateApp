using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Dtos.Properties;
using RealEstateApp.Application.Interfaces.Services.Api;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApp.Presentation.Api5.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Mantenimiento de propiedades")]
    public class PropertiesController : BaseApiController
    {
        private readonly IPropertiesApiService _propertiesService;
        private readonly IMapper _mapper;

        public PropertiesController(IPropertiesApiService propertiesService, IMapper mapper)
        {
            _propertiesService = propertiesService;
            _mapper = mapper;
        }

        [HttpGet("list")]
        [Authorize(Roles = "Admin,Developer")]
        [SwaggerOperation(Summary = "Listar propiedades", Description = "Obtiene una lista de todas las propiedades registradas en el sistema.")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List()
        {
            try
            {
                var properties = await _propertiesService.GetAllPropertiesAsync();

                if (!properties.Any())
                    return NoContent();

                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Developer")]
        [SwaggerOperation(Summary = "Obtener propiedad por ID", Description = "Obtiene los detalles de una propiedad específica a partir de su ID.")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var propertyDetails = await _propertiesService.GetPropertyDetailsAsync(Id);

                if (propertyDetails == null)
                    return NoContent();

                var result = _mapper.Map<PropertyDetailsDto>(propertyDetails);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }

        [HttpGet("code/{code}")]
        [Authorize(Roles = "Admin,Developer")]
        [SwaggerOperation(Summary = "Obtener propiedad por código", Description = "Obtiene los detalles de una propiedad a partir de su código.")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByCode(string code)
        {
            try
            {
                var pro = await _propertiesService.GetByCodeAsync(code);

                if (pro == null)
                    return NoContent();

                var result = _mapper.Map<PropertyDetailsDto>(pro);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }
    }
}
