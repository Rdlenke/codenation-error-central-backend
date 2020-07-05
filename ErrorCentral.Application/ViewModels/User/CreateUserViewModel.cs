using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorCentral.Application.ViewModels.User
{
    public class CreateUserViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
}
