using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VY.Hackathon.TeamOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MainController : ControllerBase
    {


        [HttpGet]
        public async Task<IActionResult> GetSimple()
        {
            return null;
        }
    }
}
