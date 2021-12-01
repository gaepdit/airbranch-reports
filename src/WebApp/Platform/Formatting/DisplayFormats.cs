namespace WebApp.Platform.Formatting;

public static class DisplayFormats
{
    public const string LongDate = "MMMM\u00a0d, yyyy";
    public const string ShortDate = "d\u2011MMM\u2011yyyy";

    public static string ToString(this DateTime? dt, string format, string nullReplacement = "N/A") =>
        dt == null ? nullReplacement : dt.Value.ToString(format);
}
