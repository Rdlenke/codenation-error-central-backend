using ErrorCentral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErrorCentral.Contracts
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public string[] Errors { get; set; }
        public bool Success { get; set; }
    }
}
