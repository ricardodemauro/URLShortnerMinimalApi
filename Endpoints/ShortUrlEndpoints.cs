using Microsoft.Extensions.Primitives;
using URLShortnerMinimalApi.Data;
using URLShortnerMinimalApi.Extensions;
using URLShortnerMinimalApi.Models;

namespace URLShortnerMinimalApi.Endpoints
{
    public static class ShortUrlEndpoints
    {
        public static void MapShortUrlEndpoints(this IEndpointRouteBuilder app)
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

                    var shortDb = new ShortUrl
                    {
                        Active = true,
                        Chunck = chunck,
                        CreatedAt = DateTime.UtcNow,
                        Url = shortUrl.Url,
                        UserSub = ctx?.User?.UserId() ?? string.Empty,
                        UserDisplay = ctx?.User?.UserDisplay() ?? string.Empty
                    };

                    await db.Create(shortDb);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    var rawShortUrl = $"{ctx.Request.Scheme}://{ctx.Request.Host}/{shortDb.Chunck}";
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    return Results.Ok(new { ShortUrl = rawShortUrl });
                }
                return Results.BadRequest(new { ErrorMessage = "Invalid Url" });
            }).RequireAuthorization("create:url");

            app.MapGet("/urls", async (IApplicationDb db) => Results.Ok(await db.GetAll()));

        }
    }
}
