using BlazorWebAssemblyWithIdentity.Shared;

namespace BlazorWebAssemblyWithIdentity.Client.Services;

public interface IUserManagerApi
{
    Task LoginAsync(LoginModel loginModel);
    Task<InvokedResult> RegisterAsync(RegisterModel registerModel);
    Task LogoutAsync();
    Task<UserInfo> GetUserInfoAsync();
}
