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

        public string Details { get; set; }

        public bool Filed { get; set; }

        public int Id { get; set; }

        public ListLogErrorsViewModel(int userId, string title, ELevel level, EEnvironment environment, string source, string details, int events, bool filed, int id)
        {
            UserId = userId;
            Title = title;
            Level = level;
            Source = source;
            Details = details;
            Events = events;
            Environment = environment;
            Filed = filed;
            Id = id;
        }
    }
}
