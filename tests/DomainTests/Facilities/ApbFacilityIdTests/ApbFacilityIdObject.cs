using Domain.Facilities.Models;
using FluentAssertions;
using NUnit.Framework;

namespace DomainTests.Facilities.ApbFacilityIdTests;

public class ApbFacilityIdObject
{
    [Test]
    public void HasCorrectlyFormattedProperties()
    {
        var airs = new ApbFacilityId("12345678");

        Assert.Multiple(() =>
        {
            airs.ToString().Should().Be("12345678");
            airs.ShortString.Should().Be("12345678");
            airs.FormattedString.Should().Be("123-45678");
            airs.DbFormattedString.Should().Be("041312345678");
            airs.EpaFacilityIdentifier.Should().Be("GA0000001312345678");
        });
    }
}