using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TestHackBlazor.Server.Data;
using TestHackBlazor.Server.Entities;

namespace TestHackBlazor.Server.Areas.Api.Controllers.Auth
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AuthController(
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public IActionResult Test()
        {
            return Ok();
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            ApplicationUser user = await _context.Users
                .Include(e => e.Profession)
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
                return BadRequest();

            bool passwordChecked = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordChecked)
                return Forbid();

            var userDTO = _mapper.Map<ApplicationUser, UserInfoDTO>(user);

            userDTO.ProfessionTitle = user.Profession.Title;

            //var userDTO = new UserInfoDTO
            //{
            //    Email = user.Email,
            //    Created = user.Created,
            //    ApiKey = user.ApiKey
            //};

            return Ok(userDTO);
        }
    }
}
