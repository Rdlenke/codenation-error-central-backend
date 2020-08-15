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
    public class LogErrorRepositoryTest
    {
        DbContextOptions<ErrorCentralContext> Options;
        private readonly DbConnection _connection;
        private LogError _one;
        private LogError _two;

        public LogErrorRepositoryTest(DbContextOptions<ErrorCentralContext> options = null)
        {
            if(options == null)
            {
                Options = new DbContextOptionsBuilder<ErrorCentralContext>().UseSqlite(CreateInMemoryDatabase()).Options;
            }

            _connection = RelationalOptionsExtension.Extract(Options).Connection;
            _one = new LogErrorBuilder().Build();
            _two = new LogErrorBuilder().Build();
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }


        private ErrorCentralContext Seed()
        {
            var context = new ErrorCentralContext(Options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var userOne = new UserBuilder().Build();

            context.Add(userOne);

            context.SaveChanges();

            context.AddRange(_one, _two);

            context.SaveChanges();

            return context;
        }

        [Fact]
        public void Add_LogError()
        {
            using (var context = new ErrorCentralContext(Options))
            {
                var logError = new LogErrorBuilder().Build();

                var repository = new LogErrorRepository(context);
                LogError result = repository.Add(logError);

                result.Should().BeEquivalentTo(logError);
            }
        }

        [Fact]
        public async void Update_LogError()
        {
            using (var context = Seed())
            {
                var repository = new LogErrorRepository(context);
                
                LogError result = await repository.GetById(1);

                result.Archive();

                repository.Update(result);

                int updated = await repository.UnitOfWork.SaveChangesAsync();

                updated.Should().BeGreaterThan(0);
            }
        }
        [Fact]
        public void GetByEnvironment_LogError()
        {
            using (var context = Seed())
            {
                var repository = new LogErrorRepository(context);

                IList<LogError> result = repository.GetByEnvironment(EEnvironment.Production);

                foreach(LogError logError in result)
                {
                    logError.Environment.Should().Be(EEnvironment.Production);
                }
            }
        }

        [Fact]
        public void GetList_LogError()
        {
            using (var context = Seed())
            {
                LogErrorBuilder builder = new LogErrorBuilder();
                List<LogError> expected = new List<LogError> { _one, _two };

                var repository = new LogErrorRepository(context);
                IList<LogError> result = repository.GetList();

                result.Should().BeEquivalentTo(expected);
            }
        }

        public void Dispose() => _connection.Dispose();
    }
}
