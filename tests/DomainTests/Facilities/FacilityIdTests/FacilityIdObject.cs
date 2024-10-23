using Domain.Facilities.Models;
using System;

namespace DomainTests.Facilities.FacilityIdTests;

public class FacilityIdObject
{
    [Test]
    public void HasCorrectlyFormattedProperties()
    {
        var airs = new FacilityId("12345678");

        Assert.Multiple(() =>
        {
            airs.ToString().Should().Be("123-45678");
            airs.ShortString.Should().Be("12345678");
            airs.FormattedId.Should().Be("123-45678");
            airs.DbFormattedString.Should().Be("041312345678");
            airs.EpaFacilityIdentifier.Should().Be("GA0000001312345678");
        });
    }

    [Test]
    public void ThrowsOnLongForm()
    {
        const string id = "041312345678";
        var act = () => new FacilityId(id);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void HasCorrectlyFormattedPropertiesFromImplicitConversion()
    {
        FacilityId airs = "12345678";

        Assert.Multiple(() =>
        {
            airs.ToString().Should().Be("123-45678");
            airs.ShortString.Should().Be("12345678");
            airs.FormattedId.Should().Be("123-45678");
            airs.DbFormattedString.Should().Be("041312345678");
            airs.EpaFacilityIdentifier.Should().Be("GA0000001312345678");
        });
    }

    [Test]
    public void ThrowsOnInvalidAirsNumber()
    {
        const string id = "abc";
        var act = () => new FacilityId(id);
        act.Should().Throw<ArgumentException>();
    }
}
