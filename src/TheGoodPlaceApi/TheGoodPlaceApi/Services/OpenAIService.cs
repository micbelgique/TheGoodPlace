
using System.Text.Json;

using System.Text;
using TheGoodPlaceApi.Models.Dto;

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
        // Traitement pour récupérer uniquement le RoomRankingResponse
        return new RoomRankingResponseDto();
    }
}