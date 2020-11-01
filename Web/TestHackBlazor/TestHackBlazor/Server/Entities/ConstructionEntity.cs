using System.Collections.Generic;

namespace TestHackBlazor.Server.Entities
{
    public class ConstructionEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Address { get; set; }

        public List<BorderPointEntity> BorderPoints { get; set; }

        public List<UserShiftEventEntity> Shifts { get; set; }

        public List<GpsTrackEntity> GpsTracks { get; set; }

        public List<EmergencyEntity> Emergencies { get; set; }
    }
}
