# AiApiProject

AiApiProject is a simple ASP.NET Core Web API that uses Google Generative Language API to generate creative text based on a given topic. The project demonstrates how to integrate external AI services into a web application.

## Features

- Generate creative, friendly, one-sentence messages about a given topic.
- RESTful API with endpoints for text generation.
- Swagger/OpenAPI integration for easy API exploration and testing.

## Project Structure

AiApiProject/ ├── Controllers/ │ └── AiController.cs # API controller for handling requests ├── AiService.cs # Service for interacting with the AI API ├── Program.cs # Entry point and configuration ├── appsettings.json # Application configuration ├── appsettings.Development.json # Development-specific configuration ├── AiApiProject.http # HTTP file for testing API endpoints └── Properties/ └── launchSettings.json # Launch settings for debugging


## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A valid API key for Google Generative Language API.

## Getting Started

### 1. Clone the Repository


git clone <repository-url>
cd AiApiProject

### 2. Configure the API Key
Add your API key to the appsettings.json or appsettings.Development.json file:

Alternatively, you can set the ApiKey <vscode_annotation details='%5B%7B%22title%22%3A%22hardcoded-credentials%22%2C%22description%22%3A%22Embedding%20credentials%20in%20source%20code%20risks%20unauthorized%20access%22%7D%5D'> your</vscode_annotation>in environment variables.

### 3. Run the Application
Use the following command to run the application:

The application will start on http://localhost:5005 by default.

### 4. Test the API
You can test the API using the provided AiApiProject.http file or Swagger UI:

Swagger UI: Navigate to http://localhost:5005/swagger in your browser.
HTTP File: Use an HTTP client like REST Client in Visual Studio Code to send requests.
Example request:

Replace {topic} with your desired topic.

API Endpoints
GET /ai/generate/{topic}
Generates a creative message about the given topic.

Path Parameter: topic (string) - The topic for the AI to generate a message about.
Response: JSON object containing the topic and the generated message.
Example response:

Dependencies
Microsoft.AspNetCore.OpenApi
Swashbuckle.AspNetCore
License
This project is licensed under the MIT License. See the LICENSE file for details.

Acknowledgments
Google Generative Language API
ASP.NET Core
