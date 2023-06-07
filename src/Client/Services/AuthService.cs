using Blazored.LocalStorage;
using BlazorWebAssemblyWithIdentity.Shared;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorWebAssemblyWithIdentity.Client.Services;

public class AuthService : IAuthService
{
    private readonly IAuthApi _authApiClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthService(IAuthApi userManager, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        _authApiClient = userManager;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<InvokedResult> LoginAsync(LoginModel model)
    {
        var result = await _authApiClient.LoginAsync(model);
        if (result.Succeeded)
        {
            Utility.ConsoleDebug($"User [{model.UserName}] logged in success, setting auth token.");
            await _localStorage.SetItemAsStringAsync(Constants.Auth.TokenLocalStorageKey, result.Data);
            await _authStateProvider.GetAuthenticationStateAsync();
        }
        return result;
    }

    public async Task<InvokedResult> LogoutAsync()
    {
        Utility.ConsoleDebug("Removing auth token.");
        await _localStorage.RemoveItemAsync(Constants.Auth.TokenLocalStorageKey);
        await _authStateProvider.GetAuthenticationStateAsync();
        return InvokedResult.Success;
    }

    public async Task<InvokedResult> RegisterAsync(RegisterModel model)
    {
        var result = await _authApiClient.RegisterAsync(model);
        Utility.ConsoleDebug($"User [{model.UserName}] registered {result.Succeeded}");
        return result;
    }
}
