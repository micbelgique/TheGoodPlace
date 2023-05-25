using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;

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
        dynamic requestBody = new
        {
            prompt,
            max_tokens = 800
        };

        string requestBodyJson = JsonConvert.SerializeObject(requestBody, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            var content = new StringContent(requestBodyJson.ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_apiUrl, content);

            string responseJson = await response.Content.ReadAsStringAsync();

            JObject responseData = JObject.Parse(responseJson);
            JToken choicesToken = responseData["choices"];
            if (choicesToken != null && choicesToken.HasValues)
            {
                return choicesToken[0]["text"].Value<string>().Trim('\n', '"');
            }
            else
            {
                Console.WriteLine("Une erreur s'est produite lors de la génération du texte.");
                return null;
            }
        }
    }
}
