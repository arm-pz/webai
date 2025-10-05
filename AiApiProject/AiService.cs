using System.Net.Http.Json;

// We can reuse the same model classes from our previous project
public class GeminiRequest { public Content[]? contents { get; set; } }
public class Content { public Part[]? parts { get; set; } }
public class Part { public string? text { get; set; } }
public class GeminiResponse { public Candidate[]? candidates { get; set; } }
public class Candidate { public Content? content { get; set; } }

public class AiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";


    public AiService(IConfiguration configuration)
    {
        _apiKey = configuration["ApiKey"] ?? throw new ArgumentNullException("API Key not found!");
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);
    }

    public async Task<string> GenerateCreativeTextAsync(string topic)
    {
        var prompt = $"Generate a creative, friendly, one-sentence message about {topic}.";

        var requestData = new GeminiRequest
        {
            contents = new[]
            {
                new Content { parts = new[] { new Part { text = prompt } } }
            }
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync(_apiUrl, requestData);

            if (response.IsSuccessStatusCode)
            {
                var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
                string? aiMessage = geminiResponse?.candidates?[0]?.content?.parts?[0]?.text;
                return string.IsNullOrEmpty(aiMessage) ? "Could not generate a message." : aiMessage.Trim();
            }
            else
            {
                // In a real app, you'd log this error content
                return $"API Error: {response.StatusCode}";
            }
        }
        catch (Exception ex)
        {
            // In a real app, you'd log this exception
            return $"An exception occurred: {ex.Message}";
        }
    }
}
