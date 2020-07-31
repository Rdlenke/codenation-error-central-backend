using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.SeedWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ErrorCentral.Application.ViewModels.LogError
{
    public class EnvironmentLogErrorsViewModel : IViewModel
    {
        public int UserId { get; set; }

        public string Details { get; set; }

        public ELevel Level { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string Source { get; set; }

        public int Events { get; set; }


        public EnvironmentLogErrorsViewModel(int userId, string details, ELevel level, DateTimeOffset createdAt, string source, int events)
        {
            UserId = userId;
            Details = details;
            Level = level;
            CreatedAt = createdAt;
            Source = source;
            Events = events;
        }
    }
}