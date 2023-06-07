using BlazorWebAssemblyWithIdentity.Server.Models;
using BlazorWebAssemblyWithIdentity.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorWebAssemblyWithIdentity.Server.Controllers
{
    [Route("api/auth/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppSettings _appSettings;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppSettings appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        { 
            var user = new ApplicationUser { UserName = registerModel.UserName };
            var createResult = await _userManager.CreateAsync(user, registerModel.Password);
            if (!createResult.Succeeded)
                return BadRequest(InvokedResult.Fail("User creation failed.", createResult.Errors.Select(err => err.Description)));

            return Ok(InvokedResult.Success);
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
                return Ok(InvokedResult.Ok(CreateToken(user)));

            return BadRequest(InvokedResult.Fail("The user name or password is incorrect"));
        }

        private object CreateToken(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.TokenKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: credentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
