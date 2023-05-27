using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlazorWebAssemblyWithIdentity.Shared.Constants.Models;

namespace BlazorWebAssemblyWithIdentity.Shared
{
    public class RegisterModel
    {
        [Required]
        [StringLength(MaximumUserNameLength, MinimumLength = MinimumUserNameLength)]
        public string UserName { get; set; } = String.Empty;

        [Required]
        [StringLength(MaximumPasswordLength, MinimumLength = MinimumPasswordLength)]
        public string Password { get; set; } = String.Empty;

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string PasswordConfirm { get; set; } = String.Empty;
    }
}
