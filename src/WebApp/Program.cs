using WebApp.Platform.Environment;

var builder = WebApplication.CreateBuilder(args);

// Configure the UI
builder.Services.AddRazorPages();

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
app.Run();
