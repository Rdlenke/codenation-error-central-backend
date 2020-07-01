using ErrorCentral.Domain.SeedWork;

namespace ErrorCentral.Domain.AggregatesModel.LogError
{
    public interface ILogErrorRepository : IRepository<LogError>
    {
        LogError Add(LogError logError);
    }
}
