using ErrorCentral.Domain.SeedWork;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ErrorCentral.Domain.AggregatesModel.LogErrorAggregate
{
    public interface ILogErrorRepository : IRepository<LogError>
    {
        Task<LogError> AddAsync(LogError logError);
        Task<IList<LogError>> GetAllUnarchivedAsync();
        Task<IList<LogError>> GetByEnvironmentAsync(EEnvironment Environment);
        LogError Update(LogError logError);
        Task<LogError> GetByIdAsync(int id);
        Task<List<LogError>> GetArchivedAsync();
    }
}
