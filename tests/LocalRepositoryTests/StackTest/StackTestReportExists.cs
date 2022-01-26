using Domain.Facilities.Models;
using FluentAssertions;
using LocalRepository.Data;
using LocalRepository.StackTest;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.StackTest;

public class StackTestReportExists
{
    [Test]
    public async Task ReturnsTrueIfExists()
    {
        var report = StackTestData.GetStackTestReports.First();

        var repo = new StackTestRepository();
        var result = await repo.StackTestReportExistsAsync(report.Facility!.Id!, report.ReferenceNumber);
        result.Should().BeTrue();
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        var repo = new StackTestRepository();
        var result = await repo.StackTestReportExistsAsync(new ApbFacilityId(Constants.FacilityIdThatDoesNotExist), default);
        result.Should().BeFalse();
    }
}