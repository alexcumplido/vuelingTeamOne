using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VY.Hackathon.TeamOne.WebApi.Controllers.Parameters
{
    [ApiController]
    [Route("api/parameters/[controller]")]
    [Authorize]
    public class ShiftController : ControllerBase
    {
    }
}
