using System.ComponentModel.DataAnnotations;

namespace TestHackBlazor.Server.Entities
{
    public class BorderPointEntity
    {
        [Key]
        public long Id { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public int Order { get; set; }

        public long ConstructionId { get; set; }

        public ConstructionEntity Construction { get; set; }
    }
}
