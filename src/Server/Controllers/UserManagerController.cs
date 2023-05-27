using BlazorWebAssemblyWithIdentity.Server.Models;
using BlazorWebAssemblyWithIdentity.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlazorWebAssemblyWithIdentity.Server.Controllers
{
    [Route("api/user/[action]")]
    [ApiController]
    public class UserManagerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserManagerController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        { 
            var user = new ApplicationUser { UserName = registerModel.UserName };
            var createResult = await _userManager.CreateAsync(user, registerModel.Password);
            if (!createResult.Succeeded)
                return BadRequest(JsonResponse.Fail("User creation failed.", createResult.Errors.Select(err => err.Description)));

            return Ok(JsonResponse.Ok);
        }
    }
}
