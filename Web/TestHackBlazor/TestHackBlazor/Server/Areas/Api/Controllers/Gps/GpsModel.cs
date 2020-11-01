using System;
using System.ComponentModel.DataAnnotations;

namespace TestHackBlazor.Server.Areas.Api.Controllers.Gps
{
    public struct GpsModel
    {
        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public long ConstructionId { get; set; }
    }
}
