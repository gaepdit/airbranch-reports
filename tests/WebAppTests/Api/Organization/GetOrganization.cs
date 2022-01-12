using Domain.Organization.Models;
using Domain.Organization.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using WebApp.Api.Organization;

namespace WebAppTests.Api;

public class GetOrganization
{
    [Test]
    public async Task ReturnsOkIfExists()
    {
        var repo = new Mock<IOrganizationRepository>();
        repo.Setup(l => l.GetAsync())
            .ReturnsAsync(new OrganizationInfo());

        var controller = new OrganizationController();
        var response = await controller.GetAsync(repo.Object);

        Assert.Multiple(() =>
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = response.Result as OkObjectResult;
            result!.StatusCode.Should().Be(200);
            result!.Value.Should().BeEquivalentTo(new OrganizationInfo());
        });
    }
}