using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using WebApp.Platform.Settings;

namespace WebApp.Platform.Local;

// Provides an authenticated user when running locally
internal class LocalAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string BasicAuthenticationScheme = "BasicAuthentication";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!ApplicationSettings.DevOptions.LocalAuthSucceeds)
            return Task.FromResult(AuthenticateResult.Fail("Invalid"));

        var claims = new[] { new Claim(ClaimTypes.Name, "Local User") };
        var ticket = new AuthenticationTicket(
            new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name)), Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        await base.HandleChallengeAsync(properties);

        await Response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes("Status Code: 403; Forbidden \r\n" +
            "To access protected pages, set 'LocalAuthSucceeds' to 'true' in the 'appsettings.Development.json' file."));
    }
}
