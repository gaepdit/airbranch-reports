using Domain.Organization.Models;
using JetBrains.Annotations;
using System.Reflection;

namespace WebApp.Platform.Settings;

public static class ApplicationSettings
{
    public static RaygunClientSettings RaygunSettings { get; } = new();

    public static DevOptions DevOptions { get; set; } = new();

    public static OrganizationInfo OrganizationInfo { get; set; } = new();

    private static readonly DevOptions ProductionDefault = new()
    {
        UseLocalData = false,
        UseLocalAuth = false,
        LocalAuthSucceeds = false,
    };

    public static void BindSettings(WebApplicationBuilder builder)
    {
        builder.Configuration.GetSection(nameof(RaygunSettings)).Bind(RaygunSettings);
        builder.Configuration.GetSection(nameof(OrganizationInfo))
            .Bind(OrganizationInfo);

        // App versioning
        var versionSegments = (Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "").Split('+');
        RaygunSettings.InformationalVersion = versionSegments[0];

        // Dev settings
        if (builder.Environment.IsDevelopment())
            builder.Configuration.GetSection(nameof(DevOptions)).Bind(DevOptions);
        else
            DevOptions = ProductionDefault;
    }
}

public class RaygunClientSettings
{
    public string? ApiKey { get; [UsedImplicitly] init; }
    public string? InformationalVersion { get; set; }
}

public class DevOptions
{
    public bool UseLocalData { get; [UsedImplicitly] init; } = true;
    public bool UseLocalAuth { get; [UsedImplicitly] init; } = true;
    public bool LocalAuthSucceeds { get; [UsedImplicitly] init; }
}
