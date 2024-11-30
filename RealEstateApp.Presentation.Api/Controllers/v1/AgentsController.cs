using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services.Api;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApp.Presentation.Api5.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Mantenimiento de Agentes")]
    public class AgentsController : BaseApiController
    {
        private readonly IAgentApiService _agentService;

        public AgentsController(IAgentApiService agentService)
        {
            _agentService = agentService;
        }

        [HttpGet("list")]
        [Authorize(Roles = "Developer,Admin")]
        [SwaggerOperation(
            Summary = "Listar agentes",
            Description = "Obtiene una lista de todos los agentes disponibles."
        )]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List()
        {
            try
            {
                var agents = await _agentService.GetAllAgentsAsync();
                if (agents == null || !agents.Any())
                    return NoContent();

                return Ok(agents);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }

        [HttpGet("get/{id}")]
        [Authorize(Roles = "Developer,Admin")]
        [SwaggerOperation(
            Summary = "Obtener agente por ID",
            Description = "Obtiene la información de un agente en base a su ID."
        )]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var agent = await _agentService.GetAgentByIdAsync(id);
                if (agent == null)
                    return NoContent();

                return Ok(agent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }

        [HttpGet("properties/{id}")]
        [Authorize(Roles = "Developer,Admin")]
        [SwaggerOperation(
            Summary = "Obtener propiedades de un agente",
            Description = "Obtiene todas las propiedades asociadas a un agente específico."
        )]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAgentProperties(string id)
        {
            try
            {
                var properties = await _agentService.GetAgentPropertiesAsync(id);
                if (properties == null || !properties.Any())
                    return NoContent();

                return Ok(properties);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }

        [HttpPut("change-status")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Cambiar estado de un agente",
            Description = "Permite cambiar el estado (activo/inactivo) de un agente."
        )]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangeStatus(string id, [FromBody] bool status)
        {
            try
            {
                var result = await _agentService.ChangeAgentStatusAsync(id, status);
                if (!result)
                    return NotFound(new { message = "Agent not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }
    }
}
