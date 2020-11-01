using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestHackBlazor.Server.Entities
{
    public class ProfessionEntity
    {
        [Key]
        public long Id { get; set; }

        public string Title { get; set; }

        public List<ApplicationUser> Users { get; set; }
    }
}
