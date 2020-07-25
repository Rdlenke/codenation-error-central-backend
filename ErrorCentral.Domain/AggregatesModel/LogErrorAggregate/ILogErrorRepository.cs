using ErrorCentral.Domain.SeedWork;
using System.Threading.Tasks;

namespace ErrorCentral.Domain.AggregatesModel.LogErrorAggregate
{
    public interface ILogErrorRepository : IRepository<LogError>
    {
        LogError Add(LogError logError);
        Task<LogError> GetById(int id);
    }
}
