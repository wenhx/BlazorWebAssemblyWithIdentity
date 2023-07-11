using BlazorWebAssemblyWithIdentity.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWebAssemblyWithIdentity.Server.Controllers
{
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult IdentityFail(string message, IdentityResult result)
        {
            return BadRequest(InvokedResult.Fail(message, result.Errors.Select(err => err.Description)));
        }
    }
}
