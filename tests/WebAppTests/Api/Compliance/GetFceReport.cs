using Domain.Compliance.Models;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using WebApp.Api.Compliance;

namespace WebAppTests.Api;

public class GetFceReport
{
    [Test]
    public async Task ReturnsOkIfExists()
    {
        var facilityId = "12100021";
        var fceId = 7136;
        var repo = new Mock<IComplianceRepository>();
        repo.Setup(l => l.GetFceReportAsync(It.IsAny<ApbFacilityId>(), It.IsAny<int>()))
            .ReturnsAsync(new FceReport());

        var controller = new ComplianceController();
        var response = await controller.GetFceReportAsync(repo.Object, facilityId, fceId);

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = response.Result as OkObjectResult;
            result!.StatusCode.Should().Be(200);
            result!.Value.Should().BeEquivalentTo(new FceReport());
        });
    }

    [Test]
    public async Task ReturnsNotFoundIfNotExists()
    {
        var facilityId = "12100021";
        var fceId = 7136;
        var repo = new Mock<IComplianceRepository>();
        repo.Setup(l => l.GetFceReportAsync(It.IsAny<ApbFacilityId>(), It.IsAny<int>()))
            .ReturnsAsync((FceReport?)null);

        var controller = new ComplianceController();
        var response = await controller.GetFceReportAsync(repo.Object, facilityId, fceId);

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<NotFoundResult>();
            var result = response.Result as NotFoundResult;
            result!.StatusCode.Should().Be(404);
        });
    }

    [Test]
    public async Task ReturnsBadRequestIfInvalidFacilityId()
    {
        var repo = new Mock<IComplianceRepository>();
        var controller = new ComplianceController();
        var response = await controller.GetFceReportAsync(repo.Object, "abc", default);

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<BadRequestResult>();
            var result = response.Result as BadRequestResult;
            result!.StatusCode.Should().Be(400);
        });
    }
}