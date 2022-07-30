using MongoDB.Driver;
using URLShortnerMinimalApi.Models;
using URLShortnerMinimalApi.Mongo;

namespace URLShortnerMinimalApi.Data
{
    public class ApplicationDb : IApplicationDb
    {
        readonly MongoProxy _proxy;

        public ApplicationDb(MongoProxy proxy)
        {
            _proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
        }

        public async Task<ShortUrl> Create(ShortUrl data)
        {
            await _proxy.Database.GetCollectionShortUrl().InsertOneAsync(data);

            return data;
        }

        public async Task<List<ShortUrl>> GetAll(CancellationToken cancellationToken = default)
        {
            var filter = Builders<ShortUrl>.Filter.Eq(x => x.Active, true);
            var sort = Builders<ShortUrl>.Sort.Descending(x => x.Created);

            var all = _proxy.Database.GetCollectionShortUrl().Find(filter).Sort(sort);

            return await all.ToListAsync(cancellationToken);
        }

        public async Task<ShortUrl> GetByChunckId(string chunckId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<ShortUrl>.Filter.Eq(x => x.Active, true);

            var all = _proxy.Database.GetCollectionShortUrl().Find(filter);

            return await all.FirstOrDefaultAsync(cancellationToken);
        }
    }
}
