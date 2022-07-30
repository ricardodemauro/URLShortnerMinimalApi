using Microsoft.Extensions.Primitives;
using URLShortnerMinimalApi.Data;
using URLShortnerMinimalApi.Models;

namespace URLShortnerMinimalApi.Routes
{
    public static class ShortUrlEndpoints
    {
        public static void MapShortcutEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", async (HttpContext ctx) =>
            {
                ctx.Response.Headers.ContentType = new StringValues("text/html; charset=UTF-8");
                await ctx.Response.SendFileAsync("wwwroot/index.html");
            });

            app.MapGet("/{chunck}", async (string chunck, IApplicationDb db) =>
                await db.GetByChunckId(chunck)
                is ShortUrl url
                ? Results.Redirect(url.Url)
                : Results.NotFound());

            app.MapPost("/urls", async (ShortUrl shortUrl, HttpContext ctx, IApplicationDb db) =>
            {
                if (Uri.TryCreate(shortUrl.Url, UriKind.RelativeOrAbsolute, out Uri? parsedUri))
                {
                    var chunck = Nanoid.Nanoid.Generate(size: 9);

                    var shortDb = shortUrl with
                    {
                        Active = true,
                        Chunck = chunck,
                        Created = DateTime.UtcNow
                    };

                    await db.Create(shortDb);

                    var rawShortUrl = $"{ctx.Request.Scheme}://{ctx.Request.Host}/{shortUrl.Chunck}";

                    return Results.Ok(new { ShortUrl = rawShortUrl });
                }
                return Results.BadRequest(new { ErrorMessage = "Invalid Url" });
            });

            app.MapGet("/urls", (IApplicationDb db) => Results.Ok(db.GetAll()));

        }
    }
}
