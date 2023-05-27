using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlazorWebAssemblyWithIdentity.Shared.Constants.Models;

namespace BlazorWebAssemblyWithIdentity.Shared
{
    public class LoginModel
    {
        [Required]
        [StringLength(MaximumUserNameLength, MinimumLength = MinimumUserNameLength)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(MaximumPasswordLength, MinimumLength = MinimumPasswordLength)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
