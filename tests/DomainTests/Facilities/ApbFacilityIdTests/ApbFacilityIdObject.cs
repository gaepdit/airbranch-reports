using Domain.Facilities.Models;
using System;

namespace DomainTests.Facilities.ApbFacilityIdTests;

public class ApbFacilityIdObject
{
    [Test]
    public void HasCorrectlyFormattedProperties()
    {
        var airs = new ApbFacilityId("12345678");

        Assert.Multiple(() =>
        {
            airs.ToString().Should().Be("123-45678");
            airs.ShortString.Should().Be("12345678");
            airs.FacilityId.Should().Be("123-45678");
            airs.DbFormattedString.Should().Be("041312345678");
            airs.EpaFacilityIdentifier.Should().Be("GA0000001312345678");
        });
    }

    [Test]
    public void HasCorrectlyFormattedPropertiesFromLongForm()
    {
        var airs = new ApbFacilityId("041312345678");

        Assert.Multiple(() =>
        {
            airs.ToString().Should().Be("123-45678");
            airs.ShortString.Should().Be("12345678");
            airs.FacilityId.Should().Be("123-45678");
            airs.DbFormattedString.Should().Be("041312345678");
            airs.EpaFacilityIdentifier.Should().Be("GA0000001312345678");
        });
    }

    [Test]
    public void HasCorrectlyFormattedPropertiesFromImplicitConversion()
    {
        ApbFacilityId airs = "041312345678";

        Assert.Multiple(() =>
        {
            airs.ToString().Should().Be("123-45678");
            airs.ShortString.Should().Be("12345678");
            airs.FacilityId.Should().Be("123-45678");
            airs.DbFormattedString.Should().Be("041312345678");
            airs.EpaFacilityIdentifier.Should().Be("GA0000001312345678");
        });
    }

    [Test]
    public void ThrowsOnInvalidAirsNumber()
    {
        const string id = "abc";
        var act = () => new ApbFacilityId(id);
        act.Should().Throw<ArgumentException>().WithMessage($"{id} is not a valid AIRS number.");
    }
}
