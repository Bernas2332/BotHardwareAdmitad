using System.Net.Http.Headers;
using System.Text.Json;

namespace BotHardware;

public class AdmitadClient
{
    private readonly HttpClient _httpClient = new HttpClient();

    public async Task<WebmasterResponse?> GetWebsitesParsedAsync()
{
    var json = await GetWebsitesAsync();
    // Aqui acontece a mágica: transforma string em objeto C#
    return JsonSerializer.Deserialize<WebmasterResponse>(json);
}

    public AdmitadClient(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<string> GetWebsitesAsync()
    {
        var response = await _httpClient.GetAsync("https://api.admitad.com/websites/");
        return await response.Content.ReadAsStringAsync();
    }
}