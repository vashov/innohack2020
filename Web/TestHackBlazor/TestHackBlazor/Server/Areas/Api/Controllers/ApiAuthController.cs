using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestHackBlazor.Server.Infrastructure.ApiKeys;

namespace TestHackBlazor.Server.Areas.Api.Controllers
{
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
    public class ApiAuthController : ControllerBase
    {
    }
}
