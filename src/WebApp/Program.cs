using Domain.Compliance.Repositories;
using Domain.Facilities.Repositories;
using Domain.Monitoring.Repositories;
using Domain.Organization.Repositories;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json.Serialization;
using WebApp.Platform.Environment;

var builder = WebApplication.CreateBuilder(args);

// Configure the UI
builder.Services.AddRazorPages()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Configure the data repositories
if (builder.Environment.IsLocalDev())
{
    builder.Services.AddScoped<IFacilitiesRepository,
        LocalRepository.Facilities.FacilitiesRepository>();
    builder.Services.AddScoped<IOrganizationRepository,
        LocalRepository.Organization.OrganizationRepository>();
    builder.Services.AddScoped<IComplianceRepository,
        LocalRepository.Compliance.ComplianceRepository>();
    builder.Services.AddScoped<IMonitoringRepository,
        LocalRepository.Monitoring.MonitoringRepository>();
}
else
{
    builder.Services.AddScoped<IDbConnection>(
        db => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<IFacilitiesRepository,
        Infrastructure.Facilities.FacilitiesRepository>();
    builder.Services.AddScoped<IOrganizationRepository,
        Infrastructure.Organization.OrganizationRepository>();
    builder.Services.AddScoped<IComplianceRepository,
        Infrastructure.Compliance.ComplianceRepository>();
    builder.Services.AddScoped<IMonitoringRepository,
        Infrastructure.Monitoring.MonitoringRepository>();
}

// Build the application
var app = builder.Build();
var env = app.Environment;

// Configure the HTTP request pipeline.
if (env.IsDevelopment() || env.IsLocalDev())
{
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePages();
}
else
{
    app.UseExceptionHandler("/Error");
}

if (!env.IsLocalDev())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// Only needed if API/controllers are to be used:
app.MapControllers();

app.Run();
