using Domain.Facilities.Models;
using Domain.StackTest.Models;
using FluentAssertions;
using Infrastructure.StackTest;
using NUnit.Framework;
using System.Threading.Tasks;

namespace IntegrationTests.StackTest;

public class GetStackTestReport
{
    [Test]
    public async Task ReturnsReportIfExists()
    {
        var facilityId = "121-00021";
        var referenceNumber = 201100541;
        var repo = new StackTestRepository(Global.conn!);
        var result = await repo.GetStackTestReportAsync(facilityId, referenceNumber);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<StackTestReportOneStack>();
            result.Should().NotBeNull();
            result!.ReferenceNumber.Should().Be(referenceNumber);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id!.ToString().Should().Be(facilityId);
        });
    }

    [Test]
    public async Task ParsingConfidentialInfoDoesNotFail()
    {
        var facilityId = new ApbFacilityId("17900001");
        var referenceNumber = 202001297;
        var repo = new StackTestRepository(Global.conn!);
        StackTestReportOneStack? stackTestReport = await repo.GetStackTestReportAsync(facilityId, referenceNumber) as StackTestReportOneStack;

        var act = () => stackTestReport!.RedactedStackTestReport();
        act.Should().NotThrow();
    }

    [Test]
    public async Task ReturnsNullIfNotExists()
    {
        var repo = new StackTestRepository(Global.conn!);
        var result = await repo.GetStackTestReportAsync("000-00000", 2019);

        result.Should().BeNull();
    }
}
