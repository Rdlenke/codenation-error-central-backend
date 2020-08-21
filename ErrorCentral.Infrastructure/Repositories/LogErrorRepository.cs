﻿using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        public async Task<LogError> AddAsync(LogError logError)
        {
            var result = await _context.LogErrors.AddAsync(logError);

            return result.Entity;
        }

        public async Task<IList<LogError>> GetAllUnarchivedAsync()
        {
            return await _context.LogErrors.Where(x => x.Filed == false).ToListAsync() ;
        }

        public async Task<IList<LogError>> GetByEnvironmentAsync(EEnvironment environment)
        {
            return await _context.LogErrors.Where(x => x.Environment == environment && x.Filed == false).ToListAsync();
        }

        public LogError Update(LogError logError)
        {
            return _context.LogErrors.Update(logError).Entity;
        }

        public async Task<LogError> GetByIdAsync(int id)
        {
            return await _context.LogErrors.FirstOrDefaultAsync(l => l.Id == id && l.Filed == false);
        }

        public async Task<LogError> GetFiledByIdAsync(int id)
        {
            return await _context.LogErrors.FirstOrDefaultAsync(l => l.Id == id && l.Filed == true);
        }

        public async Task<List<LogError>> GetArchivedAsync()
        {
            return await _context.LogErrors.Where(x => x.Filed == true).ToListAsync();
        }
    }
}

