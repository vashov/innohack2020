using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHackBlazor.Server.Data;
using TestHackBlazor.Server.Entities;
using TestHackBlazor.Shared.Extensions;

namespace TestHackBlazor.Server.Areas.Api.Controllers.Gps
{
    [ApiController]
    [Route("api/[controller]")]
    public class GpsController : ApiAuthController
    {
        private readonly ApplicationDbContext _context;

        public GpsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("[action]")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]

        public async Task<IActionResult> Add(
            [FromHeader(Name = "api-key")] string apiKey,
            [FromBody] List<GpsModel> model)
        {
            if (model.IsNullOrEmpty() || model.Any(m => m.ConstructionId < 1))
            {
                return ValidationProblem();
            }

            string userId = User.FindFirst(AppClaimTypes.UserId).Value;

            List<GpsTrackEntity> tracks = model.Select(g => 
                new GpsTrackEntity
                {
                    UserId = userId,
                    Latitude = g.Latitude,
                    Longitude = g.Longitude,
                    Date = g.Date,
                    ConstructionId = g.ConstructionId
                })
                .ToList();

            await _context.GpsTracks.AddRangeAsync(tracks);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("[action]")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public async Task<IActionResult> List(
            [FromHeader(Name = "api-key")] string apiKey)
        {
            string userId = User.FindFirst(AppClaimTypes.UserId).Value;

            var tracks = await _context.GpsTracks
                .Where(t => t.UserId == userId)
                .Select(t => new { t.Id, t.Date, t.Latitude, t.Longitude})
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            return Ok(tracks);
        }
    }
}
