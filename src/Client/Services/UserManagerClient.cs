using BlazorWebAssemblyWithIdentity.Shared;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorWebAssemblyWithIdentity.Client.Services;

public class UserManagerClient : IUserManagerApi
{
    private static readonly string _unknownErrorMessage = "Unknown error occurred.";
    private readonly HttpClient _httpClient;

    public UserManagerClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<UserInfo> GetUserInfoAsync()
    {
        throw new NotImplementedException();
    }

    public Task LoginAsync(LoginModel loginModel)
    {
        throw new NotImplementedException();
    }

    public Task LogoutAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<InvokedResult> RegisterAsync(RegisterModel registerModel)
    {
        var result = await _httpClient.PostAsJsonAsync("api/user/register", registerModel);
        try
        {
            var jsonResponse = await result.Content.ReadFromJsonAsync<JsonResponse>();
            if (jsonResponse != null)
            {
                if (jsonResponse.Status == JsonResponseStatus.Success)
                    return InvokedResult.Success;
                else
                {
                    if (jsonResponse.Data != null)
                    {
                        var jsonElement = (JsonElement)jsonResponse.Data;
                        var errors = JsonSerializer.Deserialize<string[]>(jsonElement.GetRawText())!;
                        return InvokedResult.Failed(errors);
                    }
                    return InvokedResult.Failed(jsonResponse.Message ?? _unknownErrorMessage);
                }
            }
            return InvokedResult.Failed(_unknownErrorMessage);
        }
        catch (Exception ex)
        {
            return InvokedResult.Failed(ex.Message);
        }
    }
}