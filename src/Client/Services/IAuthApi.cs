using BlazorWebAssemblyWithIdentity.Shared;

namespace BlazorWebAssemblyWithIdentity.Client.Services;

public interface IAuthApi
{
    Task<InvokedResult<string>> LoginAsync(LoginModel loginModel);
    Task<InvokedResult> RegisterAsync(RegisterModel registerModel);
    Task LogoutAsync();
    Task<UserInfo> GetUserInfoAsync();
}
