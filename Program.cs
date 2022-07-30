using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Serilog;
using URLShortnerMinimalApi.Data;
using URLShortnerMinimalApi.Models;
using URLShortnerMinimalApi.Mongo;
using URLShortnerMinimalApi.Routes;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

BsonClassMap.RegisterClassMap<ShortUrl>(x =>
{
    x.AutoMap();
    x.MapProperty(x => x.Chunck).SetIsRequired(true);
    x.MapProperty(x => x.Created).SetDefaultValue(() => DateTime.UtcNow);
    x.MapProperty(x => x.Active).SetDefaultValue(() => true);
    x.MapIdProperty(x => x.Id).SetIdGenerator(new StringObjectIdGenerator());
});

builder.Services.AddTransient<IApplicationDb, ApplicationDb>();
builder.Services.AddTransient<MongoProxy>(
    x => new MongoProxy(builder.Configuration.GetConnectionString("Database"))
);

var app = builder.Build();

app.UseStaticFiles();

app.MapShortcutEndpoints();

app.Run();