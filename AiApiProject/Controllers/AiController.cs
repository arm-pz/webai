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
    /// <summary>
    /// Generates a travel itinerary for a specified destination and number of days.
    /// </summary>
    /// <param name="destination">The destination for which the itinerary is to be generated.</param>
    /// <param name="days">The number of days for the itinerary.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the generated itinerary in JSON format,
    /// or a <see cref="BadRequestResult"/> if the input parameters are invalid.
    /// </returns>
    /// <remarks>
    /// This method uses an AI service to generate the itinerary asynchronously.
    /// Ensure that the destination is not null or empty and the number of days is greater than zero.
    /// </remarks>
    [HttpGet("generate/itinerary/{destination}/{days}")]
    public async Task<IActionResult> GenerateItinerary(string destination, int days)
    {
        if (string.IsNullOrWhiteSpace(destination) || days <= 0)
        {
            return BadRequest("A valid destination and number of days must be provided.");
        }

        var itineraryJson = await _aiService.GenerateItineraryAsync(destination, days);

        // We return the raw JSON string we get from the AI
        return Content(itineraryJson, "application/json");
    }

}
