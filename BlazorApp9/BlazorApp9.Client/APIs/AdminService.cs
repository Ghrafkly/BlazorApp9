using System.Net.Http.Json;

namespace BlazorApp9.Client.APIs;
// WEQ
public class AdminService : IAdminService
{
    private readonly HttpClient _httpClient;

    public AdminService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserInfo>> GetAllUsers()
    {
        return await _httpClient.GetFromJsonAsync<List<UserInfo>>("api/Admin/GetAllUsers");
    }

    public async Task<UserInfo> GetUser(string id)
    {
        return await _httpClient.GetFromJsonAsync<UserInfo>($"api/Admin/GetUser/{id}");
    }
}

