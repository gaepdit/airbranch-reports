using FluentAssertions;
using LocalRepository.Data;
using LocalRepository.Monitoring;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.Monitoring;

public class StackTestReportExists
{
    [Test]
    public async Task ReturnsTrueIfExists()
    {
        var report = MonitoringData.GetStackTestReports.First();

        var repo = new MonitoringRepository();
        var result = await repo.StackTestReportExistsAsync(report.Facility.Id, report.ReferenceNumber);
        result.Should().BeTrue();
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        var repo = new MonitoringRepository();
        var result = await repo.StackTestReportExistsAsync(default, default);
        result.Should().BeFalse();
    }
}