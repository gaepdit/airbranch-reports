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

public class GetAccReport
{
    [Test]
    public async Task ReturnsOkIfExists()
    {
        var repo = new Mock<IComplianceRepository>();
        repo.Setup(l => l.GetAccReportAsync(It.IsAny<ApbFacilityId>(), It.IsAny<int>()))
            .ReturnsAsync((AccReport)default);

        var controller = new ComplianceController();
        var response = await controller.GetAccReportAsync(repo.Object, "00100001", default);

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = response.Result as OkObjectResult;
            result!.StatusCode.Should().Be(200);
            result!.Value.Should().BeEquivalentTo((AccReport)default);
        });
    }

    [Test]
    public async Task ReturnsNotFoundIfNotExists()
    {
        var repo = new Mock<IComplianceRepository>();
        repo.Setup(l => l.GetAccReportAsync(It.IsAny<ApbFacilityId>(), It.IsAny<int>()))
            .ReturnsAsync((AccReport?)null);

        var controller = new ComplianceController();
        var response = await controller.GetAccReportAsync(repo.Object, "00100001", default);

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
        var response = await controller.GetAccReportAsync(repo.Object, "abc", default);

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<BadRequestResult>();
            var result = response.Result as BadRequestResult;
            result!.StatusCode.Should().Be(400);
        });
    }
}