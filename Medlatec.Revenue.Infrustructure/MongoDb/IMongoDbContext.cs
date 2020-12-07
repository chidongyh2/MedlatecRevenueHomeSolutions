using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Core.Infrastructure.MongoDb
{
    public interface IMongoDbContext
    {
        IMongoDatabase Database { get; }
        IMongoDbRepository<T, TKey> GetRepository<T, TKey>() where T : IEntity<TKey>;
        IMongoQueryable<T> Set<T>();

        /// <summary>
        /// Các bộ lọc cho từng nguồn dữ liệu
        /// </summary>
        /// <value>Các bộ lọc.</value>
        IQueryFilterProvider Filters { get; }
    }
}
