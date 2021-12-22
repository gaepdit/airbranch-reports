using Domain.Facilities.Models;
using Domain.Facilities.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using WebApp.Api.Facilities;

namespace WebAppTests.Api;

public class GetFacility
{
    [Test]
    public async Task ReturnsOkIfExists()
    {
        var repo = new Mock<IFacilitiesRepository>();
        repo.Setup(l => l.GetFacilityAsync(It.IsAny<ApbFacilityId>()))
            .ReturnsAsync(new Facility());

        var controller = new FacilityController();
        var response = await controller.GetAsync(repo.Object, "00100001");

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = response.Result as OkObjectResult;
            result!.StatusCode.Should().Be(200);
            result!.Value.Should().BeEquivalentTo(new Facility());
        });
    }

    [Test]
    public async Task ReturnsNotFoundIfNotExists()
    {
        var repo = new Mock<IFacilitiesRepository>();
        repo.Setup(l => l.GetFacilityAsync(It.IsAny<ApbFacilityId>()))
            .ReturnsAsync((Facility?)null);

        var controller = new FacilityController();
        var response = await controller.GetAsync(repo.Object, "00100001");

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
        var repo = new Mock<IFacilitiesRepository>();
        var controller = new FacilityController();
        var response = await controller.GetAsync(repo.Object, "abc");

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<BadRequestResult>();
            var result = response.Result as BadRequestResult;
            result!.StatusCode.Should().Be(400);
        });
    }
}