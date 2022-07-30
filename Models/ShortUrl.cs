using MongoDB.Bson;

namespace URLShortnerMinimalApi.Models
{
    public record class ShortUrl(string Url, string Chunck, bool Active, DateTime Created)
    {
        public string? Id { get; set; }
    }
}
