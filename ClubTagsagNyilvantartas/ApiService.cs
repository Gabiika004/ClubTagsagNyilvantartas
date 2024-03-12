using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ClubTagsagNyilvantartas; // Feltételezve, hogy ez a névtér tartalmazza a Member osztályt

public class ApiService
{
    private readonly HttpClient _client = new HttpClient();

    public ApiService()
    {
        _client.BaseAddress = new Uri("https://retoolapi.dev/OEDUXm/member");
    }

    // Olvasás (Read)
    public async Task<Member[]> GetAllMembersAsync()
    {
        try
        {
            var response = await _client.GetAsync("");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return Member.FromJson(content);
        }
        catch(HttpRequestException e)
        {
            Console.WriteLine($"Hiba történt a kéréskor: {e.Message}");
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Hiba történt a tagok lekérésekor: {e.Message}");
            return null;
        }
    }
}
