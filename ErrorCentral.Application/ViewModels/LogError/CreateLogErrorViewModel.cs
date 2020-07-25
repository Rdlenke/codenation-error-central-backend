using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.SeedWork;
using System.ComponentModel.DataAnnotations;

namespace ErrorCentral.Application.ViewModels.LogError
{
    public class CreateLogErrorViewModel : IViewModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Details { get; set; }
        [Required]
        public string Source { get; set; }
        [Required]
        public ELevel Level { get; set; }
        [Required]
        public EEnvironment Environment { get; set; }

        protected CreateLogErrorViewModel() { }

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
