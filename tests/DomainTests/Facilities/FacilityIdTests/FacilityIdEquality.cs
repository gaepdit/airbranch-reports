using Domain.Facilities.Models;

namespace DomainTests.Facilities.FacilityIdTests;

public class FacilityIdEquality
{
    [Test]
    public void IsTrueForEquivalentAirsNumbers()
    {
        var airs1 = new FacilityId("12345678");
        var airs2 = new FacilityId("123-45678");
        Assert.Multiple(() =>
        {
            (airs1 == airs2).Should().BeTrue();
            (airs1 != airs2).Should().BeFalse();
        });
    }

    [Test]
    public void IsFalseForDifferentAirsNumbers()
    {
        var airs1 = new FacilityId("12345678");
        var airs2 = new FacilityId("87654321");
        Assert.Multiple(() =>
        {
            (airs1 == airs2).Should().BeFalse();
            (airs1 != airs2).Should().BeTrue();
        });
    }
}
