using Domain.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace DomainTests.Utils;

public class EnumGetDescription
{
    private enum DescriptionsTest
    {
        NoDescription = 0,

        [System.ComponentModel.Description("Enum Description")]
        WithDescription = 1,
    }

    [Test]
    public void ReturnsDescriptionIfExists()
    {
        var desc = DescriptionsTest.NoDescription.GetDescription();
        desc.Should().Be("NoDescription");
    }

    [Test]
    public void ReturnsStringIfNoDescriptionExists()
    {
        var desc = DescriptionsTest.WithDescription.GetDescription();
        desc.Should().Be("Enum Description");
    }

    [Test]
    public void ReturnsValueAsStringIfNotInEnum()
    {
        const DescriptionsTest testEnum = (DescriptionsTest)2;
        var desc = testEnum.GetDescription();
        desc.Should().Be("2");
    }
}
