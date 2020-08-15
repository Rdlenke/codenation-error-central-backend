using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using ErrorCentral.Domain.SeedWork;
using System;

namespace ErrorCentral.Domain.AggregatesModel.LogErrorAggregate
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
        public User User { get; set; }

        protected LogError()
        {
            Filed = false;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public LogError(int userId, string title, string details, string source, ELevel level, EEnvironment environment) : this()
        {
            UserId = userId;
            this.Title = !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentNullException(nameof(title));
            Details = details;
            Source = source;
            Level = level;
            Environment = environment;
        }
        public virtual void Archive()
        {
            Filed = true;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public virtual void Unarchive()
        {
            Filed = false;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
