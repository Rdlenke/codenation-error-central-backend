using ErrorCentral.Domain.AggregatesModel.LogError;
using ErrorCentral.Domain.SeedWork;
using System;

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
    }
}
