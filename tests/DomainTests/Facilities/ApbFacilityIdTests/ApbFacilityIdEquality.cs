using Domain.Facilities.Models;

namespace DomainTests.Facilities.ApbFacilityIdTests;

public class ApbFacilityIdEquality
{
    [Test]
    public void IsTrueForEquivalentAirsNumbers()
    {
        var airs1 = new ApbFacilityId("12345678");
        var airs2 = new ApbFacilityId("123-45678");
        Assert.Multiple(() =>
        {
            (airs1 == airs2).Should().BeTrue();
            (airs1 != airs2).Should().BeFalse();
        });
    }

    [Test]
    public void IsFalseForDifferentAirsNumbers()
    {
        var airs1 = new ApbFacilityId("12345678");
        var airs2 = new ApbFacilityId("87654321");
        Assert.Multiple(() =>
        {
            (airs1 == airs2).Should().BeFalse();
            (airs1 != airs2).Should().BeTrue();
        });
    }
}
