using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWebAssemblyWithIdentity.Shared
{
    public class Constants
    {
        public class Models
        {
            public const int MaximumUserNameLength = 20;
            public const int MinimumUserNameLength = 5;
            public const int MaximumPasswordLength = 20;
            public const int MinimumPasswordLength = 8;
            public static readonly int MaxPage = 100;
            public static readonly int MaxPageSize = 100;
        }

        public class Auth
        {
            public static readonly string TokenLocalStorageKey = ".AuthToken";
        }

        public class RoleNames
        {
            public const string Admin = "Admin";
            public const string User = "User";
        }

        public class Messages
        {
            public static readonly string ServerErrorMessage = "An error occurred on the server.";
        }
    }
}
