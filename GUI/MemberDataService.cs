using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class MemberDataService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://retoolapi.dev/OEDUXm/member";

    public MemberDataService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<Member>> GetAllMembersAsync()
    {
        var response = await _httpClient.GetAsync(BaseUrl);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<Member>>(content);
    }

    public async Task<Member> AddMemberAsync(Member member)
    {
        var content = new StringContent(JsonConvert.SerializeObject(member, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(BaseUrl, content);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Member>(responseContent);
    }

public async Task<Member> UpdateMemberAsync(int id, Member member)
    {
        var content = new StringContent(JsonConvert.SerializeObject(member), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{BaseUrl}/{id}", content);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Member>(responseContent);
    }

    public async Task<bool> DeleteMemberAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        return response.IsSuccessStatusCode;
    }
}

public class Member
{
    public int Id { get; set; }
    public string Entry { get; set; }
    public int Rating { get; set; }
    public string Fullname { get; set; }
    public string Interest { get; set; }
}
