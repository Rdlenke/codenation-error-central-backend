using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErrorCentral.Contracts
{
    public static class ApiRoutes
    {
        public const string Base = "api";
        public static class Authentication
        {
            public const string Register = Base + "/identity/register";
            public const string Login = Base + "/identity/login";
        }
    }
}
