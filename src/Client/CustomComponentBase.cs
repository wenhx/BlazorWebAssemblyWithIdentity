using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Principal;

namespace BlazorWebAssemblyWithIdentity.Client;

public abstract class CustomComponentBase : ComponentBase
{
    [CascadingParameter]
    Task<AuthenticationState>? AuthenticationStateTask { get; set; }

    public async Task<IIdentity?> GetCurrentUser()
    {
        if (AuthenticationStateTask == null)
            return null;

        var authState = await AuthenticationStateTask;
        var user = authState.User;
        return user.Identity;
    }
}
