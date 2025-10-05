using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AiController : ControllerBase
{
    private readonly AiService _aiService;

    public AiController(AiService aiService)
    {
        _aiService = aiService;
    }

    [HttpGet("generate/{topic}")]
    public async Task<IActionResult> GenerateText(string topic)
    {
        if (string.IsNullOrWhiteSpace(topic))
        {
            return BadRequest("A topic must be provided.");
        }

        var generatedText = await _aiService.GenerateCreativeTextAsync(topic);
        
        return Ok(new { topic = topic, message = generatedText });
    }
}
