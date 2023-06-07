using BlazorWebAssemblyWithIdentity.Shared;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorWebAssemblyWithIdentity.Client.Services;

public class AuthApiClient : IAuthApi
{
    private readonly HttpClient _httpClient;

    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<UserInfo> GetUserInfoAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<InvokedResult<string>> LoginAsync(LoginModel loginModel)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);
        var result = await response.Content.ReadFromJsonAsync<InvokedResult<string>>();
        Utility.EnsureResultNotNull(result);
        return result!;
    }

    public Task LogoutAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<InvokedResult> RegisterAsync(RegisterModel registerModel)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerModel);
        var result = await response.Content.ReadFromJsonAsync<InvokedResult>();
        Utility.EnsureResultNotNull(result);
        return result!;
    }
}