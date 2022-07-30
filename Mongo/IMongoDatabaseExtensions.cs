using MongoDB.Driver;
using URLShortnerMinimalApi.Models;

namespace URLShortnerMinimalApi.Mongo
{
    public static class IMongoDatabaseExtensions
    {
        public static IMongoCollection<ShortUrl> GetCollectionShortUrl(this IMongoDatabase data)
            => data.GetCollection<ShortUrl>("short");
    }
}
