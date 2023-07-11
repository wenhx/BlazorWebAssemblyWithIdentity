using Blazored.LocalStorage;
using BlazorWebAssemblyWithIdentity.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorWebAssemblyWithIdentity.Client.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;

    public CustomAuthenticationStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authToken = await _localStorage.GetItemAsStringAsync(Constants.Auth.TokenLocalStorageKey);
        var identity = new ClaimsIdentity();
        _httpClient.DefaultRequestHeaders.Authorization = null;

        if (!String.IsNullOrEmpty(authToken)) 
        {
            try
            {
                identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Replace("\"", ""));
            }
            catch (Exception)
            {
                await _localStorage.RemoveItemAsync(Constants.Auth.TokenLocalStorageKey);
                identity = new ClaimsIdentity();
            }
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);
        NotifyAuthenticationStateChanged(Task.FromResult(state));
        return state;
    }

    private IEnumerable<Claim>? ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes)!;
        claims.AddRange(ParseRolesClaims(keyValuePairs));
        claims.AddRange(keyValuePairs.Select(p => new Claim(p.Key, p.Value.ToString() ?? String.Empty)));
        return claims;
    }

    private static IEnumerable<Claim> ParseRolesClaims(Dictionary<string, object> keyValuePairs)
    {
        if (keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles))
        {
            var rolesString = roles.ToString() ?? String.Empty;
            if (rolesString.Trim().StartsWith("["))
            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(rolesString) ?? Array.Empty<string>();
                foreach (var role in parsedRoles)
                {
                    yield return new Claim(ClaimTypes.Role, role);
                }
            }
            else
            {
                yield return new Claim(ClaimTypes.Role, rolesString);
            }
            keyValuePairs.Remove(ClaimTypes.Role);
        }
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: 
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }
        return Convert.FromBase64String(base64);
    }
}