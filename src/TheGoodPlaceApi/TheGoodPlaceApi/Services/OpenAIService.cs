
using System.Text.Json;

using System.Text;
using TheGoodPlaceApi.Models.Dto;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using TheGoodPlaceApi.Models.OpenAI;
using System.Net;

public interface IOpenAiService
{
    Task<string> GenerateTextAsync(string prompt);
    Task<RoomRankingResponseDto> GetRoomRanking(string systemPrompt, string userPrompt);
}

public class OpenAiService : IOpenAiService
{
    private readonly string _apiKey;
    private readonly string _apiUrl;

    public OpenAiService(string apiKey, string apiUrl)
    {
        _apiKey = apiKey;
        _apiUrl = apiUrl;
    }

    public Task<string> GenerateTextAsync(string prompt)
    {
        throw new NotImplementedException();
    }

    public async Task<RoomRankingResponseDto> GetRoomRanking(string systemPrompt, string userPrompt)
    {
        var requestObject = new RootObject
        {
            model = "deepwork",
            response_format = new ResponseFormat { type = "json_object" },
            messages = new List<Message>
                {
                    new Message
                    {
                        role = "system",
                        content = systemPrompt
                    },
                    new Message
                    {
                        role = "user",
                        content = userPrompt
                    }

                },
            functions = new List<Functions>
                {
                    new Functions
                    {
                        name = "calculate_wellness_value",
                        description = "Calcule la wellnessvalue et donne une justification",
                        parameters = new ParametersParameters
                        {
                            type = "object",
                            properties = new Properties2
                            {
                                Salles = new Salles()

                            },
                            required = new List<string> { "salles" }
                        }
                    }
                },
            function_call = "auto",
            temperature = 0.75
        };

        Console.WriteLine($"System Prompt: {systemPrompt}");
        Console.WriteLine($"User Prompt: {userPrompt}");

        var jsonRequest = JsonSerializer.Serialize(requestObject,
             new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

        Console.WriteLine($"JSON Request: {jsonRequest}");

        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(_apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var completionResponse = JsonSerializer.Deserialize<OpenAiCompletionRequestDto>(responseContent);

                var roomsResponse = completionResponse.choices[0].message.function_call.arguments;


                RoomRankingResponseDto rooms = JsonSerializer.Deserialize<RoomRankingResponseDto>(roomsResponse,
                        new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                return rooms;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erreur : {response.StatusCode}");
                Console.WriteLine($"Contenu de l'erreur : {errorContent}");
                return null;
            }


        }

    }
    
}
   
   
