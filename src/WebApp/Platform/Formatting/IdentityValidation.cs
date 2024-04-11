namespace WebApp.Platform.Formatting;

public static class IdentityValidation
{
    public static bool IsValidDnrEmail(this string email) => email.EndsWith("@dnr.ga.gov");
}
