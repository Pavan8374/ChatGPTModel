using ChatGPT.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatGPT.API.Controllers
{
    [ApiController]
    [Route("api/home"), Tags("Home")]
    public class HomeController : ControllerBase
    {
        private readonly IChatGPTModel _chatGPT;

        public HomeController(IChatGPTModel chatGPT)
        {
            _chatGPT = chatGPT;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> SendMessage([FromBody] ChatInput input)
        {
            if (input == null || string.IsNullOrEmpty(input.Text))
            {
                return BadRequest("Input text is required.");
            }

            var response = await _chatGPT.GetResponse(input.Text);
            return Ok(new ChatOutput { Text = response });
        }
        public class ChatInput
        {
            public string Text { get; set; }
        }

        public class ChatOutput
        {
            public string Text { get; set; }
        }
    }

    
}
