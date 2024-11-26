using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Dtos.Properties;
using RealEstateApp.Application.Interfaces.Services.Api;

namespace RealEstateApp.Presentation.Api5.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PropertiesController : ControllerBase
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
