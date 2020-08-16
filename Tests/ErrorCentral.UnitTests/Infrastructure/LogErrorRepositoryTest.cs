using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using ErrorCentral.Infrastructure;
using ErrorCentral.Infrastructure.Repositories;
using ErrorCentral.UnitTests.Builders.AggregatesModel;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Xunit;

namespace ErrorCentral.UnitTests.Infrastructure
{
    public class LogErrorRepositoryTest : RepositoryTest
    {
        private List<LogError> _logErrors;

        public LogErrorRepositoryTest() : base()
        {
            _logErrors = new List<LogError> { new LogErrorBuilder().Build(), new LogErrorBuilder().Build() };
        }

        private ErrorCentralContext Seed()
        {
            var context = new ErrorCentralContext(Options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();


            context.Add(new UserBuilder().Build());

            context.SaveChanges();

            context.AddRange(_logErrors[0], _logErrors[1]);

            context.SaveChanges();

            return context;
        }

        [Fact]
        public async void Add_LogError()
        {
            using (var context = new ErrorCentralContext(Options))
            {
                var logError = new LogErrorBuilder().Build();

                var repository = new LogErrorRepository(context);
                LogError result = await repository.AddAsync(logError);

                result.Should().BeEquivalentTo(logError);
            }
        }

        [Fact]
        public async void Update_LogError()
        {
            using (var context = Seed())
            {
                var repository = new LogErrorRepository(context);
                
                LogError result = await repository.GetByIdAsync(1);

                result.Archive();

                repository.Update(result);

                int updated = await repository.UnitOfWork.SaveChangesAsync();

                updated.Should().BeGreaterThan(0);
            }
        }
        [Fact]
        public async void GetByEnvironment_LogError()
        {
            using (var context = Seed())
            {
                var repository = new LogErrorRepository(context);

                IList<LogError> result = await repository.GetByEnvironmentAsync(EEnvironment.Production);

                foreach(LogError logError in result)
                {
                    logError.Environment.Should().Be(EEnvironment.Production);
                }
            }
        }

        [Fact]
        public async void GetList_LogError()
        {
            using (var context = Seed())
            {
                LogErrorBuilder builder = new LogErrorBuilder();

                var repository = new LogErrorRepository(context);
                IList<LogError> result = await repository.GetAllUnarchivedAsync();

                result.Should().BeEquivalentTo(_logErrors);
            }
        }

    }
}
