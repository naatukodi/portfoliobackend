// Controllers/SkillController.cs
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/skill")]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _svc;
        public SkillController(ISkillService svc) => _svc = svc;

        // GET /api/skill
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Skill>>> GetAll() =>
            Ok(await _svc.GetAllAsync());

        // GET /api/skill/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Skill>> Get(int id)
        {
            var s = await _svc.GetByIdAsync(id);
            if (s is null) return NotFound();
            return Ok(s);
        }

        // POST /api/skill
        [HttpPost]
        public async Task<ActionResult<Skill>> Create([FromBody] Skill dto)
        {
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT /api/skill/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Skill dto)
        {
            await _svc.UpdateAsync(id, dto);
            return NoContent();
        }

        // DELETE /api/skill/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteAsync(id);
            return NoContent();
        }
    }
}
