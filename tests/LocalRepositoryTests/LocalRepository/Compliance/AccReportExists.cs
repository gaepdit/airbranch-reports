using FluentAssertions;
using LocalRepository.Compliance;
using LocalRepository.Data.Compliance;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace InfrastructureTests.LocalRepository.Compliance;

public class AccReportExists
{
    [Test]
    public async Task ReturnsTrueIfExists()
    {
        var report = AccData.GetAccReports.First();

        var repo = new ComplianceRepository();
        var result = await repo.AccReportExistsAsync(report.Facility.Id, report.AccReportingYear);
        result.Should().BeTrue();
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        var repo = new ComplianceRepository();
        var result = await repo.AccReportExistsAsync(default, default);
        result.Should().BeFalse();
    }
}