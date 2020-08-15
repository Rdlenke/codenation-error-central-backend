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

        public LogError Add(LogError logError)
        {
            return _context.LogErrors.Add(logError).Entity;
        }

        public async Task<LogError> GetById(int id)
        {
            return await _context.LogErrors.FirstOrDefaultAsync(x => x.Id == id && x.Filed == false);
        }

        public IList<LogError> GetList()
        {
            return _context.LogErrors.Where(x => x.Filed == false).ToList() ;
        }

        public IList<LogError> GetByEnvironment(EEnvironment environment)
        {
            return _context.LogErrors.Where(x => x.Environment == environment && x.Filed == false).ToList();
        }

        public LogError Update(LogError logError)
        {
            return _context.LogErrors.Update(logError).Entity;
        }

        public async Task<LogError> GetByIdAsync(int id)
        {
            return await _context.LogErrors.FirstOrDefaultAsync(l => l.Id == id && l.Filed == false);
        }

        public async Task<List<LogError>> GetArchived()
        {
            return await _context.LogErrors.Where(x => x.Filed == true).ToListAsync();
        }
    }
}

