using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ErrorCentral.UnitTests.Domain
{
    public class ELevelExtensionsTest
    {
        [Theory]
        [InlineData(ELevel.Debug, "Debug")]
        [InlineData(ELevel.Error, "Error")]
        [InlineData(ELevel.Warning, "Warning")]

        public void TestToFriendlyString(ELevel environment, string text)
        {
            environment.ToFriendlyString().Should().Be(text);
        }
    }
}
