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
        var repo = new MonitoringRepository(Global.conn!);
        var result = await repo.StackTestReportExistsAsync(facilityId, referenceNumber);
        result.Should().BeTrue();
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        var facilityId = "000-00000";
        var referenceNumber = 1;
        var repo = new MonitoringRepository(Global.conn!);
        var result = await repo.StackTestReportExistsAsync(facilityId, referenceNumber);
        result.Should().BeFalse();
    }

    [Test]
    public async Task ReturnsFalseIfUnassignedDocumentType()
    {
        var facilityId = "14300017";
        var referenceNumber = 202001344;
        var repo = new MonitoringRepository(Global.conn!);
        var result = await repo.StackTestReportExistsAsync(facilityId, referenceNumber);
        result.Should().BeFalse();
    }
}
