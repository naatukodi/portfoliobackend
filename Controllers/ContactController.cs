// Controllers/ContactController.cs
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/contact")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _svc;
        public ContactController(IContactService svc) => _svc = svc;

        // GET /api/contact
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetAll() =>
            Ok(await _svc.GetAllAsync());

        // GET /api/contact/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Contact>> Get(int id)
        {
            var c = await _svc.GetByIdAsync(id);
            if (c is null) return NotFound();
            return Ok(c);
        }

        // POST /api/contact
        [HttpPost]
        public async Task<ActionResult<Contact>> Create([FromBody] Contact dto)
        {
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT /api/contact/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Contact dto)
        {
            await _svc.UpdateAsync(id, dto);
            return NoContent();
        }

        // DELETE /api/contact/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteAsync(id);
            return NoContent();
        }
    }
}
