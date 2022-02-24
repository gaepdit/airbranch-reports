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
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.StackTestReportExistsAsync("12100021", 201100541);
        result.Should().BeTrue();
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.StackTestReportExistsAsync("000-00000", 1);
        result.Should().BeFalse();
    }

    [Test]
    public async Task ReturnsFalseIfUnassignedDocumentType()
    {
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.StackTestReportExistsAsync("095-00085", 202101068);
        result.Should().BeFalse();
    }

    [Test]
    public async Task ReturnsFalseIfTestDeleted()
    {
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.StackTestReportExistsAsync("05900059", 202000278);
        result.Should().BeFalse();
    }
}
