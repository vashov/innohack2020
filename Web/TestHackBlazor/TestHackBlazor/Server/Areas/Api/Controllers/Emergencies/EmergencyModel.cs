using System;
using System.ComponentModel.DataAnnotations;
using TestHackBlazor.Shared.Models;

namespace TestHackBlazor.Server.Areas.Api.Controllers.Emergencies
{
    public class EmergencyModel
    {
        [Required]
        public EmergencyType Type { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public long ConstructionId { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }
    }
}
