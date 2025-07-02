using Domain.Organization.Models;
using JetBrains.Annotations;
using System.Reflection;

namespace WebApp.Platform;

public static class AppSettings
{
    public static string Version { get; } = GetVersion();
    public static Raygun RaygunSettings { get; } = new();
    public static OrganizationInfo OrganizationInfo { get; } = new();
    public static Dev DevOptions { get; set; } = new();

    private static readonly Dev ProductionDefault = new()
    {
        UseLocalData = false,
        UseLocalAuth = false,
        LocalAuthSucceeds = false,
    };

    public class Dev
    {
        public bool UseLocalData { get; [UsedImplicitly] init; } = true;
        public bool UseLocalAuth { get; [UsedImplicitly] init; } = true;
        public bool LocalAuthSucceeds { get; [UsedImplicitly] init; }
    }

    public class Raygun
    {
        public string? ApiKey { get; [UsedImplicitly] init; }
    }

    private static string GetVersion()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        var segments = (entryAssembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? entryAssembly?.GetName().Version?.ToString() ?? "").Split('+');
        return segments[0] + (segments.Length > 0 ? $"+{segments[1][..Math.Min(7, segments[1].Length)]}" : "");
    }

    public static void BindAppSettings(this WebApplicationBuilder builder)
    {
        builder.Configuration.GetSection(nameof(OrganizationInfo))
            .Bind(OrganizationInfo);
        builder.Configuration.GetSection(nameof(RaygunSettings))
            .Bind(RaygunSettings);

        // Dev settings
        if (builder.Environment.IsDevelopment())
            builder.Configuration.GetSection(nameof(DevOptions)).Bind(DevOptions);
        else
            DevOptions = ProductionDefault;
    }
}
