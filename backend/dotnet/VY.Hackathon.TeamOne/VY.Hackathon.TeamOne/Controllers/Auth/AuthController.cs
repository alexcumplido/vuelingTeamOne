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

namespace VY.Hackathon.TeamOne.WebApi.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly JwtBearerTokenSettings _jwtBearerTokenSettings;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthController(
        IOptions<JwtBearerTokenSettings> jwtTokenOptions,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _jwtBearerTokenSettings = jwtTokenOptions.Value;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost]
    [Route("SignUp")]
    public async Task<IActionResult> SignUp([FromBody] SignUpInfo? signUpInfo)
    {
        if (!ModelState.IsValid || signUpInfo == null)
        {
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

            return new BadRequestObjectResult(new { Message = "User Registration Failed", Errors = dictionary });
        }

        foreach (var role in signUpInfo.Roles)
        {
            if (await _roleManager.RoleExistsAsync(role))
            {
                IdentityResult roleResult = await _userManager.AddToRoleAsync(identityUser, role);
            }
        }

        return Ok(new { Message = "User Registration Successful" });
    }

    [HttpPost]
    [Route("Role/{role}")]
    public async Task<IActionResult> CreateRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            return new BadRequestObjectResult(new { Message = "Role name required" });
        }

        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(new IdentityRole(role));
        }

        return NoContent();
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginCredentials? credentials)
    {
        IdentityUser identityUser;

        if (!ModelState.IsValid
            || credentials == null
            || (identityUser = await ValidateUser(credentials)) == null)
        {
            return new BadRequestObjectResult(new { Message = "Login failed" });
        }

        var token = GenerateToken(identityUser);
        return Ok(new { Token = token, Message = "Success" });
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
