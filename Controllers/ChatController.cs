// Controllers/ChatController.cs
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Services;
using Portfolio.Api.Models;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _svc;
        public ChatController(IChatService svc) => _svc = svc;

        /// <summary>
        /// Ask a question about me.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ChatResponse>> Post([FromBody] ChatRequest req)
        {
            var resp = await _svc.AskAsync(req.Question);
            return Ok(resp);
        }
    }
}
