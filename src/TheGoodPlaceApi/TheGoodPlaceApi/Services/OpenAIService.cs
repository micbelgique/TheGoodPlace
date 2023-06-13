
using System.Text.Json;

using System.Text;

public interface IOpenAiService
{
    Task<string> GenerateTextAsync(string prompt);
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

    public async Task<string> GenerateTextAsync(string prompt)
    {
        var requestBody = new
        {
            model = "text-davinci-003", // Le modèle à utiliser
            prompt,
            max_tokens = 800 // Le nombre maximum de tokens à générer
        };

        using (HttpClient client = new HttpClient())
        {
            client.Timeout = TimeSpan.FromMinutes(5);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var responseData = JsonDocument.Parse(responseJson);
                var choicesToken = responseData.RootElement.GetProperty("choices");

                if (choicesToken.ValueKind == JsonValueKind.Array && choicesToken.GetArrayLength() > 0)
                {
                    return choicesToken[0].GetProperty("text").GetString()?.Trim('\n', '"');
                }
                else
                {
                    Console.WriteLine("Une erreur s'est produite lors de la génération du texte.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Une erreur s'est produite lors de la requête HTTP.");
                return null;
            }
        }
    }
}
