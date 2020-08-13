using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;

namespace ErrorCentral.UnitTests.Builders.AggregatesModel
{
    public class LogErrorBuilder : IBuilder<LogError>
    {
        private LogError _logError;
        public string Title => "Run-time exception (line 8): Attempted to divide by zero.";
        public string Details => "[System.DivideByZeroException: Attempted to divide by zero.] \nat Program.Main() :line 8";
        public string Source => "http://production.com/";
        public ELevel Level => ELevel.Error;
        public EEnvironment Environment => EEnvironment.Production;
        public int UserId => 1;
        public LogErrorBuilder()
        {
            _logError = WithDefaultValues();
        }

        public LogError WithDefaultValues()
        {
            _logError = new LogError(
                userId: UserId,
                title: Title,
                details: Details,
                source: Source,
                level: Level,
                environment: Environment);
            return _logError;
        }

        public LogError Build()
        {
            return _logError;
        }
    }
}
