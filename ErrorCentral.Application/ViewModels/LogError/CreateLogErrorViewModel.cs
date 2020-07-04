using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.SeedWork;
using System.ComponentModel.DataAnnotations;

namespace ErrorCentral.Application.ViewModels.LogError
{
    public class CreateLogErrorViewModel : IViewModel
    {
        [Required]
        public int UserId { get; private set; }
        [Required]
        public string Title { get; private set; }
        public string Details { get; private set; }
        [Required]
        public string Source { get; private set; }
        [Required]
        public ELevel Level { get; private set; }
        [Required]
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
