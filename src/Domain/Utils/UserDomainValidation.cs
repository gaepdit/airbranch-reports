namespace Domain.Utils;

public static class UserDomainValidation
{
    public const string DnrGaGov = "@dnr.ga.gov";

    public static bool IsValidEmailDomain(this string email) =>
        email.EndsWith(DnrGaGov, StringComparison.CurrentCultureIgnoreCase);
}
