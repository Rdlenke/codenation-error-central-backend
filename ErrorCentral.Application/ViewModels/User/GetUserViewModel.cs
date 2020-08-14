using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorCentral.Application.ViewModels.User
{
    public class GetUserViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
        public string Token { get; set; }
        public Guid Guid { get; set; }
    }
}
