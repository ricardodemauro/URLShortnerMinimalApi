using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Serilog;
using URLShortnerMinimalApi.Data;
using URLShortnerMinimalApi.Extensions;
using URLShortnerMinimalApi.Models;
using URLShortnerMinimalApi.Mongo;
using URLShortnerMinimalApi.Routes;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddAndConfigureDatabase(builder.Configuration);
builder.Services.AddAndConfigureAuth0(builder.Configuration);

var app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapShortcutEndpoints();

app.Run();