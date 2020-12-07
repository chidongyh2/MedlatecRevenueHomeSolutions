using Medlatec.Revenue.Infrustructure.Contants;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Security.Authentication;

namespace CMS_Core.Infrastructure.MongoDb
{
    public class MongoDbContext : IMongoDbContext, IDisposable
    {
        private readonly Lazy<QueryFilterProvider> _filterProviderInitializer = new Lazy<QueryFilterProvider>();
        private IMongoClient _client;
        private IMongoDatabase _database;
        public IMongoDatabase Database => _database;
        public MongoDbContext()
        {
        }

        public MongoDbContext(IConfiguration configuration)
        {
            // Đăng ký conventionPack
            RegisterConventionPack();
            var mongoSettings = configuration.GetSection("ConnectionStrings").Get<MongoDBSettings>();
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(mongoSettings.Host, mongoSettings.Port);

            settings.UseTls = mongoSettings.UserTls;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            MongoIdentity identity = new MongoInternalIdentity(mongoSettings.AuthDbName, mongoSettings.UserName);
            MongoIdentityEvidence evidence = new PasswordEvidence(mongoSettings.Password);

            settings.Credential = new MongoCredential(mongoSettings.AuthMechanism, identity, evidence);

            MongoClient client = new MongoClient(settings);
            _database = client.GetDatabase(mongoSettings.DbName);
            //var mongoUrl = new MongoUrl(connectionString);
            //_client = new MongoClient(mongoUrl);
            //_database = _client.GetDatabase(mongoUrl.DatabaseName);
        }

        private void RegisterConventionPack()
        {
            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", conventionPack, t => true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client = null;
                _database = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IMongoDbRepository<T, TKey> GetRepository<T, TKey>() where T : IEntity<TKey>
        {
            return new MongoDbRepository<T, TKey>(this);
        }

        public IMongoQueryable<T> Set<T>()
        {
            return _database.GetCollection<T>(nameof(T)) as IMongoQueryable<T>;
        }

        public IQueryFilterProvider Filters => _filterProviderInitializer.Value;
    }
}
