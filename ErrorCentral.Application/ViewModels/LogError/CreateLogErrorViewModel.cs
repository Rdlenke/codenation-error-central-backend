using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.SeedWork;

namespace ErrorCentral.Application.ViewModels.LogError
{
    public class CreateLogErrorViewModel : IViewModel
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string Source { get; set; }
        public ELevel Level { get; set; }
        public EEnvironment Environment { get; set; }
    }
}
