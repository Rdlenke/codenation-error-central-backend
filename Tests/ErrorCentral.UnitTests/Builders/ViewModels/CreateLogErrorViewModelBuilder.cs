using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;

namespace ErrorCentral.UnitTests.Builders.ViewModels
{
    public class CreateLogErrorViewModelBuilder : IBuilder<CreateLogErrorViewModel>
    {
        private CreateLogErrorViewModel _createLogErrorViewModel;
        public string Title => "Run-time exception (line 8): Attempted to divide by zero.";
        public string Details => "[System.DivideByZeroException: Attempted to divide by zero.] \nat Program.Main() :line 8";
        public string Source => "http://production.com/";
        public ELevel Level => ELevel.Error;
        public EEnvironment Environment => EEnvironment.Production;
        public int UserId => 1;

        public CreateLogErrorViewModelBuilder()
        {
            _createLogErrorViewModel = WithDefaultValues();
        }

        public CreateLogErrorViewModel WithDefaultValues()
        {
            _createLogErrorViewModel = new CreateLogErrorViewModel()
            {
                Title = Title,
                Details = Details,
                Source = Source,
                Level = Level,
                Environment = Environment,
                UserId = UserId
            };
            return _createLogErrorViewModel;
        }

        public CreateLogErrorViewModelBuilder WithUserId(int userId)
        {
            _createLogErrorViewModel.UserId = userId;
            return this;
        }

        public CreateLogErrorViewModel Build()
        {
            return _createLogErrorViewModel;
        }
    }
}
