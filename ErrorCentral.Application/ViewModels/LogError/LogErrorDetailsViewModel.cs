using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.SeedWork;
using System;

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

        public bool Filed { get; set; }
    }
}
