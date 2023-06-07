using BlazorWebAssemblyWithIdentity.Shared;

namespace BlazorWebAssemblyWithIdentity.Client.Services;

public interface IAuthService
{
    Task<InvokedResult> RegisterAsync(RegisterModel model);
    Task<InvokedResult> LoginAsync(LoginModel model);
    Task<InvokedResult> LogoutAsync();
}
