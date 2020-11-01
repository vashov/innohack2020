using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestHackBlazor.Server.Areas.Api.Controllers.Constructions.Models
{
    public class ConstructionCreateModel
    {
        [Required(AllowEmptyStrings =false)]
        public string Name { get; set; }

        [Required]
        public List<BorderPointModel> BorderPoints { get; set; }
    }
}
