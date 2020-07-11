using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using FluentAssertions;
using FluentAssertions.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ErrorCentral.UnitTests.Domain
{
    public class UserTest
    {
        [Fact]
        public void Create_User_Sucess()
        {
            const string Email = "user@user.com";
            const string FirstName = "User";
            const string LastName = "User";
            const string Password = "Password";

            //Act
            User user = new User(email: Email, lastName: LastName, firstName: FirstName, password: Password);

            //Assert
            user
                .Should()
                .NotBeNull();

            user.Removed
                .Should()
                .BeFalse();

            user.Email.Should().BeEquivalentTo(Email);

            user.FirstName.Should().BeEquivalentTo(FirstName);

            user.LastName.Should().BeEquivalentTo(LastName);

            user.Password.Should().BeEquivalentTo(Password);

            user.CreatedAt
                .Should()
                .BeAfter(1.Hours().Before(DateTime.UtcNow));

            user.UpdatedAt
                .Should()
                .BeAfter(1.Hours().Before(DateTime.UtcNow));
        }
    }
}
