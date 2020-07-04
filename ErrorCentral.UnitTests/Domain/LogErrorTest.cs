﻿using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.Exceptions;
using FluentAssertions;
using FluentAssertions.Extensions;
using System;
using Xunit;

namespace ErrorCentral.UnitTests.Domain
{
    public class LogErrorTest
    {
        [Fact]
        public void Create_log_error_success()
        {
            //Arrange
            var userId = 1;
            var title = "fakeTitle";
            var details = "fakeDatails";
            var source = "localhost";
            var level = ELevel.Debug;
            var environment = EEnvironment.Development;

            //Act 
            var logError = new LogError(
                userId: userId,
                title: title,
                details: details,
                source: source,
                level: level,
                environment: environment);

            //Assert
            logError
                .Should()
                .NotBeNull();

            logError.UserId
                .Should()
                .Be(userId);

            logError.Removed
                .Should()
                .BeFalse();

            logError.Filed
                .Should()
                .BeFalse();

            logError.Level
                .Should()
                .NotBeNull();

            logError.Environment
                .Should()
                .NotBeNull();

            logError.CreatedAt
                .Should()
                .BeAfter(1.Hours().Before(DateTime.UtcNow));

            logError.UpdatedAt
                .Should()
                .BeAfter(1.Hours().Before(DateTime.UtcNow));
        }

        [Fact]
        public void Create_log_error_fail()
        {
            //Arrange
            var userId = 1;
            var title = "";
            var details = "fakeDatails";
            var source = "localhost";
            var level = ELevel.Debug;
            var environment = EEnvironment.Development;

            //Act - Assert
            Action act = () => new LogError(
                userId: userId,
                title: title,
                details: details,
                source: source,
                level: level,
                environment: environment);

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'title')");
        }
    }
}
