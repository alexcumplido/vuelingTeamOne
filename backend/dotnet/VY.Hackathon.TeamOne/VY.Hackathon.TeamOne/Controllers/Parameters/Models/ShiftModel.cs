using System.ComponentModel.DataAnnotations;
using VY.Hackaton.Entities.Enums;

namespace VY.Hackathon.TeamOne.WebApi.Controllers.Parameters.Models
{
    public class ShiftModel
    {
        [Required]
        public ShiftType Shift { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}
