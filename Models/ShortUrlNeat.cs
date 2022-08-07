using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShortnerMinimalApi.Models
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public record class ShortUrlNeat(
        string Url,
        string Chunck,
        bool Active,
        DateTime CreatedAt,
        int Id,
        string UserDisplay,
        string UserUrl)
    {
        public static implicit operator ShortUrlNeat(ShortUrl d)
            => new ShortUrlNeat(d.Url, d.Chunck, d.Active, d.CreatedAt, d.Id, d.UserDisplay, d.UserUrl);
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public static class ShortUrlNeatExtensions
    {
        public static ShortUrlNeat ToNeat(this ShortUrl data)
            => data;
    }
}
