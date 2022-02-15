using FluentAssertions;
using Infrastructure.StackTest;
using NUnit.Framework;
using System.Threading.Tasks;

namespace IntegrationTests.StackTest;

public class StackTestReportExists
{
    [Test]
    public async Task ReturnsTrueIfExists()
    {
        var facilityId = "12100021";
        var referenceNumber = 201100541;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.StackTestReportExistsAsync(facilityId, referenceNumber);
        result.Should().BeTrue();
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        var facilityId = "000-00000";
        var referenceNumber = 1;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.StackTestReportExistsAsync(facilityId, referenceNumber);
        result.Should().BeFalse();
    }

    [Test]
    public async Task ReturnsFalseIfUnassignedDocumentType()
    {
        var facilityId = "095-00085";
        var referenceNumber = 202101068;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.StackTestReportExistsAsync(facilityId, referenceNumber);
        result.Should().BeFalse();
    }
}
