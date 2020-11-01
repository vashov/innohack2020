using System.ComponentModel.DataAnnotations;

namespace TestHackBlazor.Server.Areas.Api.Controllers.Auth
{
    public class UserLoginModel
    {
        [Required(AllowEmptyStrings =false)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings =false)]
        public string Password { get; set; }
    }
}
