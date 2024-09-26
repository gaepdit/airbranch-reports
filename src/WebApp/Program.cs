using Domain.Compliance.Repositories;
using Domain.Facilities.Models;
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
using Microsoft.OpenApi.Models;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.AspNetCore;
using System.Text.Json.Serialization;
using WebApp.Platform.Local;
using WebApp.Platform.SecurityHeaders;
using WebApp.Platform.Settings;

var builder = WebApplication.CreateBuilder(args);

// Set default timeout for regular expressions.
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices#use-time-out-values
// ReSharper disable once HeapView.BoxingAllocation
AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(100));

// Set Application Settings
ApplicationSettings.BindSettings(builder);

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

// Configure the UI.
builder.Services
    .AddRazorPages()
    .AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .AddMicrosoftIdentityUI();

// Configure HSTS (max age: two years)
if (!builder.Environment.IsDevelopment())
    builder.Services.AddHsts(opts => opts.MaxAge = TimeSpan.FromDays(730));

// Configure application monitoring.
if (!string.IsNullOrEmpty(ApplicationSettings.RaygunSettings.ApiKey))
{
    builder.Services.AddSingleton(provider =>
    {
        var client = new RaygunClient(provider.GetService<RaygunSettings>()!,
            provider.GetService<IRaygunUserProvider>()!);
        client.SendingMessage += (_, eventArgs) =>
            eventArgs.Message.Details.Tags.Add(builder.Environment.EnvironmentName);
        return client;
    });
    builder.Services.AddRaygun(opts =>
    {
        opts.ApiKey = ApplicationSettings.RaygunSettings.ApiKey;
        opts.ApplicationVersion = ApplicationSettings.RaygunSettings.InformationalVersion;
        opts.IgnoreFormFieldNames = ["*Password"];
        opts.EnvironmentVariables.Add("ASPNETCORE_*");
    });
    builder.Services.AddRaygunUserProvider();
}

// Configure the data repositories.
if (ApplicationSettings.DevOptions.UseLocalData)
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

// Add API documentation.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Air Branch API",
    });
});

// Build the application.
var app = builder.Build();

// Configure error handling.
if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage(); // Development
else app.UseExceptionHandler("/Error"); // Production or Staging

// Configure security HTTP headers
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseSecurityHeaders(policyCollection => policyCollection.AddSecurityHeaderPolicies());
}

if (!string.IsNullOrEmpty(ApplicationSettings.RaygunSettings.ApiKey)) app.UseRaygun();

// Configure the application pipeline.
app.UseStatusCodePages()
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();

// Configure API documentation.
app.UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/openapi.json"; })
    .UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("v1/openapi.json", "Air Branch API v1");
        options.RoutePrefix = "api-docs";
        options.DocumentTitle = "Air Branch API";
    });

// Map endpoints.
app.MapRazorPages();
app.MapControllers();

// Add minimal APIs for retrieving facility data.
app.MapGet("/api/facility/{id}", async (IFacilitiesRepository repo, string id) =>
{
    if (!FacilityId.IsValidFormat(id)) return Results.NotFound();
    var facility = await repo.GetFacilityAsync(new FacilityId(id));
    return facility is not null ? Results.Ok(facility) : Results.NotFound();
}).WithName("GetFacility").WithOpenApi();

app.MapGet("/api/facility/exists/{id}", async (IFacilitiesRepository repo, string id) =>
    Results.Ok(await repo.FacilityExistsAsync(id))
).WithName("FacilityExists").WithOpenApi();

// Make it so.
await app.RunAsync();
