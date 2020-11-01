using System;
using TestHackBlazor.Shared.Models;

namespace TestHackBlazor.Shared.DTO
{
    public class EmergencyDTO
    {
        public long Id { get; set; }

        public EmergencyType Type { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Inserted { get; set; }

        public string UserId { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public UserBaseInfoDTO User { get; set; }

        public ConstructionDTO Construction { get; set; }

        public bool Checked { get; set; }
    }
}
