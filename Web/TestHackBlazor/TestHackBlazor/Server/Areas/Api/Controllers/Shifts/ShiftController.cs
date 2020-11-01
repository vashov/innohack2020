using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestHackBlazor.Server.Data;
using TestHackBlazor.Server.Entities;

namespace TestHackBlazor.Server.Areas.Api.Controllers.Shifts
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShiftController : ApiAuthController
    {
        private readonly ApplicationDbContext _context;

        public ShiftController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("[action]")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public async Task<IActionResult> Add(
            [FromHeader(Name = "api-key")] string apiKey,
            [FromBody] ShiftEventModel model)
        {
            if (model.Type == ShiftEventType.None || model.ConstructionId < 1)
                return ValidationProblem();

            string userId = User.FindFirst(AppClaimTypes.UserId).Value;

            if (model.Type == ShiftEventType.Begin)
            {
                UserShiftEventEntity shiftEvent = await _context.UserShiftEvents
                    .AsNoTracking()
                    .Where(e => e.UserId == userId)
                    .Where(e => e.EndServer == null)
                    .SingleOrDefaultAsync();

                if (shiftEvent != null)
                    return BadRequest("There is started shift for this user already.");

                UserShiftEventEntity shift = new UserShiftEventEntity
                {
                    Begin = model.Date,
                    BeginServer = DateTimeOffset.UtcNow,
                    UserId = userId,
                    ConstructionId = model.ConstructionId
                };

                await _context.UserShiftEvents.AddAsync(shift);
                await _context.SaveChangesAsync();
                return Ok();
            }

            if (model.Type == ShiftEventType.End)
            {
                UserShiftEventEntity shiftEvent = await _context.UserShiftEvents
                    .Where(e => e.UserId == userId)
                    .Where(e => e.EndServer == null)
                    .SingleOrDefaultAsync();

                if (shiftEvent == null)
                    return BadRequest("There is no started shift for this user.");


                shiftEvent.End = model.Date;
                shiftEvent.EndServer = DateTimeOffset.UtcNow;

                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("Unsupported shift event type.");
        }

        [HttpGet("[action]")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public async Task<IActionResult> List(
            [FromHeader(Name = "api-key")] string apiKey)
        {
            string userId = User.FindFirst(AppClaimTypes.UserId).Value;

            var shifts = await _context.UserShiftEvents
                .Where(t => t.UserId == userId)
                .Select(t => new { t.Id, t.Begin, t.BeginServer, t.End, t.EndServer})
                .OrderByDescending(t => t.Begin)
                .ToListAsync();

            return Ok(shifts);
        }

        [HttpGet("is-started")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public async Task<ActionResult<bool>> IsStarted(
            [FromHeader(Name = "api-key")] string apiKey)
        {
            string userId = User.FindFirst(AppClaimTypes.UserId).Value;

            UserShiftEventEntity shiftEvent = await _context.UserShiftEvents
                    .AsNoTracking()
                    .Where(e => e.UserId == userId)
                    .Where(e => e.EndServer == null)
                    .SingleOrDefaultAsync();

            bool shiftStarted = shiftEvent != null;

            return shiftStarted;
        }
    }
}
