using Domain.Organization.Models;
using FluentAssertions;
using Infrastructure.Organization;
using NUnit.Framework;
using System.Threading.Tasks;

namespace InfrastructureTests.Organization;

public class GetOrganization
{
    [Test]
    public async Task ReturnsOrganization()
    {
        var repo = new OrganizationRepository(Global.db!);
        var result = await repo.GetAsync();

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<OrganizationInfo>();
            result.Org.Should().Be("Air Protection Branch");
        });
    }
}
