using BlazorWebAssemblyWithIdentity.Client.Services;
using BlazorWebAssemblyWithIdentity.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorWebAssemblyWithIdentity.Client.Pages
{
    public partial class Login : CustomComponentBase
    {
        LoginModel _loginModel = new LoginModel();
        string _error = default!;

        [Parameter, SupplyParameterFromQuery]
        public string? From
        {
            get;
            set;
        }

        public bool IsFromRegister
        {
            get
            {
                return String.Equals(From, "register", StringComparison.OrdinalIgnoreCase);
            }
        }

        async Task OnSubmit()
        {
            _error = String.Empty;
            try
            {
                var result = await AuthService.LoginAsync(_loginModel);
                if (result.Succeeded)
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    _error = result.Message!;
                }
            }
            catch (Exception ex)
            {
                _error = ex.Message;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var user = await GetCurrentUser();
            if (user != null && user.IsAuthenticated)
            {
                Utility.ConsoleDebug($"User [{user.Name}] authentication succeeded");
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
