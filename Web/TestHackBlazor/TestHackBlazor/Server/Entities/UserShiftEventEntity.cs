using System;
using System.ComponentModel.DataAnnotations;

namespace TestHackBlazor.Server.Entities
{
    public class UserShiftEventEntity
    {
        [Key]
        public long Id { get; set; }

        public DateTimeOffset Begin { get; set; }

        public DateTimeOffset? End { get; set; }

        public DateTimeOffset BeginServer{ get; set; }

        public DateTimeOffset? EndServer { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public long ConstructionId { get; set; }

        public ConstructionEntity Construction { get; set; }
    }
}
