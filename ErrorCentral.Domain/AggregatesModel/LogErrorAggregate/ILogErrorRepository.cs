using ErrorCentral.Domain.SeedWork;

namespace ErrorCentral.Domain.AggregatesModel.LogErrorAggregate
{
    public interface ILogErrorRepository : IRepository<LogError>
    {
        LogError Add(LogError logError);
    }
}
