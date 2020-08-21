using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using System;

namespace ErrorCentral.UnitTests.Builders.ViewModels
{
    public class LogErrorDetailsViewModelBuilder : IBuilder<LogErrorDetailsViewModel>
    {
        private LogErrorDetailsViewModel _logErrorDetailsViewModel;
        public string Title => "Run-time exception (line 8): Attempted to divide by zero.";
        public string Details => "[System.DivideByZeroException: Attempted to divide by zero.] \nat Program.Main() :line 8";
        public string Source => "http://production.com/";
        public ELevel Level => ELevel.Error;
        public EEnvironment Environment => EEnvironment.Production;
        public int UserId => 1;
        public DateTimeOffset CreatedAt => DateTimeOffset.UtcNow;

        public int Id => 1;

        public LogErrorDetailsViewModelBuilder()
        {
            _logErrorDetailsViewModel = WithDefaultValues();
        }

        public LogErrorDetailsViewModel WithDefaultValues()
        {
            _logErrorDetailsViewModel = new LogErrorDetailsViewModel()
            {
                UserId = UserId,
                Title = Title,
                Details = Details,
                Level = Level,
                Environment = Environment,
                CreatedAt = CreatedAt,
                Source = Source,
                Id = Id
            };
            return _logErrorDetailsViewModel;
        }

        public LogErrorDetailsViewModel Build()
        {
            return _logErrorDetailsViewModel;
        }
    }
}
