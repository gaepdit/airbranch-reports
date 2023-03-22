using WebApp.Platform.Settings;

namespace WebApp.Platform.SecurityHeaders;

internal static class SecurityHeaders
{
    internal static void AddSecurityHeaderPolicies(this HeaderPolicyCollection policies)
    {
        policies.AddFrameOptionsDeny();
        policies.AddXssProtectionBlock();
        policies.AddContentTypeOptionsNoSniff();
        policies.AddReferrerPolicyStrictOriginWhenCrossOrigin();
        policies.RemoveServerHeader();
        policies.AddContentSecurityPolicy(builder => builder.CspBuilder());
        policies.AddCustomHeader("Reporting-Endpoints",
            $"default=\"https://report-to-api.raygun.com/reports?apikey={ApplicationSettings.RaygunSettings.ApiKey}\",csp-endpoint=\"https://report-to-api.raygun.com/reports-csp?apikey={ApplicationSettings.RaygunSettings.ApiKey}\"");
        policies.AddCustomHeader("Report-To",
            $"{{\"group\":\"default\",\"max_age\":10886400,\"endpoints\":[{{\"url\":\"https://report-to-api.raygun.com/reports?apikey={ApplicationSettings.RaygunSettings.ApiKey}\"}}]}},{{\"group\":\"csp-endpoint\",\"max_age\":10886400,\"endpoints\":[{{\"url\":\"https://report-to-api.raygun.com/reports-csp?apikey={ApplicationSettings.RaygunSettings.ApiKey}\"}}]}}");
    }

#pragma warning disable S1075 // "URIs should not be hardcoded"
    private static void CspBuilder(this CspBuilder builder)
    {
        builder.AddDefaultSrc().None();
        builder.AddBaseUri().None();
        builder.AddScriptSrc()
            .From("https://cdn.raygun.io/raygun4js/raygun.min.js")
            .WithHash256("kOJzCjwwBHVC6EAEX5M+ovfu9sE7JG0G9LcYssttn6I=") // Raygun CDN loader
            .WithHash256("k8lqom5XjWiHpIL9TqKQ7DpRVbQNTtRtBFIKZ0iQaBk=") // Raygun pulse
            .WithHashTagHelper()
            .ReportSample();
        builder.AddStyleSrc()
            .Self()
            .From("https://cdn.skypack.dev/-/bootstrap@v5.2.2-rQQzEZiIuMxXgDelisA1/")
            .From("https://cdn.skypack.dev/-/sanitize.css@v13.0.0-9hf8PtILaGjq3949IzOc/")
            .ReportSample();
        builder.AddImgSrc().Self();
        builder.AddConnectSrc().From("https://api.raygun.io");
        builder.AddFontSrc().Self();
        builder.AddFormAction()
            .Self()
            .From("https://login.microsoftonline.com");
        builder.AddManifestSrc().Self();
        builder.AddFrameAncestors().None();
        builder.AddReportUri()
            .To($"https://report-to-api.raygun.com/reports-csp?apikey={ApplicationSettings.RaygunSettings.ApiKey}");
        builder.AddCustomDirective("report-to", "csp-endpoint");
    }
#pragma warning restore S1075
}
