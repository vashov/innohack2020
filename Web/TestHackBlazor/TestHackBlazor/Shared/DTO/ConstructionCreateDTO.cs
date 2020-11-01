using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestHackBlazor.Shared.DTO
{
    public class ConstructionCreateDTO
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Code { get; set; }

        public string Address { get; set; }

        [Required]
        public List<BorderPointDTO> BorderPoints { get; set; } = new List<BorderPointDTO>();
    }
}
