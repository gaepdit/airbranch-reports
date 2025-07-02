namespace WebApp.Platform;

internal static class SecurityHeaders
{
    private static readonly string ReportUri =
        $"https://report-to-api.raygun.com/reports?apikey={AppSettings.RaygunSettings.ApiKey}";

    internal static void AddSecurityHeaderPolicies(this HeaderPolicyCollection policies)
    {
        // Not used because it adds its own HSTS header, and I prefer the one built into ASP.NET.
        // Also, I wanted to customize the CSP.
        // policies.AddDefaultSecurityHeaders(); //
        policies
            .AddFrameOptionsDeny()
            .AddContentTypeOptionsNoSniff()
            .AddReferrerPolicyStrictOriginWhenCrossOrigin()
            .RemoveServerHeader()
            .AddContentSecurityPolicy(builder => builder.CspBuilder())
            .AddCrossOriginOpenerPolicy(builder => builder.SameOrigin())
            .AddCrossOriginEmbedderPolicy(builder => builder.Credentialless())
            .AddCrossOriginResourcePolicy(builder => builder.SameSite());

        if (string.IsNullOrEmpty(AppSettings.RaygunSettings.ApiKey)) return;
        policies.AddReportingEndpoints(builder => builder.AddEndpoint("csp-endpoint", ReportUri));
    }

#pragma warning disable S1075 // "URIs should not be hardcoded"
    private static void CspBuilder(this CspBuilder builder)
    {
        builder.AddDefaultSrc().None();
        builder.AddBaseUri().None();
        builder.AddObjectSrc().None();
        builder.AddScriptSrc()
            .From("https://cdn.raygun.io/raygun4js/raygun.min.js")
            .WithHash256("kOJzCjwwBHVC6EAEX5M+ovfu9sE7JG0G9LcYssttn6I=") // Raygun CDN loader
            .WithHash256("k8lqom5XjWiHpIL9TqKQ7DpRVbQNTtRtBFIKZ0iQaBk=") // Raygun pulse
            .WithHashTagHelper()
            .ReportSample();
        builder.AddStyleSrc()
            .Self()
            .From("https://cdn.jsdelivr.net/npm/sanitize.css@13.0.0/sanitize.css")
            .From("https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css")
            .ReportSample();
        builder.AddImgSrc().Self();
        builder.AddConnectSrc()
            .From("https://api.raygun.com")
            .From("https://api.raygun.io");
        builder.AddFontSrc().Self();
        builder.AddFormAction()
            .Self()
            .From("https://login.microsoftonline.com");
        builder.AddManifestSrc().Self();
        builder.AddFrameAncestors().None();

        if (string.IsNullOrEmpty(AppSettings.RaygunSettings.ApiKey)) return;
        builder.AddReportUri().To(ReportUri);
        builder.AddReportTo("csp-endpoint");
    }
#pragma warning restore S1075
}
