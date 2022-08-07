using Serilog;
using URLShortnerMinimalApi.Data;
using URLShortnerMinimalApi.Extensions;
using URLShortnerMinimalApi.Models;
using URLShortnerMinimalApi.Routes;
using URLShortnerMinimalApi.SupabaseProxy;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

var secretsJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "secrets.json");

builder.Configuration.AddJsonFile(secretsJsonPath, optional: true);

builder.Host.UseSerilog();

// Make sure you set these (or similar)
var url = builder.Configuration["Supabase:Url"] ?? Environment.GetEnvironmentVariable("SUPABASE_URL");
var key = builder.Configuration["Supabase:Key"] ?? Environment.GetEnvironmentVariable("SUPABASE_KEY");

await Supabase.Client.InitializeAsync(url, key);

builder.Services.AddTransient<PgDatabase>();
builder.Services.AddTransient<IApplicationDb, ApplicationDb>();
builder.Services.AddAndConfigureDatabase(builder.Configuration);
builder.Services.AddAndConfigureAuth0(builder.Configuration);

var app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapShortcutEndpoints();

app.Run();