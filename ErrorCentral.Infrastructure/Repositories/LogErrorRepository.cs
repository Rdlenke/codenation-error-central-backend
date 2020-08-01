using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ErrorCentral.Infrastructure.Repositories
{
    public class LogErrorRepository : ILogErrorRepository
    {
        private readonly ErrorCentralContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public LogErrorRepository(ErrorCentralContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public LogError Add(LogError logError)
        {
            return _context.LogErrors.Add(logError).Entity;
        }
        
        public LogError Update(LogError logError)
        {
            return _context.LogErrors.Update(logError).Entity;
        }

        public async Task<LogError> GetByIdAsync(int id)
        {
            return await _context.LogErrors.FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
