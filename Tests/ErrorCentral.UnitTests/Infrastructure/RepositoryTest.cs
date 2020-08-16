using ErrorCentral.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace ErrorCentral.UnitTests.Infrastructure
{
    public class RepositoryTest
    {
        protected DbContextOptions<ErrorCentralContext> Options;
        protected readonly DbConnection _connection;

        public RepositoryTest(DbContextOptions<ErrorCentralContext> options = null)
        {
            if (options == null)

            {
                Options = new DbContextOptionsBuilder<ErrorCentralContext>().UseSqlite(CreateInMemoryDatabase()).Options;
            }

            _connection = RelationalOptionsExtension.Extract(Options).Connection;
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }
        public void Dispose() => _connection.Dispose();
    }
}
