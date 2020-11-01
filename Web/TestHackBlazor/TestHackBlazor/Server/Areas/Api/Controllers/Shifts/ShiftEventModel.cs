using System;
using System.ComponentModel.DataAnnotations;

namespace TestHackBlazor.Server.Areas.Api.Controllers.Shifts
{
    public class ShiftEventModel
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public ShiftEventType Type { get; set; }

        [Required]
        public long ConstructionId { get; set; }
    }

    public enum ShiftEventType
    {
        None = 0,
        Begin = 1,
        End = 2
    }
}
