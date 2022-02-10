using Air.WebApp.Platform.Raygun;
using Domain.Compliance.Repositories;
using Domain.Facilities.Repositories;
using Domain.Organization.Repositories;
using Domain.StackTest.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Mindscape.Raygun4Net.AspNetCore;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json.Serialization;
using WebApp.Platform.Local;

var builder = WebApplication.CreateBuilder(args);

// Configure authentication
if (builder.Environment.IsLocalDev())
{
    // When running locally, uses a built-in authenticated user.
    builder.Services
        .AddAuthentication(LocalAuthenticationHandler.BasicAuthenticationScheme)
        .AddScheme<AuthenticationSchemeOptions, LocalAuthenticationHandler>(LocalAuthenticationHandler.BasicAuthenticationScheme, null);
}
else
{
    // When running on the server, requires an Azure AD login account (configured in the appsettings file).
    builder.Services
        .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
}

// Persist data protection keys
var keysFolder = Path.Combine(builder.Configuration["PersistedFilesBasePath"], "DataProtectionKeys");
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(Directory.CreateDirectory(keysFolder));
builder.Services.AddAuthorization();

// Configure the UI
builder.Services
    .AddRazorPages()
    .AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .AddMicrosoftIdentityUI();

// Configure HSTS (max age: two years)
builder.Services.AddHsts(opts => opts.MaxAge = TimeSpan.FromDays(730));

// Configure application monitoring
builder.Services.AddRaygun(builder.Configuration,
    new RaygunMiddlewareSettings { ClientProvider = new RaygunClientProvider() });
builder.Services.AddHttpContextAccessor(); // needed by RaygunScriptPartial

// Configure the data repositories
if (builder.Environment.IsLocalDev())
{
    // Uses sample data when running locally.
    builder.Services.AddScoped<IFacilitiesRepository,
        LocalRepository.Facilities.FacilitiesRepository>();
    builder.Services.AddScoped<IOrganizationRepository,
        LocalRepository.Organization.OrganizationRepository>();
    builder.Services.AddScoped<IComplianceRepository,
        LocalRepository.Compliance.ComplianceRepository>();
    builder.Services.AddScoped<IStackTestRepository,
        LocalRepository.StackTest.StackTestRepository>();
}
else
{
    // When running on the server, requires a deployed database (configured in the appsettings file).
    builder.Services.AddScoped<IDbConnection>(
        db => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<IFacilitiesRepository,
        Infrastructure.Facilities.FacilitiesRepository>();
    builder.Services.AddScoped<IOrganizationRepository,
        Infrastructure.Organization.OrganizationRepository>();
    builder.Services.AddScoped<IComplianceRepository,
        Infrastructure.Compliance.ComplianceRepository>();
    builder.Services.AddScoped<IStackTestRepository,
        Infrastructure.StackTest.StackTestRepository>();
}

// Build the application
var app = builder.Build();
var env = app.Environment;

// Configure the HTTP request pipeline.
if (env.IsDevelopment() || env.IsLocalDev())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

if (!env.IsLocalDev())
{
    app.UseRaygun();
    app.UseHsts();
}

app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
// Only needed if API/controllers are to be used:
app.MapControllers();

app.Run();
