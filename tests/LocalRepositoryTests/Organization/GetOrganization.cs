using FluentAssertions;
using LocalRepository.Data;
using LocalRepository.Organization;
using NUnit.Framework;
using System.Threading.Tasks;

namespace LocalRepositoryTests.Organization;

public class GetOrganization
{
    [Test]
    public async Task ReturnsOrganization()
    {
        var repo = new OrganizationRepository();
        var result = await repo.GetAsync();
        result.Should().BeEquivalentTo(OrganizationData.Organization);
    }
}
