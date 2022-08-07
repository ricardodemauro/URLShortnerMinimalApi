using Microsoft.Extensions.FileProviders;
using Serilog;
using URLShortnerMinimalApi.Data;
using URLShortnerMinimalApi.Endpoints;
using URLShortnerMinimalApi.Extensions;
using URLShortnerMinimalApi.SupabaseProxy;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Extensions.Hosting", Serilog.Events.LogEventLevel.Debug)
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

var secretsJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "secrets.json");

builder.Configuration.AddJsonFile(secretsJsonPath, optional: true);

builder.Host.UseSerilog();

// Make sure you set these (or similar)
var url = builder.Configuration["Supabase:Url"] ?? Environment.GetEnvironmentVariable("SUPABASE_URL");
var key = builder.Configuration["Supabase:Key"] ?? Environment.GetEnvironmentVariable("SUPABASE_KEY");

await Supabase.Client.InitializeAsync(url, key);

builder.Services.AddTransient<IApplicationDb, ApplicationDb>();
builder.Services.AddTransient<PgDatabase>();
builder.Services.AddAndConfigureAuth0(builder.Configuration);

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    RequestPath = "/static",
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
});

app.UseAuthentication();
app.UseAuthorization();

app.MapShortUrlEndpoints();

app.Run();