using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ErrorCentral.UnitTests.Domain
{
    public class EEnvironmentExtensionsTest
    {
        [Theory]
        [InlineData(EEnvironment.Development, "Development")]
        [InlineData(EEnvironment.Homologation, "Homologation")]
        [InlineData(EEnvironment.Production, "Production")]

        public void TestToFriendlyString(EEnvironment environment, string text)
        {
            environment.ToFriendlyString().Should().Be(text);
        }
    }
}
