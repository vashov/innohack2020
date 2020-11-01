using System;
using System.ComponentModel.DataAnnotations;

namespace TestHackBlazor.Server.Entities
{
    public class GpsTrackEntity
    {
        [Key]
        public long Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public DateTimeOffset Date { get; set; }

        public long ConstructionId { get; set; }

        public ConstructionEntity Construction { get; set; }

    }
}
