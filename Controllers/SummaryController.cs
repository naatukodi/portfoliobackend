// Controllers/SummaryController.cs
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/summary")]
    public class SummaryController : ControllerBase
    {
        private readonly ISummaryService _svc;
        public SummaryController(ISummaryService svc) => _svc = svc;

        /// <summary>
        /// Get the resume summary (name, title, contact, bio).
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<Summary>> Get()
        {
            var summary = await _svc.GetAsync();
            return Ok(summary);
        }

        /// <summary>
        /// Update the resume summary.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Summary dto)
        {
            await _svc.UpdateAsync(dto);
            return NoContent();
        }
    }
}
