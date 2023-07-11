using BlazorWebAssemblyWithIdentity.Server.Models;
using BlazorWebAssemblyWithIdentity.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorWebAssemblyWithIdentity.Server.Controllers
{
    [Route("api/auth/[action]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<ApplicationUser> userManager,
                            SignInManager<ApplicationUser> signInManager,
                            AppSettings appSettings,
                            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        { 
            var user = new ApplicationUser { UserName = registerModel.UserName };
            var createResult = await _userManager.CreateAsync(user, registerModel.Password);
            if (!createResult.Succeeded)
                return IdentityFail("User creation failed.", createResult);

            await AddTheFirstUserRoles(user);
            return Ok(InvokedResult.Success);
        }

        private async Task AddTheFirstUserRoles(ApplicationUser user)
        {
            var usersCount = await _userManager.Users.CountAsync();
            if (usersCount == 1)
            {
                await AddUserToRole(user, Constants.RoleNames.Admin);
                await AddUserToRole(user, Constants.RoleNames.User);
            }
        }

        private async Task AddUserToRole(ApplicationUser user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to add [{0}] role to the user.{1}{2}", roleName, Environment.NewLine,
                    result.Errors.Select(err => err.Code + ", " + err.Description));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);
            if (user == null)
                return BadRequest(InvokedResult.Fail("The user name or password is incorrect"));

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, true);
            if (signInResult.IsLockedOut)
                return BadRequest(InvokedResult.Fail("Account locked."));
            if (signInResult.IsNotAllowed)
                return BadRequest(InvokedResult.Fail("Account is not allowed to login."));
            if (signInResult.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(InvokedResult.Ok(CreateToken(user, roles)));
            }

            return BadRequest(InvokedResult.Fail("The user name or password is incorrect"));
        }

        private object CreateToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.TokenKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: credentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}