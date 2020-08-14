using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace ErrorCentral.Domain.AggregatesModel.UserAggregate
{
    public class User : Entity, IAggregateRoot
    {
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        private readonly List<LogError> _logErrors;
        public IReadOnlyCollection<LogError> LogErrors => _logErrors;

        public Guid Guid { get; set; }

        protected User()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public User(string firstName, string lastName, string email, string password) : this()
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }

    }
}
