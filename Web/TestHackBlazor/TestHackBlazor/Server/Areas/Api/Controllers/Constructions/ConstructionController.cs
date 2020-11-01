using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestHackBlazor.Server.Areas.Api.Controllers.Constructions.Models;
using TestHackBlazor.Server.Data;
using TestHackBlazor.Server.Entities;
using TestHackBlazor.Server.Infrastructure.ApiKeys;
using TestHackBlazor.Shared.DTO;
using System.Linq;

namespace TestHackBlazor.Server.Areas.Api.Controllers.Constructions
{
    [Route("api/[controller]")]
    public class ConstructionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ConstructionController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<ConstructionDTO>>> List()
        {
            var result = await _context.Constructions
                .AsNoTracking()
                .Include(c => c.BorderPoints)
                .ToListAsync();

            var constructions = _mapper.Map<List<ConstructionDTO>>(result);

            return constructions;
        }

        [HttpGet("[action]/{constructionId}")]
        public async Task<ActionResult<ConstructionDTO>> Get([FromRoute] long constructionId)
        {
            if (constructionId < 1)
                return ValidationProblem();

            var result = await _context.Constructions
                .AsNoTracking()
                .Include(c => c.BorderPoints)
                .SingleOrDefaultAsync(c => c.Id == constructionId);

            var construction = _mapper.Map<ConstructionDTO>(result);

            return construction;
        }

        [HttpGet("list-base-info")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public async Task<ActionResult<List<ConstructionBase>>> ListBaseInfo([FromHeader(Name = "api-key")] string apiKey)
        {
            var result = await _context.Constructions.AsNoTracking().ToListAsync();

            var baseResult = _mapper.Map<List<ConstructionBase>>(result);

            return baseResult;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Add([FromBody] ConstructionCreateDTO model)
        {
            var construction = _mapper.Map<ConstructionEntity>(model);

            await _context.Constructions.AddAsync(construction);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Update([FromBody] ConstructionDTO model)
        {
            var construction = _mapper.Map<ConstructionEntity>(model);

            using var transaction = await _context.Database.BeginTransactionAsync();

            List<BorderPointEntity> oldBorders = 
                await _context.BorderPoints.Where(b => b.ConstructionId == model.Id).ToListAsync();

            _context.BorderPoints.RemoveRange(oldBorders);

            _context.Constructions.Update(construction);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok();
        }
    }
}
