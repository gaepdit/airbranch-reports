using Domain.Compliance.Repositories;
using Domain.Facilities.Repositories;
using Domain.StackTest.Repositories;
using Infrastructure.DbConnection;
using LocalRepository.Compliance;
using LocalRepository.Facilities;
using LocalRepository.StackTest;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.Text.Json.Serialization;
using WebApp.Platform;

var builder = WebApplication.CreateBuilder(args);

// Set default timeout for regular expressions.
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices#use-time-out-values
// ReSharper disable once HeapView.BoxingAllocation
AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(100));

// Set Application Settings
builder.BindAppSettings();
builder.AddErrorLogging();

// Configure authentication
if (AppSettings.DevOptions.UseLocalAuth)
{
    // Optionally handles authentication internally.
    builder.Services
        .AddAuthentication(LocalAuthenticationHandler.BasicAuthenticationScheme)
        .AddScheme<AuthenticationSchemeOptions, LocalAuthenticationHandler>(
            LocalAuthenticationHandler.BasicAuthenticationScheme, null);
}
else
{
    // When running on the server, requires an Azure AD login account (configured in the app settings file).
    builder.Services
        .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
}

// Persist data protection keys
var keysFolder = Path.Combine(builder.Configuration["PersistedFilesBasePath"] ?? "./", "DataProtectionKeys");
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(Directory.CreateDirectory(keysFolder));
builder.Services.AddAuthorization();

// Configure the UI
builder.Services
    .AddRazorPages()
    .AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .AddMicrosoftIdentityUI();

// Configure HSTS (max age: two years) and HTTPS redirection.
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHsts(opts => opts.MaxAge = TimeSpan.FromDays(730));
    builder.Services.AddHttpsRedirection(options =>
    {
        options.HttpsPort = 443;
        options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
    });
}
else
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.HttpsPort = 443;
        options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    });
}

// Configure the data repositories
if (AppSettings.DevOptions.UseLocalData)
{
    // Uses sample data when running locally.
    builder.Services.AddScoped<IFacilitiesRepository, FacilitiesRepository>();
    builder.Services.AddScoped<IComplianceRepository, ComplianceRepository>();
    builder.Services.AddScoped<IStackTestRepository, StackTestRepository>();
}
else
{
    // When running on the server, requires a deployed database (configured in the app settings file).
    // (Note: this pattern works because we only have a single DB connection string.
    // See https://stackoverflow.com/a/47403685/212978 for more info.)
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                           ?? throw new ArgumentException("Connection string missing.");
    builder.Services.AddTransient<IDbConnectionFactory, DbConnectionFactory>(_ =>
        new DbConnectionFactory(connectionString));

    builder.Services.AddScoped<IFacilitiesRepository, Infrastructure.Facilities.FacilitiesRepository>();
    builder.Services.AddScoped<IComplianceRepository, Infrastructure.Compliance.ComplianceRepository>();
    builder.Services.AddScoped<IStackTestRepository, Infrastructure.StackTest.StackTestRepository>();
}

// Build the application
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseSecurityHeaders(policyCollection => policyCollection.AddSecurityHeaderPolicies());
}

app.UseErrorHandling();
app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers(); // Only needed if API/controllers are to be used

await app.RunAsync();
