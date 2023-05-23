using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorWebAssemblyWithIdentity.Client.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var state = new AuthenticationState(new ClaimsPrincipal());
            var result = Task.FromResult(state);
            NotifyAuthenticationStateChanged(result);
            return result;
        }
    }
}