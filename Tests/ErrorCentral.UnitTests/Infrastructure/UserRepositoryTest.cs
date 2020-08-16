using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using ErrorCentral.Infrastructure;
using ErrorCentral.Infrastructure.Repositories;
using ErrorCentral.UnitTests.Builders.AggregatesModel;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Xunit;

namespace ErrorCentral.UnitTests.Infrastructure
{
    public class UserRepositoryTest : RepositoryTest
    {
        private User _user;

        public UserRepositoryTest() : base()
        {
            _user = new UserBuilder().Build();
        }

        private ErrorCentralContext Seed()
        {
            var context = new ErrorCentralContext(Options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Add(_user);

            context.SaveChanges();

            return context;
        }

        [Fact]
        public async void Create_User()
        {
            User user = new UserBuilder().Build();

            user.Email = "new_user@email.com";

            using (var context = Seed())
            {
                var repository = new UserRepository(context);

                var created = repository.Create(user);

                await repository.UnitOfWork.SaveEntitiesAsync();

                created.Should().BeEquivalentTo(user);

                var inDatabase = await repository.GetByEmailAsync(user.Email);

                inDatabase.Should().BeEquivalentTo(user);
            }
        }

        [Fact]
        public async void GetByEmail_User()
        {
            using (var context = Seed())
            {
                var repository = new UserRepository(context);

                var found = await repository.GetByEmailAsync(_user.Email);

                found.Should().BeEquivalentTo(_user);
            }
        }
    }
}
