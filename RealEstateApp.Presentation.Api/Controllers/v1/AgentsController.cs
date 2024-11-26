using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services.Api;

namespace RealEstateApp.Presentation.Api5.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AgentsController : ControllerBase
    {
        private readonly IAgentApiService _agentService;

        public AgentsController(IAgentApiService agentService)
        {
            _agentService = agentService;
        }

        [HttpGet("list")]
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
