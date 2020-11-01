using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHackBlazor.Server.Data;
using TestHackBlazor.Shared.DTO;

namespace TestHackBlazor.Server.Areas.Api.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("list-base-info")]
        public async Task<ActionResult<List<UserBaseInfoDTO>>> ListBaseInfo()
        {
            var usersInfo = await _context.Users.AsNoTracking()
                .Include(u => u.Profession)
                .Select(u => new UserBaseInfoDTO
                {
                    FirstName = u.FirstName,
                    SecondName = u.SecondName,
                    Patronymic = u.Patronymic,
                    ProffessionTitle = u.Profession.Title
                })
                .ToListAsync();

            return usersInfo;
        }
    }
}
