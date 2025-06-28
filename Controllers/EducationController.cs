// Controllers/EducationController.cs
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/education")]
    public class EducationController : ControllerBase
    {
        private readonly IEducationService _svc;
        public EducationController(IEducationService svc) => _svc = svc;

        // GET /api/education
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Education>>> GetAll() =>
            Ok(await _svc.GetAllAsync());

        // GET /api/education/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Education>> Get(int id)
        {
            var ed = await _svc.GetByIdAsync(id);
            if (ed is null) return NotFound();
            return Ok(ed);
        }

        // POST /api/education
        [HttpPost]
        public async Task<ActionResult<Education>> Create([FromBody] Education dto)
        {
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT /api/education/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Education dto)
        {
            await _svc.UpdateAsync(id, dto);
            return NoContent();
        }

        // DELETE /api/education/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteAsync(id);
            return NoContent();
        }
    }
}
