
using System.Text.Json;

using System.Text;
using TheGoodPlaceApi.Models.Dto;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using TheGoodPlaceApi.Models.OpenAI;

public interface IOpenAiService
{
    Task<string> GenerateTextAsync(string prompt);
    Task<RoomRankingResponseDto> GetRoomRanking(string prompt);
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

    public async Task<RoomRankingResponseDto> GetRoomRanking(string prompt)
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
                        content = "Tu es un assistant qui va faire un ranking des salles en calculant la wellnessvalue et tu vas la justifier le tout dans JSON format."
                    },
                    new Message
                    {
                        role = "user",
                        content = $"{prompt}"
                    }

                },
            functions = new List<Functions>
                {
                    new Functions
                    {
                        name = "calculate_wellness_value",
                        description = "Calcule la valeur de bien-être pour chaque salle en fonction de la température",
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
                                            Name = new Name { Type = "string", Description = "Nom de la salle de travail" },
                                            Capacity = new Capacity { Type = "string", Description = "Capacité de la salle de travail" },
                                            PictureUrl = new PictureUrl { Type = "string", Description = "URL de l'image de la salle de travail" },
                                            WellnessValue = new WellnessValue { Type = "string", Description = "Valeur de bien-être de la salle de travail" },
                                            Temperature = new Temperature { Type = "string", Description = "Température de la salle de travail" },
                                            Humidity = new Humidity { Type = "string", Description = "Humidité de la salle de travail" },
                                            Justification = new Justification { Type = "string", Description = "Justification de la valeur de bien-être de la salle de travail" }
                                        },
                                        Required = new List<string> { "name", "capacity", "pictureUrl", "wellnessValue", "temperature", "humidity", "justification" }
                                    }
                                }
                            },
                            required = new List<string> { "salles" }
                        }
                    }
                },
            function_call = "auto",

            temperature = 0.7
        };


        var jsonRequest = JsonSerializer.Serialize(requestObject,
             new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });



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
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erreur : {response.StatusCode}");
                Console.WriteLine($"Contenu de l'erreur : {errorContent}");
            }


        }


        return new RoomRankingResponseDto();
    }
    
}
   
   
