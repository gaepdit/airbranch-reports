namespace WebApp.Platform.Local;

internal static class WebHostEnvironmentExtensions
{
    internal static bool IsLocalEnv(this IWebHostEnvironment env) => env.IsEnvironment("Local");
}
