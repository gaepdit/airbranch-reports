using JetBrains.Annotations;
using WebApp.Platform.Models;

namespace WebApp.Platform.Settings;

public static class ApplicationSettings
{
    public static RaygunSettings RaygunSettings { get; } = new();
    public static DevOptions DevOptions { get; set; } = new();
    public static OrganizationInfo OrganizationInfo { get; set; } = new();

    public static readonly DevOptions ProductionDefault = new()
    {
        UseLocalData = false,
        UseLocalAuth = false,
        LocalAuthSucceeds = false,
    };
}

public class RaygunSettings
{
    public string ApiKey { get; [UsedImplicitly] init; } = "";
}

public class DevOptions
{
    public bool UseLocalData { get; [UsedImplicitly] init; } = true;
    public bool UseLocalAuth { get; [UsedImplicitly] init; } = true;
    public bool LocalAuthSucceeds { get; [UsedImplicitly] init; }
}
