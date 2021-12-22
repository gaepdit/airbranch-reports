using Domain.Facilities.Models;
using FluentAssertions;
using LocalRepository.Compliance;
using LocalRepository.Data;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.Compliance;

public class AccReportExists
{
    [Test]
    public async Task ReturnsTrueIfExists()
    {
        var report = AccData.GetAccReports.First();

        var repo = new ComplianceRepository();
        var result = await repo.AccReportExistsAsync(report.Facility!.Id!, report.AccReportingYear);
        result.Should().BeTrue();
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        var repo = new ComplianceRepository();
        var result = await repo.AccReportExistsAsync(new ApbFacilityId(Constants.FacilityIdThatDoesNotExist), default);
        result.Should().BeFalse();
    }
}