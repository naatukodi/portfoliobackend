// Controllers/ExperienceController.cs
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/experience")]
    public class ExperienceController : ControllerBase
    {
        private readonly IExperienceService _svc;
        public ExperienceController(IExperienceService svc) => _svc = svc;

        // GET /api/experience
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Experience>>> GetAll() =>
            Ok(await _svc.GetAllAsync());

        // GET /api/experience/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Experience>> Get(int id)
        {
            var exp = await _svc.GetByIdAsync(id);
            if (exp is null) return NotFound();
            return Ok(exp);
        }

        // POST /api/experience
        [HttpPost]
        public async Task<ActionResult<Experience>> Create([FromBody] Experience dto)
        {
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT /api/experience/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Experience dto)
        {
            await _svc.UpdateAsync(id, dto);
            return NoContent();
        }

        // DELETE /api/experience/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteAsync(id);
            return NoContent();
        }
    }
}
