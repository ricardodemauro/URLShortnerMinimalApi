using LiteDB;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Db");
builder.Services.AddSingleton<ILiteDatabase, LiteDatabase>(x => new LiteDatabase(connectionString));

var app = builder.Build();

app.MapGet("/", async (HttpContext ctx) =>
{
    ctx.Response.Headers.ContentType = new Microsoft.Extensions.Primitives.StringValues("text/html; charset=UTF-8");
    await ctx.Response.SendFileAsync("wwwroot/index.html");
});

app.MapGet("/{chunck}", (string chunck, ILiteDatabase db) =>
    db.GetCollection<ShortUrl>().FindOne(x => x.Chunck == chunck)
    is ShortUrl url
    ? Results.Redirect(url.Url)
    : Results.NotFound());

app.MapPost("/urls", (ShortUrl shortUrl, HttpContext ctx, ILiteDatabase db) =>
{
    if (Uri.TryCreate(shortUrl.Url, UriKind.RelativeOrAbsolute, out Uri? parsedUri))
    {
        shortUrl.Chunck = Nanoid.Nanoid.Generate(size: 9);

        db.GetCollection<ShortUrl>(BsonAutoId.Guid).Insert(shortUrl);

        var rawShortUrl = $"{ctx.Request.Scheme}://{ctx.Request.Host}/{shortUrl.Chunck}";

        return Results.Ok(new { ShortUrl = rawShortUrl });
    }
    return Results.BadRequest(new { ErrorMessage = "Invalid Url" });
});

app.MapGet("/urls", (ILiteDatabase db) => Results.Ok(db.GetCollection<ShortUrl>().FindAll()));

app.Run();


public record class ShortUrl(string Url)
{
    public Guid Id { get; set; }

    public string? Chunck { get; set; }
}