namespace WebApp.Platform.Formatting;

public static class DisplayFormats
{
    public const string LongDate = "MMMM\u00a0d, yyyy";
    public const string ShortDate = "d\u2011MMM\u2011yyyy";

    /// <summary>
    ///     DateTime extension method to display a nullable DateTime as a string or display a replacement string
    ///     if the value is null.
    /// </summary>
    /// <param name="dt">The nullable DateTime to display.</param>
    /// <param name="format">The format for displaying the DateTime if it is not null.</param>
    /// <param name="nullReplacement">The replacement string to display if the DateTime is null.</param>
    public static string ToString(this DateTime? dt, string format, string nullReplacement = "N/A") =>
        dt == null ? nullReplacement : dt.Value.ToString(format);
}
