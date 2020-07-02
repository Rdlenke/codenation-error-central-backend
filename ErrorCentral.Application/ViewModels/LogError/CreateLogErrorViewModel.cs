using ErrorCentral.Domain.AggregatesModel.LogError;
using ErrorCentral.Domain.SeedWork;

namespace ErrorCentral.Application.ViewModels.LogError
{
    public class CreateLogErrorViewModel : IViewModel
    {
        public int UserId { get; private set; }
        public string Title { get; private set; }
        public string Details { get; private set; }
        public string Source { get; private set; }
        public ELevel Level { get; private set; }
        public EEnvironment Environment { get; private set; }

        public CreateLogErrorViewModel(int userId, string title, string details, string source, ELevel level, EEnvironment environment)
        {
            this.UserId = userId;
            this.Title = title;
            this.Details = details;
            this.Source = source;
            this.Level = level;
            this.Environment = environment;
        }
    }
}
