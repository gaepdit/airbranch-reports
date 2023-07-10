using Domain.Compliance.Repositories;
using Domain.Facilities.Repositories;
using Domain.StackTest.Repositories;
using Infrastructure.DbConnection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Mindscape.Raygun4Net.AspNetCore;
using System.Text.Json.Serialization;
using WebApp.Platform.Local;
using WebApp.Platform.Raygun;
using WebApp.Platform.SecurityHeaders;
using WebApp.Platform.Settings;

var builder = WebApplication.CreateBuilder(args);

// Set Application Settings
builder.Configuration.GetSection(nameof(ApplicationSettings.RaygunSettings)).Bind(ApplicationSettings.RaygunSettings);
builder.Configuration.GetSection(nameof(ApplicationSettings.OrganizationInfo)).Bind(ApplicationSettings.OrganizationInfo);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.GetSection(nameof(ApplicationSettings.DevOptions)).Bind(ApplicationSettings.DevOptions);
}
else
{
    ApplicationSettings.DevOptions = ApplicationSettings.ProductionDefault;
}

// Configure authentication
if (ApplicationSettings.DevOptions.UseLocalAuth)
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

// Configure HSTS (max age: two years)
if (!builder.Environment.IsDevelopment())
    builder.Services.AddHsts(opts => opts.MaxAge = TimeSpan.FromDays(730));

// Configure application monitoring
builder.Services.AddRaygun(builder.Configuration,
    new RaygunMiddlewareSettings { ClientProvider = new RaygunClientProvider() });
builder.Services.AddHttpContextAccessor(); // needed by RaygunScriptPartial

// Configure the data repositories
if (ApplicationSettings.DevOptions.UseLocalData)
{
    // Uses sample data when running locally.
    builder.Services.AddScoped<IFacilitiesRepository, LocalRepository.Facilities.FacilitiesRepository>();
    builder.Services.AddScoped<IComplianceRepository, LocalRepository.Compliance.ComplianceRepository>();
    builder.Services.AddScoped<IStackTestRepository, LocalRepository.StackTest.StackTestRepository>();
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
var env = app.Environment;

// Configure the HTTP request pipeline.
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseRaygun();
}

// Configure security HTTP headers
app.UseSecurityHeaders(policies => policies.AddSecurityHeaderPolicies());

app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers(); // Only needed if API/controllers are to be used

app.Run();
