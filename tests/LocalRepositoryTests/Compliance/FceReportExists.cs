using Domain.Facilities.Models;
using FluentAssertions;
using LocalRepository.Compliance;
using LocalRepository.Data;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.Compliance;

public class FceReportExists
{
    [Test]
    public async Task ReturnsTrueIfExists()
    {
        var report = FceData.GetFceReports.First();

        var repo = new ComplianceRepository();
        var result = await repo.FceReportExistsAsync(report.Facility!.Id!, report.Id);
        result.Should().BeTrue();
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        var repo = new ComplianceRepository();
        var result = await repo.FceReportExistsAsync(new ApbFacilityId(Constants.FacilityIdThatDoesNotExist), default);
        result.Should().BeFalse();
    }
}