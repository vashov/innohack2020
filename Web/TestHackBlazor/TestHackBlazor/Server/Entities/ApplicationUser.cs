using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestHackBlazor.Server.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTimeOffset Created { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ApiKey { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Patronymic { get; set; }

        public long ProfessionId { get; set; }
        public ProfessionEntity Profession { get; set; }

        public IList<GpsTrackEntity> GpsTracks { get; set; }
        public IList<EmergencyEntity> Emergencies { get; set; }
        public IList<UserShiftEventEntity> UserShiftsEvents { get; set; }

    }
}
