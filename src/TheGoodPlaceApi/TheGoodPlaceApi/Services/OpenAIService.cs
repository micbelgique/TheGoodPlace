
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
                                Salles = new Salles
                                {
                                    Type = "array",
                                    Items = new Item
                                    {
                                        Type = "object",
                                        Properties = new Properties1
                                        {
                                            Name = new Name { Type = "string", Description = "Nom de la salle" },
                                            Capacity = new Capacity { Type = "string", Description = "Nombre de personne que peut acceuillir la salle" },                         
                                            WellnessValue = new WellnessValue { Type = "string", Description = "Nombre de 1 à 100 du bien etre, plus les temperature, l'humidité et la pression sont propice à travailler plus sa wellnessvalue sera grande" },
                                            Temperature = new Temperature { Type = "string", Description = "La température de la salle en Celcius" },
                                            Humidity = new Humidity { Type = "string", Description = "Le niveau d'humidité de la salle en %" },
                                            Justification = new Justification { Type = "string", Description = "La justification du score de bien-être de calcul en une phrase" },
                                            PictureUrl = new PictureUrl { Type = "string", Description = "L'url de l'image de la salle" }
                                        },
                                        Required = new List<string> { "name", "capacity", "wellnessValue", "temperature", "humidity", "justification", "PictureUrl" }
                                    }
                                }
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
   
   
