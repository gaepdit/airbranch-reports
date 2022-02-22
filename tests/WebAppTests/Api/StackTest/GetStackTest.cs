using Domain.Facilities.Models;
using Domain.StackTest.Models;
using Domain.StackTest.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Api.StackTest;

namespace WebAppTests.Api.StackTest;

public class GetStackTest
{
    [Test]
    public async Task ReturnsOkIfExists()
    {
        var repo = new Mock<IStackTestRepository>();
        repo.Setup(l => l.GetStackTestReportAsync(It.IsAny<ApbFacilityId>(), It.IsAny<int>()))
            .ReturnsAsync(new StackTestReportOneStack());

        var controller = new StackTestController();
        var response = await controller.GetAsync(repo.Object, "00100001", default);

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = response.Result as OkObjectResult;
            result!.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new StackTestReportOneStack());
        });
    }

    [Test]
    public async Task ReturnsNotFoundIfNotExists()
    {
        var repo = new Mock<IStackTestRepository>();
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
        var repo = new Mock<IStackTestRepository>();
        var controller = new StackTestController();
        var response = await controller.GetAsync(repo.Object, "abc", default);

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<BadRequestResult>();
            var result = response.Result as BadRequestResult;
            result!.StatusCode.Should().Be(400);
        });
    }

    [Test]
    public async Task ConfidentialRequestReturnsOkIfUserLoggedIn()
    {
        var repo = new Mock<IStackTestRepository>();
        repo.Setup(l => l.GetStackTestReportAsync(It.IsAny<ApbFacilityId>(), It.IsAny<int>()))
            .ReturnsAsync(new StackTestReportOneStack());

        var mockContext = new Mock<HttpContext>(MockBehavior.Strict);
        mockContext.SetupGet(c => c.User).Returns(new ClaimsPrincipal());
        mockContext.SetupGet(c => c.User.Identity!.IsAuthenticated).Returns(true);

        var controller = new StackTestController
        {
            ControllerContext = new ControllerContext() { HttpContext = mockContext.Object }
        };

        var response = await controller.GetAsync(repo.Object, "00100001", default, true);

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = response.Result as OkObjectResult;
            result!.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new StackTestReportOneStack());

        });
    }

    [Test]
    public async Task ConfidentialRequestReturnsChallengeIfUserIsNull()
    {
        var repo = new Mock<IStackTestRepository>();
        repo.Setup(l => l.GetStackTestReportAsync(It.IsAny<ApbFacilityId>(), It.IsAny<int>()))
            .ReturnsAsync(new StackTestReportOneStack());

        var mockContext = new Mock<HttpContext>(MockBehavior.Strict);
        mockContext.SetupGet(c => c.User.Identity).Returns(null as ClaimsIdentity);

        var controller = new StackTestController
        {
            ControllerContext = new ControllerContext() { HttpContext = mockContext.Object }
        };

        var response = await controller.GetAsync(repo.Object, "00100001", default, true);

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<ChallengeResult>();
            var result = response.Result as ChallengeResult;
            result!.AuthenticationSchemes.Should().BeEmpty();
        });
    }

    [Test]
    public async Task ConfidentialRequestReturnsChallengeIfUserNotAuthenticated()
    {
        var repo = new Mock<IStackTestRepository>();
        repo.Setup(l => l.GetStackTestReportAsync(It.IsAny<ApbFacilityId>(), It.IsAny<int>()))
            .ReturnsAsync(new StackTestReportOneStack());

        var mockContext = new Mock<HttpContext>(MockBehavior.Strict);
        mockContext.SetupGet(c => c.User).Returns(new ClaimsPrincipal());
        mockContext.SetupGet(c => c.User.Identity!.IsAuthenticated).Returns(false);

        var controller = new StackTestController
        {
            ControllerContext = new ControllerContext() { HttpContext = mockContext.Object }
        };

        var response = await controller.GetAsync(repo.Object, "00100001", default, true);

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<ChallengeResult>();
            var result = response.Result as ChallengeResult;
            result!.AuthenticationSchemes.Should().BeEmpty();
        });
    }
}
