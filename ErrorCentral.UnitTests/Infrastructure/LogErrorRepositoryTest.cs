using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ErrorCentral.UnitTests.Infrastructure
{
    public class LogErrorRepositoryTest
    {
        private readonly Mock<ILogErrorRepository> _logErrorRepositoryMock;
    }
}
