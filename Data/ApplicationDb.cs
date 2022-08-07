using URLShortnerMinimalApi.Models;
using URLShortnerMinimalApi.SupabaseProxy;

namespace URLShortnerMinimalApi.Data
{
    public class ApplicationDb : IApplicationDb
    {
        readonly PgDatabase _proxy;

        public ApplicationDb(PgDatabase proxy)
        {
            _proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
        }

        public async Task<ShortUrl> Create(ShortUrl data)
        {
            var d = await _proxy.Add<ShortUrl>(data);

            return data;
        }

        public async Task<List<ShortUrl>> GetAll(CancellationToken cancellationToken = default)
        {
            var all = await _proxy.List<ShortUrl>();

            return all;
        }

        public async Task<ShortUrl> GetByChunckId(string chunckId, CancellationToken cancellationToken = default)
        {
            var all = await _proxy.Get<ShortUrl, string>("chunck", chunckId);

            return all;
        }
    }
}
