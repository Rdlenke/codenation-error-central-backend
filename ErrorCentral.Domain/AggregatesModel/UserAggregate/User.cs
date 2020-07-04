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

        public User() { }

        private readonly List<LogError> _logErrors;
        public IReadOnlyCollection<LogError> LogErrors => _logErrors;
    }
}
