using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using URLShortnerMinimalApi.Data;
using URLShortnerMinimalApi.Models;
using URLShortnerMinimalApi.Mongo;

namespace URLShortnerMinimalApi.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddAndConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            BsonClassMap.RegisterClassMap<ShortUrl>(x =>
            {
                x.AutoMap();
                x.MapProperty(x => x.Chunck).SetIsRequired(true);
                x.MapProperty(x => x.Created).SetDefaultValue(() => DateTime.UtcNow);
                x.MapProperty(x => x.Active).SetDefaultValue(() => true);
                x.MapIdProperty(x => x.Id).SetIdGenerator(new StringObjectIdGenerator());
            });

            services.AddTransient<IApplicationDb, ApplicationDb>();
            services.AddTransient<MongoProxy>(
                x => new MongoProxy(configuration.GetConnectionString("Database"))
            );

            return services;
        }
    }
}
