
using datingAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace datingAPI.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        
    }
}