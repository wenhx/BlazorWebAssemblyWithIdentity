using BlazorWebAssemblyWithIdentity.Shared;
using System.Net.Http;
using System.Net.Http.Json;

namespace BlazorWebAssemblyWithIdentity.Client.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IList<UserInfo>> GetUsersAsync(int page, int pageSize)
    {
        var apiUri = $"/api/users?page={page}&pageSize={pageSize}";
        return (await _httpClient.GetFromJsonAsync<List<UserInfo>>(apiUri)) ?? new List<UserInfo>();
    }
}