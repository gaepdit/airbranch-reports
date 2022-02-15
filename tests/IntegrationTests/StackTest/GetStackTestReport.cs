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
    public async Task ParsingConfidentialInfoDoesNotFail()
    {
        var facilityId = new ApbFacilityId("17900001");
        var referenceNumber = 202001297;
        var repo = new StackTestRepository(Global.DbConn!);
        var stackTestReport =
            await repo.GetStackTestReportAsync(facilityId, referenceNumber) as StackTestReportOneStack;

        var act = () => stackTestReport!.RedactedStackTestReport();
        act.Should().NotThrow();
    }

    [Test]
    public async Task ReturnsNullIfNotExists()
    {
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.GetStackTestReportAsync("000-00000", 2019);

        result.Should().BeNull();
    }

    [Test]
    public async Task ReturnsOneStackReportIfExists()
    {
        var facilityId = new ApbFacilityId("121-00021");
        var referenceNumber = 201100541;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.GetStackTestReportAsync(facilityId, referenceNumber);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<StackTestReportOneStack>();
            result.Should().NotBeNull();
            result!.ReferenceNumber.Should().Be(referenceNumber);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id.Should().Be(facilityId);
        });
    }

    [Test]
    public async Task ReturnsTwoStackStandardReportIfExists()
    {
        var facilityId = new ApbFacilityId("24500002");
        var referenceNumber = 201600525;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.GetStackTestReportAsync(facilityId, referenceNumber);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<StackTestReportTwoStack>();
            result.Should().NotBeNull();
            result!.ReferenceNumber.Should().Be(referenceNumber);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id.Should().Be(facilityId);
            result.DocumentType.Should().Be(DocumentType.TwoStackStandard);
        });
    }

    [Test]
    public async Task ReturnsTwoStackDreReportIfExists()
    {
        var facilityId = new ApbFacilityId("07300003");
        var referenceNumber = 200400473;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.GetStackTestReportAsync(facilityId, referenceNumber);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<StackTestReportTwoStack>();
            result.Should().NotBeNull();
            result!.ReferenceNumber.Should().Be(referenceNumber);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id.Should().Be(facilityId);
            result.DocumentType.Should().Be(DocumentType.TwoStackDre);
        });
    }

    [Test]
    public async Task ReturnsLoadingRackReportIfExists()
    {
        var facilityId = new ApbFacilityId("02100090");
        var referenceNumber = 200500014;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.GetStackTestReportAsync(facilityId, referenceNumber);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<StackTestReportLoadingRack>();
            result.Should().NotBeNull();
            result!.ReferenceNumber.Should().Be(referenceNumber);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id.Should().Be(facilityId);
            ((StackTestReportLoadingRack)result).DestructionReduction.Units.Should().Be("%");
        });
    }

    [Test]
    public async Task ReturnsPondTreatmentReportIfExists()
    {
        var facilityId = new ApbFacilityId("11500021");
        var referenceNumber = 200400023;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.GetStackTestReportAsync(facilityId, referenceNumber);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<StackTestReportPondTreatment>();
            result.Should().NotBeNull();
            result!.ReferenceNumber.Should().Be(referenceNumber);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id.Should().Be(facilityId);
        });
    }

    [Test]
    public async Task ReturnsGasConcentrationReportIfExists()
    {
        var facilityId = new ApbFacilityId("15300040");
        var referenceNumber = 200400009;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.GetStackTestReportAsync(facilityId, referenceNumber);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<StackTestReportGasConcentration>();
            result.Should().NotBeNull();
            result!.ReferenceNumber.Should().Be(referenceNumber);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id.Should().Be(facilityId);
        });
    }

    [Test]
    public async Task ReturnsFlareReportIfExists()
    {
        var facilityId = new ApbFacilityId("05700040");
        var referenceNumber = 200400407;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.GetStackTestReportAsync(facilityId, referenceNumber);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<StackTestReportFlare>();
            result.Should().NotBeNull();
            result!.ReferenceNumber.Should().Be(referenceNumber);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id.Should().Be(facilityId);
        });
    }

    [Test]
    public async Task ReturnsMemorandumStandardIfExists()
    {
        var facilityId = new ApbFacilityId("17900001");
        var referenceNumber = 200600289;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.GetStackTestReportAsync(facilityId, referenceNumber);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<StackTestMemorandum>();
            result.Should().NotBeNull();
            result!.ReferenceNumber.Should().Be(referenceNumber);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id.Should().Be(facilityId);
            result.DocumentType.Should().Be(DocumentType.MemorandumStandard);
        });
    }

    [Test]
    public async Task ReturnsMemorandumToFileIfExists()
    {
        var facilityId = new ApbFacilityId("17900001");
        var referenceNumber = 201500570;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.GetStackTestReportAsync(facilityId, referenceNumber);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<StackTestMemorandum>();
            result.Should().NotBeNull();
            result!.ReferenceNumber.Should().Be(referenceNumber);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id.Should().Be(facilityId);
            result.DocumentType.Should().Be(DocumentType.MemorandumToFile);
        });
    }

    [Test]
    public async Task ReturnsMemorandumPteIfExists()
    {
        var facilityId = new ApbFacilityId("07300003");
        var referenceNumber = 200400476;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.GetStackTestReportAsync(facilityId, referenceNumber);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<StackTestMemorandum>();
            result.Should().NotBeNull();
            result!.ReferenceNumber.Should().Be(referenceNumber);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id.Should().Be(facilityId);
            result.DocumentType.Should().Be(DocumentType.PTE);
        });
    }

    [Test]
    public async Task ReturnsMethod9MultiReportIfExists()
    {
        var facilityId = new ApbFacilityId("11500021");
        var referenceNumber = 201801068;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.GetStackTestReportAsync(facilityId, referenceNumber);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<StackTestReportOpacity>();
            result.Should().NotBeNull();
            result!.ReferenceNumber.Should().Be(referenceNumber);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id.Should().Be(facilityId);
            result.DocumentType.Should().Be(DocumentType.Method9Multi);
        });
    }

    [Test]
    public async Task ReturnsMethod9SingleReportIfExists()
    {
        var facilityId = new ApbFacilityId("24500002");
        var referenceNumber = 200700192;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.GetStackTestReportAsync(facilityId, referenceNumber);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<StackTestReportOpacity>();
            result.Should().NotBeNull();
            result!.ReferenceNumber.Should().Be(referenceNumber);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id.Should().Be(facilityId);
            result.DocumentType.Should().Be(DocumentType.Method9Single);
        });
    }

    [Test]
    public async Task ReturnsMethod22ReportIfExists()
    {
        var facilityId = new ApbFacilityId("31300062");
        var referenceNumber = 200600052;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.GetStackTestReportAsync(facilityId, referenceNumber);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<StackTestReportOpacity>();
            result.Should().NotBeNull();
            result!.ReferenceNumber.Should().Be(referenceNumber);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id.Should().Be(facilityId);
            result.DocumentType.Should().Be(DocumentType.Method22);
        });
    }
}
