using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CMS_Core.Infrastructure.MongoDb
{
    public class MongoDbRepository<T, TKey> : IMongoDbRepository<T, TKey> where T : IEntity<TKey>
    {
        public IMongoCollection<T> Collection { get; set; }
        public IMongoDatabase Database { get; set; }

        public Expression Expression { get; }

        public Type ElementType { get; }

        public IQueryProvider Provider { get; }

        public MongoDbRepository(IMongoDbContext context)
        {
            Database = context.Database;
            Collection = context.Database.GetCollection<T>(typeof(T).Name);
        }

        public T Get(TKey id)
        {
            return Collection.Find(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return Collection.Find(predicate).FirstOrDefault();
        }

        public List<T> Gets(Expression<Func<T, bool>> predicate = null,
            Func<IMongoQueryable<T>, IMongoQueryable<T>> preFilters = null,
            params Func<IMongoQueryable<T>, IMongoQueryable<T>>[] postFilters)
        {
            return FindCore(predicate, preFilters, postFilters).ToList();
        }

        public List<TOutput> GetsAs<TOutput>(Expression<Func<T, TOutput>> projector, Expression<Func<T, bool>> predicate = null, Func<IMongoQueryable<T>, IMongoQueryable<T>> preFilters = null,
            params Func<IMongoQueryable<T>, IMongoQueryable<T>>[] postFilters)
        {
            if (projector == null)
                throw new ArgumentNullException(nameof(projector));

            return FindCore(predicate, preFilters, postFilters).Select(projector).ToList();
        }

        public T Add(T entity)
        {
            Collection.InsertOne(entity);
            return entity;
        }

        public long Add(IEnumerable<T> entities)
        {
            Collection.InsertMany(entities);
            return 1;
        }

        public long Update(T entity, UpdateOptions options = null)
        {
            var updateBuilder = BuildUpdate(entity);
            var result = Collection.UpdateOne(x => x.Id.Equals(entity.Id), updateBuilder);
            return result.ModifiedCount;
        }

        public long UpdateMany(IEnumerable<T> entities, UpdateOptions options = null)
        {
            return entities.Sum(entity => Update(entity));
        }

        public long Delete(string id)
        {
            return Collection.DeleteOne(x => x.Id.Equals(id)).DeletedCount;
        }

        public long Delete(Expression<Func<T, bool>> predicate)
        {
            return Collection.DeleteOne(predicate).DeletedCount;
        }

        public long DeleteAll()
        {
            return Collection.DeleteMany(null).DeletedCount;
        }

        public long Count(Expression<Func<T, bool>> predicate)
        {
            // return Collection.AsQueryable().Count(predicate);
            return Collection.AsQueryable().Count(predicate);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return Collection.AsQueryable().Any(predicate);
        }

        public IMongoQueryable<T> Raw => Collection.AsQueryable();

        public async Task<T> GetAsync(string id)
        {
            return await Collection.AsQueryable().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate = null)
        {
            return await Collection.AsQueryable().FirstOrDefaultAsync(predicate);
        }

        public Task<List<T>> GetsAsync(Expression<Func<T, bool>> predicate = null,
            Func<IMongoQueryable<T>, IMongoQueryable<T>> preFilters = null,
            params Func<IMongoQueryable<T>, IMongoQueryable<T>>[] postFilters)
        {
            return FindCore(predicate, preFilters, postFilters).ToListAsync();
        }

        public Task<List<TOutput>> GetsAsAsync<TOutput>(Expression<Func<T, TOutput>> projector, Expression<Func<T, bool>> predicate = null, Func<IMongoQueryable<T>, IMongoQueryable<T>> preFilters = null,
            params Func<IMongoQueryable<T>, IMongoQueryable<T>>[] postFilters)
        {
            if (projector == null)
                throw new ArgumentNullException(nameof(projector));

            return FindCore(predicate, preFilters, postFilters).Select(projector).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await Collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<List<T>> AddManyAsync(IEnumerable<T> entities)
        {
            var listInserted = new List<T>();
            foreach (var entity in entities)
            {
                await Collection.InsertOneAsync(entity);
                listInserted.Add(entity);
            }
            return listInserted;
        }

        public async Task<long> AddManyAsync(List<T> entities)
        {
            await Collection.InsertManyAsync(entities);
            return 1;
        }

        public async Task<long> UpdateAsync(T entity, UpdateOptions options = null)
        {
            var updateBuilder = BuildUpdate(entity);
            var result = await Collection.UpdateOneAsync(x => x.Id.Equals(entity.Id), updateBuilder);
            return result.ModifiedCount;
        }

        public async Task<long> UpdateAsync(Expression<Func<T, bool>> predicate, UpdateDefinition<T> updateDefinition, UpdateOptions options = null)
        {
            var result = await Collection.UpdateOneAsync(predicate, updateDefinition, options);
            return result.ModifiedCount;
        }

        public async Task<long> UpdateAsync(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition, UpdateOptions options = null)
        {
            var result = await Collection.UpdateManyAsync(filterDefinition, updateDefinition);
            return result.ModifiedCount;
        }

        public async Task<long> UpdateAsync(Expression<Func<T, bool>> predicate, T entity, UpdateOptions options = null)
        {
            var updateBuilder = BuildUpdate(entity);
            var result = await Collection.UpdateOneAsync(predicate, updateBuilder);
            return result.ModifiedCount;
        }

        public async Task<long> UpdateManyAsync(Expression<Func<T, bool>> predicate, UpdateDefinition<T> updateDefinition, UpdateOptions options = null)
        {
            var result = await Collection.UpdateManyAsync(predicate, updateDefinition, options);
            return result.ModifiedCount;
        }

        public async Task<long> UpdateManyAsync(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition, UpdateOptions options = null)
        {
            var result = await Collection.UpdateManyAsync(filterDefinition, updateDefinition, options);
            return result.ModifiedCount;
        }

        public async Task<long> DeleteAsync(TKey id)
        {
            var result = await Collection.DeleteOneAsync(x => x.Id.Equals(id));
            return result.DeletedCount;
        }

        public async Task<long> DeleteAsync(Expression<Func<T, bool>> predicate = null)
        {
            var result = await Collection.DeleteOneAsync(predicate);
            return result.DeletedCount;
        }

        public async Task<long> DeleteManyAsync(Expression<Func<T, bool>> predicate = null)
        {
            var result = await Collection.DeleteManyAsync(predicate);
            return result.DeletedCount;
        }

        public async Task<long> CountAysnc(Expression<Func<T, bool>> predicate = null)
        {
            // return await Collection.CountAsync(predicate);
            return await Collection.AsQueryable().CountAsync(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate = null)
        {
            return await Collection.AsQueryable().AnyAsync(predicate);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Collection.AsQueryable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Collection.AsQueryable().GetEnumerator();
        }


        #region Private

        private IMongoQueryable<T> FindCore(Expression<Func<T, bool>> predicate,
            Func<IMongoQueryable<T>, IMongoQueryable<T>> preFilter, params Func<IMongoQueryable<T>, IMongoQueryable<T>>[] postFilters)
        {
            var result = preFilter != null ? preFilter(Collection.AsQueryable()) : Collection.AsQueryable();
            if (predicate != null)
            {
                result = result.Where(predicate);
            }
            return postFilters.Where(postFilter => postFilter != null).Aggregate(result, (current, postFilter) => postFilter(current));
        }

        private static UpdateDefinition<T> BuildUpdate(T doc)
        {
            var builder = Builders<T>.Update;
            var updates = (from prop in typeof(T).GetProperties()
                           where prop.Name != "Id"
                           select builder.Set(prop.Name, prop.GetValue(doc))).ToList();
            return builder.Combine(updates);
        }

        #endregion
    }
}
