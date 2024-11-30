using Microsoft.AspNetCore.Mvc;

namespace RealEstateApp.Presentation.Api5.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {

    }
}
