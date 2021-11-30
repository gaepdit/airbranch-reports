using Domain.Facilities.Repositories;
using Domain.Facilities.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using WebApp.Api.Compliance;
using WebApp.Api.Facilities;

namespace WebAppTests.Api;

public class FacilityExists
{
    [Test]
    public async Task ReturnsTrueIfExists()
    {
        var repo = new Mock<IFacilitiesRepository>();
        repo.Setup(l => l.FacilityExistsAsync(It.IsAny<ApbFacilityId>()))
            .ReturnsAsync(true);

        var controller = new FacilitiesController();
        var response = await controller.ExistsAsync(repo.Object, "00100001");

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = response.Result as OkObjectResult;
            result!.StatusCode.Should().Be(200);
            result!.Value.Should().BeEquivalentTo(true);
        });
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        var repo = new Mock<IFacilitiesRepository>();
        repo.Setup(l => l.FacilityExistsAsync(It.IsAny<ApbFacilityId>()))
            .ReturnsAsync(false);

        var controller = new FacilitiesController();
        var response = await controller.ExistsAsync(repo.Object, "00100001");

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = response.Result as OkObjectResult;
            result!.StatusCode.Should().Be(200);
            result!.Value.Should().BeEquivalentTo(false);
        });
    }

    [Test]
    public async Task ReturnsBadRequestIfInvalidFacilityId()
    {
        var repo = new Mock<IFacilitiesRepository>();
        var controller = new FacilitiesController();
        var response = await controller.ExistsAsync(repo.Object, "abc");

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<BadRequestResult>();
            var result = response.Result as BadRequestResult;
            result!.StatusCode.Should().Be(400);
        });
    }
}