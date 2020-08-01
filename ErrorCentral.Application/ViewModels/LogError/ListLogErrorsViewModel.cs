using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.SeedWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ErrorCentral.Application.ViewModels.LogError
{
    public class ListLogErrorsViewModel : IViewModel
    {
        public int UserId { get; set; }

        public ELevel Level { get; set; }

        public EEnvironment Environment { get; set; }

        public string Source { get; set; }

        public int Events { get; set; }

        public string Title { get; set; }

        public ListLogErrorsViewModel(int userId, string title, ELevel level, EEnvironment environment, string source, int events)
        {
            UserId = userId;
            Title = title;
            Level = level;
            Source = source;
            Events = events;
            Environment = environment;
        }
    }
}
