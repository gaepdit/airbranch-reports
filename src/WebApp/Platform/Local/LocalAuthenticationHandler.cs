using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace WebApp.Platform.Local;

// Provides an authenticated user when running locally
internal class LocalAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string BasicAuthenticationScheme = "BasicAuthentication";
    private readonly IConfiguration _configuration;

    public LocalAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IConfiguration configuration)
        : base(options, logger, encoder, clock) =>
        _configuration = configuration;

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (_configuration.GetValue<bool>("AuthenticatedUser"))
        {
            var claims = new[] { new Claim(ClaimTypes.Name, "Local User") };
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name)),
                Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        return Task.FromResult(AuthenticateResult.Fail("Invalid"));
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        await base.HandleChallengeAsync(properties);

        await Response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes("Status Code: 403; Forbidden \r\n" +
            "To access protected pages, set 'AuthenticatedUser' to 'true' in 'appsettings.Local.json' file."));
    }
}
