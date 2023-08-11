using BlazorWebAssemblyWithIdentity.Server.Filters;
using BlazorWebAssemblyWithIdentity.Server.Models;
using BlazorWebAssemblyWithIdentity.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BlazorWebAssemblyWithIdentity.Server.Controllers
{
    [ApiController]
    [Authorize(Roles = Constants.RoleNames.Admin)]
    public class UserController : ApiControllerBase
    {
        private UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userNamager)
        {
            _userManager = userNamager;
        }

        [Route("api/users")]
        [ValidatePageParameters]
        public async Task<ActionResult<UserInfo[]>> GetUsers(int page = 1, int pageSize = 10)
        {
            return await _userManager.Users.Skip((page - 1) * pageSize).Take(pageSize).Select(u => new UserInfo
            {
                Id = u.Id.ToString(),
                Name = u.UserName
            }).ToArrayAsync();
        }
    }
}
