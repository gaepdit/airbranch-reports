namespace WebApp.Platform.Environment;

public static class WebHostEnvironmentExtensions
{
    public static bool IsLocalDev(this IWebHostEnvironment env) => env.IsEnvironment("Local");
}
