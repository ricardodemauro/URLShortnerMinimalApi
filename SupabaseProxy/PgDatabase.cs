using Supabase;
using System.Linq.Expressions;

namespace URLShortnerMinimalApi.SupabaseProxy
{
    public class PgDatabase
    {
        readonly ILogger<PgDatabase> _logger;

        public PgDatabase(ILogger<PgDatabase> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<T> Add<T>(T model) where T : SupabaseModel, new()
        {
            var instance = Client.Instance;

            var apiResponse = await instance.From<T>().Insert(model);

            _logger.LogDebug("ApiResponse {Content}", apiResponse.Content);

            return apiResponse?.Models?.Count > 0 ? apiResponse?.Models[0] ?? new() : new T();
        }

        public async Task<List<T>> List<T>() where T : SupabaseModel, new()
        {
            var instance = Client.Instance;

            var channels = await instance.From<T>().Get();

            _logger.LogDebug("ApiResponse {Content}", channels?.Content);

            return channels?.Models ?? new List<T>();
        }

        public async Task<T> Get<T, TFilter>(string field, TFilter value) where T : SupabaseModel, new()
        {
            var instance = Client.Instance;

            var filter = new Dictionary<string, string>()
            {
                [field] = value?.ToString() ?? string.Empty
            };

            var channels = await instance.From<T>().Match(filter).Single();

            _logger.LogDebug("return from {channels}", channels);

            return channels;
        }
    }
}
