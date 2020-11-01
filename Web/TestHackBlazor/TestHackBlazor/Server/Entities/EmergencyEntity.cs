using System;
using System.ComponentModel.DataAnnotations;
using TestHackBlazor.Shared.Models;

namespace TestHackBlazor.Server.Entities
{
    public class EmergencyEntity
    {
        [Key]
        public long Id { get; set; }

        public EmergencyType Type { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Inserted { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public long ConstructionId { get; set; }

        public ConstructionEntity Construction { get; set; }

        public bool Checked { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }

    
}
