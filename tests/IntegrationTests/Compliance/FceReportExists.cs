using FluentAssertions;
using Infrastructure.Compliance;
using NUnit.Framework;
using System.Threading.Tasks;

namespace IntegrationTests.Compliance;

public class FceReportExists
{
    [Test]
    public async Task ReturnsTrueIfExists()
    {
        var repo = new ComplianceRepository(Global.DbConn!);
        var result = await repo.FceReportExistsAsync("00100001", 7136);
        result.Should().BeTrue();
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        var repo = new ComplianceRepository(Global.DbConn!);
        var result = await repo.FceReportExistsAsync("000-00000", 1);
        result.Should().BeFalse();
    }
}
