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
    public class Itinerary
    {
        public string? Destination { get; set; }
        public int Duration { get; set; }
        public DailyPlan[]? DailyPlans { get; set; }
    }

    public class DailyPlan
    {
        public int Day { get; set; }
        public string? Theme { get; set; }
        public Activity[]? Activities { get; set; }
    }

    public class Activity
    {
        public string? Time { get; set; }
        public string? Description { get; set; }
    }


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
   
public async Task<string> GenerateItineraryAsync(string destination, int days)
{
    var prompt = $@"
        Generate a travel itinerary for a trip to {destination} for {days} days.
        Respond ONLY with a JSON object. Do not include any text, headers, or code block markers before or after the JSON.
        The JSON object must strictly follow this structure:
        {{
            ""destination"": ""{destination}"",
            ""duration"": {days},
            ""dailyPlans"": [
                {{
                    ""day"": 1,
                    ""theme"": ""Arrival and Exploration"",
                    ""activities"": [
                        {{ ""time"": ""Afternoon"", ""description"": ""Arrive at the hotel and check in."" }},
                        {{ ""time"": ""Evening"", ""description"": ""Explore the local market and have dinner."" }}
                    ]
                }}
            ]
        }}
        Ensure the JSON is well-formed and complete.
    ";

    var requestData = new GeminiRequest
    {
        contents = new[] { new Content { parts = new[] { new Part { text = prompt } } } }
    };

    var response = await _httpClient.PostAsJsonAsync(_apiUrl, requestData);

    if (response.IsSuccessStatusCode)
    {
        var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
        var rawResponseText = geminiResponse?.candidates?[0]?.content?.parts?[0]?.text;

        if (string.IsNullOrWhiteSpace(rawResponseText))
        {
            return "{{ \"error\": \"AI returned an empty response.\" }}";
        }

        // Robustly find and extract the JSON object from the AI's response
        var startIndex = rawResponseText.IndexOf('{');
        var endIndex = rawResponseText.LastIndexOf('}');

        if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
        {
            return rawResponseText.Substring(startIndex, endIndex - startIndex + 1);
        }

        return "{{ \"error\": \"Failed to parse JSON from AI response.\" }}";
    }

    return "{{ \"error\": \"Failed to generate itinerary.\" }}";
}

}
