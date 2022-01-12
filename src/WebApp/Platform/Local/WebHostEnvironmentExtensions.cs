namespace WebApp.Platform.Local;

internal static class WebHostEnvironmentExtensions
{
    internal static bool IsLocalDev(this IWebHostEnvironment env) => env.IsEnvironment("Local");
}
