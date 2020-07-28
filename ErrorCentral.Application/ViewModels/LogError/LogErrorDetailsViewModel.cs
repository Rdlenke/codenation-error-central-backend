using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorCentral.Application.ViewModels.LogError
{
    public class LogErrorDetailsViewModel : IViewModel
    {
        public int UserId { get; set; }

        public string Title { get; set; }

        public string Details { get; set; }

        public ELevel Level { get; set; }

        public EEnvironment Environment { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string Source { get; set; }

        public LogErrorDetailsViewModel(int userId, string title, string details, ELevel level, EEnvironment environment, DateTimeOffset createdAt, string source)
        {
            UserId = userId;
            Title = title;
            Details = details;
            Level = level;
            Environment = environment;
            CreatedAt = createdAt;
            Source = source;
        }
    }
}
