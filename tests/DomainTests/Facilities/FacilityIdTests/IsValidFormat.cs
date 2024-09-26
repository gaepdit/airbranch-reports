using Domain.Facilities.Models;
using System;
using System.Collections.Generic;

namespace DomainTests.Facilities.FacilityIdTests;

public class IsValidFormat
{
    [Test]
    [TestCaseSource(nameof(InvalidAirsNumbers))]
    public void RejectsInvalidAirsNumbers(string airs)
    {
        FacilityId.IsValidFormat(airs).Should().BeFalse();
    }

    [Test]
    public void NullAirsNumberThrowsException()
    {
        var act = () => FacilityId.IsValidFormat(null!).Should().BeFalse();
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'input')");
    }

    [Test]
    [TestCaseSource(nameof(ValidAirsNumbers))]
    public void AcceptsValidAirsNumbers(string airs)
    {
        FacilityId.IsValidFormat(airs).Should().BeTrue();
    }

    private static IEnumerable<string> InvalidAirsNumbers()
    {
        yield return "";
        yield return "111";
        yield return "abc";
        yield return "0010001";
        yield return "001000001";
        yield return "04130010001";
        yield return "001-0001";
        yield return "01-00001";
        yield return "0001-00001";
        yield return "041300100001";
        yield return "04-13-001-00001";
    }

    private static IEnumerable<string> ValidAirsNumbers()
    {
        yield return "00100001";
        yield return "001-00001";
    }
}
