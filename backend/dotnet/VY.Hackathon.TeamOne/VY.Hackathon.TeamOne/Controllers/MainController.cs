using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VY.Hackathon.TeamOne.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MainController : ControllerBase
    {


        [HttpGet]
        public async Task<IActionResult> GetSimple()
        {

        }
    }
}
