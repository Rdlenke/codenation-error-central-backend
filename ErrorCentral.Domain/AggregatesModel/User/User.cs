using ErrorCentral.Domain.SeedWork;
using System;
using System.Collections.Generic;
using AggregateLogError = ErrorCentral.Domain.AggregatesModel.LogError;

namespace ErrorCentral.Domain.AggregatesModel.User
{
    public class User : Entity, IAggregateRoot
    {
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        protected User()
        {
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        private readonly List<AggregateLogError.LogError> _logErrors;
        public IReadOnlyCollection<AggregateLogError.LogError> LogErrors => _logErrors;
    }
}
