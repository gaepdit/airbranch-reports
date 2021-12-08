using FluentAssertions;
using Infrastructure.Compliance;
using NUnit.Framework;
using System.Threading.Tasks;

namespace InfrastructureTests.Compliance;

public class AccReportExists
{
    [Test]
    public async Task ReturnsTrueIfExists()
    {
        var repo = new ComplianceRepository(Global.db!);
        var result = await repo.AccReportExistsAsync("193-00008", 2019);
        result.Should().BeTrue();
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        var repo = new ComplianceRepository(Global.db!);
        var result = await repo.AccReportExistsAsync("000-00000", 2019);
        result.Should().BeFalse();
    }
}