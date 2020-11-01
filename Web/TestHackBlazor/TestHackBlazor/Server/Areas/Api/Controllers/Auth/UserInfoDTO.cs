using System;

namespace TestHackBlazor.Server.Areas.Api.Controllers.Auth
{
    public class UserInfoDTO
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronymic { get; set; }

        public string ProfessionTitle { get; set; }

        public string Email { get; set; }

        public DateTimeOffset Created { get; set; }

        public Guid ApiKey { get; set; }
    }
}
