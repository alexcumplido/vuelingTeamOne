using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VY.Hackathon.TeamOne.WebApi.Auth;
using VY.Hackathon.TeamOne.WebApi.Controllers.Auth.Models;
using VY.Hackathon.TeamOne.WebApi.Models;

namespace VY.Hackathon.TeamOne.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly JwtBearerTokenSettings _jwtBearerTokenSettings;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IOptions<JwtBearerTokenSettings> jwtTokenOptions,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<AuthController> logger)
    {
        _jwtBearerTokenSettings = jwtTokenOptions.Value;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    [HttpPost]
    [Route("SignUp")]
    public async Task<IActionResult> SignUp([FromBody] SignUpInfo? signUpInfo)
    {
        try
        {
            if (!ModelState.IsValid || signUpInfo == null)
            {
                _logger.LogWarning("User info provided is invalid");
                return new BadRequestObjectResult(new { Message = "User Registration Failed" });
            }

            var identityUser = new IdentityUser { UserName = signUpInfo.UserName, Email = signUpInfo.Email };
            var result = await _userManager.CreateAsync(identityUser, signUpInfo.Password);

            if (!result.Succeeded)
            {
                var dictionary = new ModelStateDictionary();
                foreach (IdentityError error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                }

                _logger.LogWarning("User info provided is invalid");

                return new BadRequestObjectResult(new { Message = "User Registration Failed", Errors = dictionary });
            }

            foreach (var role in signUpInfo.Roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                {
                    await _userManager.AddToRoleAsync(identityUser, role);
                }
            }

            return Ok(new { Message = "User Registration Successful" });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Has been an error trying to SignUp user {user}", signUpInfo?.UserName);
            throw;
        }
    }

    [HttpPost]
    [Route("Role/{role}")]
    public async Task<IActionResult> CreateRole(string role)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                _logger.LogWarning("Unable to create role with name empty");

                return new BadRequestObjectResult(new { Message = "Role name required" });
            }

            if (await _roleManager.RoleExistsAsync(role))
            {
                _logger.LogWarning("Role with name {role} already exists", role);

                return BadRequest("Role already exists");
            }

            await _roleManager.CreateAsync(new IdentityRole(role));

            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Has been an error trying to create role {role}", role);
            throw;
        }
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginCredentials? credentials)
    {
        try
        {
            IdentityUser identityUser;

            if (!ModelState.IsValid
                || credentials == null
                || (identityUser = await ValidateUser(credentials)) == null)
            {
                _logger.LogWarning("Credentials for user {user} are invalid.", credentials.Username);

                return new BadRequestObjectResult(new { Message = "Login failed" });
            }

            var token = GenerateToken(identityUser);
            return Ok(new { Token = token, Message = "Success" });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Has been an error trying to login user {user}", credentials.Username);
            throw;
        }
    }

    private async Task<IdentityUser?> ValidateUser(LoginCredentials credentials)
    {
        var identityUser = await _userManager.FindByNameAsync(credentials.Username);
        if (identityUser != null)
        {
            var result = _userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, credentials.Password);
            return result == PasswordVerificationResult.Failed ? null : identityUser;
        }

        return null;
    }

    private async Task<object> GenerateToken(IdentityUser identityUser)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtBearerTokenSettings.SecretKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, identityUser.UserName),
            new(ClaimTypes.Email, identityUser.Email)
        };

        claims.AddRange(
            (await _userManager
                .GetRolesAsync(identityUser))
            .Select(role => new Claim(ClaimTypes.Role, role)));

        // Add roles as multiple claims

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),

            Expires = DateTime.UtcNow.AddSeconds(_jwtBearerTokenSettings.ExpiryTimeInSeconds),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _jwtBearerTokenSettings.Audience,
            Issuer = _jwtBearerTokenSettings.Issuer
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
