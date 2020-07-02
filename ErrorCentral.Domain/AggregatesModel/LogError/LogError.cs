using ErrorCentral.Domain.SeedWork;
using System;

namespace ErrorCentral.Domain.AggregatesModel.LogError
{
    public class LogError : Entity, IAggregateRoot
    {
        public int UserId { get; private set; }
        public string Title { get; private set; }
        public string Details { get; private set; }
        public string Source { get; private set; }
        public bool Filed { get; private set; }
        public ELevel Level { get; private set; }
        public EEnvironment Environment { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        protected LogError()
        {
            Filed = false;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public LogError(int userId, string title, string details, string source, ELevel level, EEnvironment environment) : this()
        {
            this.UserId = userId;
            this.Title = !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentNullException(nameof(title));
            this.Details = details;
            this.Source = source;
            this.Level = level;
            this.Environment = environment;
        }
    }
}
