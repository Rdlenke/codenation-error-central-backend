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
    }
}
