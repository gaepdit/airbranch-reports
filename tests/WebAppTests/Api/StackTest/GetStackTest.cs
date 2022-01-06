using Domain.Facilities.Models;
using Domain.Monitoring.Models;
using Domain.Monitoring.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using WebApp.Api.StackTest;

namespace WebAppTests.Api;

public class GetStackTest
{
    [Test]
    public async Task ReturnsOkIfExists()
    {
        var repo = new Mock<IMonitoringRepository>();
        repo.Setup(l => l.GetStackTestReportAsync(It.IsAny<ApbFacilityId>(), It.IsAny<int>()))
            .ReturnsAsync(new StackTestReportOneStack());

        var controller = new StackTestController();
        var response = await controller.GetAsync(repo.Object, "00100001", default);

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = response.Result as OkObjectResult;
            result!.StatusCode.Should().Be(200);
            result!.Value.Should().BeEquivalentTo(new StackTestReportOneStack());
        });
    }

    [Test]
    public async Task ReturnsNotFoundIfNotExists()
    {
        var repo = new Mock<IMonitoringRepository>();
        repo.Setup(l => l.GetStackTestReportAsync(It.IsAny<ApbFacilityId>(), It.IsAny<int>()))
            .ReturnsAsync((StackTestReportOneStack?)null);

        var controller = new StackTestController();
        var response = await controller.GetAsync(repo.Object, "00100001", default);

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
        var repo = new Mock<IMonitoringRepository>();
        var controller = new StackTestController();
        var response = await controller.GetAsync(repo.Object, "abc", default);

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<BadRequestResult>();
            var result = response.Result as BadRequestResult;
            result!.StatusCode.Should().Be(400);
        });
    }
}