using ErrorCentral.Domain.SeedWork;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ErrorCentral.Domain.AggregatesModel.LogErrorAggregate
{
    public interface ILogErrorRepository : IRepository<LogError>
    {
        LogError Add(LogError logError);
        Task<LogError> GetById(int id);
        IList<LogError> GetList();
        IList<LogError> GetByEnvironment(EEnvironment Environment);
        LogError Update(LogError logError);
        Task<LogError> GetByIdAsync(int id);
    }
}
