// Controllers/ProjectController.cs
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/project")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _svc;
        public ProjectController(IProjectService svc) => _svc = svc;

        // GET /api/project
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetAll() =>
            Ok(await _svc.GetAllAsync());

        // GET /api/project/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Project>> Get(int id)
        {
            var p = await _svc.GetByIdAsync(id);
            if (p is null) return NotFound();
            return Ok(p);
        }

        // POST /api/project
        [HttpPost]
        public async Task<ActionResult<Project>> Create([FromBody] Project dto)
        {
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT /api/project/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Project dto)
        {
            await _svc.UpdateAsync(id, dto);
            return NoContent();
        }

        // DELETE /api/project/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteAsync(id);
            return NoContent();
        }
    }
}
