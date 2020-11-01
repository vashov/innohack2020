using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHackBlazor.Server.Data;
using TestHackBlazor.Server.Entities;
using TestHackBlazor.Shared.DTO;
using TestHackBlazor.Shared.Extensions;

namespace TestHackBlazor.Server.Areas.Api.Controllers.Emergencies
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmergencyController : ApiAuthController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EmergencyController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("[action]")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public async Task<IActionResult> Add(
            [FromHeader(Name = "api-key")] string apiKey,
            [FromBody] List<EmergencyModel> model)
        {
            if (model.IsNullOrEmpty() || model.Any(e => e.ConstructionId < 1))
            {
                return ValidationProblem();
            }

            string userId = User.FindFirst(AppClaimTypes.UserId).Value;

            List<EmergencyEntity> emergencies = model.Select(e =>
                new EmergencyEntity
                {
                    UserId = userId,
                    Created = e.Date,
                    Type = e.Type,
                    ConstructionId = e.ConstructionId,
                    Latitude = e.Latitude,
                    Longitude = e.Longitude
                })
                .ToList();

            await _context.Emergencies.AddRangeAsync(emergencies);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("[action]")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public async Task<IActionResult> List(
            [FromHeader(Name = "api-key")] string apiKey)
        {
            string userId = User.FindFirst(AppClaimTypes.UserId).Value;

            var emergencies = await _context.Emergencies
                .AsNoTracking()
                .Where(t => t.UserId == userId)
                .Select(e => new {e.Id, e.Created, e.Inserted, e.Type, e.Checked, e.Latitude, e.Longitude })
                .OrderByDescending(t => t.Created)
                .ToListAsync();

            return Ok(emergencies);
        }

        [AllowAnonymous]
        [HttpGet("list-all")]
        public async Task<ActionResult<List<EmergencyDTO>>> ListAll()
        {
            var emergencies = await _context.Emergencies
                .AsNoTracking()
                .Include(e => e.User)
                .Include(e => e.Construction)
                .OrderByDescending(t => t.Created)
                .ToListAsync();

            var emergDTOs = _mapper.Map<List<EmergencyDTO>>(emergencies);

            return emergDTOs;
        }

        [AllowAnonymous]
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<EmergencyDTO>> Get([FromRoute] long id)
        {
            if (id < 1)
                return ValidationProblem();

            var emergency = await _context.Emergencies
                .AsNoTracking()
                .Include(e => e.User)
                .Include(e => e.Construction)
                    .ThenInclude(e => e.BorderPoints)
                .OrderByDescending(t => t.Created)
                .SingleOrDefaultAsync(e => e.Id == id);

            var emergDTO = _mapper.Map<EmergencyDTO>(emergency);

            return emergDTO;
        }
    }
}
