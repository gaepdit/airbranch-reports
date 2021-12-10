using FluentAssertions;
using Infrastructure.Monitoring;
using NUnit.Framework;
using System.Threading.Tasks;

namespace IntegrationTests.Monitoring;

public class StackTestReportExists
{
    [Test]
    public async Task ReturnsTrueIfExists()
    {
        var facilityId = "12100021";
        var referenceNumber = 201100541;
        var repo = new MonitoringRepository(Global.db!);
        var result = await repo.StackTestReportExistsAsync(facilityId, referenceNumber);
        result.Should().BeTrue();
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        var repo = new MonitoringRepository(Global.db!);
        var result = await repo.StackTestReportExistsAsync("000-00000", 1);
        result.Should().BeFalse();
    }
}
