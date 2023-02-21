using System.ComponentModel.DataAnnotations;

namespace VY.Hackathon.TeamOne.WebApi.Controllers.Auth.Models
{
    public class SignUpInfo
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}
