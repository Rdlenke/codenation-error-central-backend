using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using ErrorCentral.Domain.SeedWork;
using ErrorCentral.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorCentral.Infrastructure
{
    public class ErrorCentralContext : DbContext, IUnitOfWork
    {
        public DbSet<User> Users { get; set; }
        public DbSet<LogError> LogErrors { get; set; }

        private IDbContextTransaction _currentTransaction;
        public ErrorCentralContext(DbContextOptions<ErrorCentralContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserMap());
            builder.ApplyConfiguration(new LogErrorMap());
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
