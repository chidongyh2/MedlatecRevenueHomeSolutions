using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CMS_Core.Infrastructure.MongoDb
{
    public interface IMongoDbRepository<T, in TKey> : IQueryable<T> where T: IEntity<TKey>
    {
        #region Synchronous
        /// <summary>
        /// Gets the Mongo collection (to perform advanced operations).
        /// </summary>
        /// <remarks>
        /// One can argue that exposing this property (and with that, access to it's Database property for instance
        /// (which is a "parent")) is not the responsibility of this class. Use of this property is highly discouraged;
        /// for most purposes you can use the MongoRepositoryManager&lt;T&gt;
        /// </remarks>
        /// <value>The Mongo collection (to perform advanced operations).</value>
        IMongoCollection<T> Collection { get; }

        /// <summary>
        /// Returns the T by its given id.
        /// </summary>
        /// <param name="id">The value representing the ObjectId of the entity to retrieve.</param>
        /// <returns>The Entity T.</returns>
        T Get(TKey id);

        /// <summary>
        /// Returns the T by expression.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>The expression.</returns>
        T Get(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// Returns the list of T by expression.
        /// </summary>
        /// <param name="predicate">The conditionals.</param>
        /// <param name="preFilters">pre filter Queryable</param>
        /// <param name="postFilters">post filter Queryable</param>
        /// <returns>List of object.</returns>
        List<T> Gets(Expression<Func<T, bool>> predicate = null,
            Func<IMongoQueryable<T>, IMongoQueryable<T>> preFilters = null,
            params Func<IMongoQueryable<T>, IMongoQueryable<T>>[] postFilters);

        /// <summary>
        /// Returns the T by expression
        /// </summary>
        /// <param name="projector">Result projector after select</param>
        /// <param name="predicate">The condition expression</param>
        /// <param name="preFilters">The pre filter Queryable</param>
        /// <param name="postFilters">The post filter Queryable</param>
        /// <returns></returns>
        List<TOutput> GetsAs<TOutput>(Expression<Func<T, TOutput>> projector,
            Expression<Func<T, bool>> predicate = null,
            Func<IMongoQueryable<T>, IMongoQueryable<T>> preFilters = null,
            params Func<IMongoQueryable<T>, IMongoQueryable<T>>[] postFilters);

        /// <summary>
        /// Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity including its new ObjectId.</returns>
        T Add(T entity);

        /// <summary>
        /// Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities of type T.</param>
        long Add(IEnumerable<T> entities);

        /// <summary>
        /// Upserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="options">Mongodb Update options</param>
        /// <returns>Number of document updated.</returns>
        long Update(T entity, UpdateOptions options = null);

        /// <summary>
        /// Upserts the entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <param name="options">Mongodb Update options</param>
        long UpdateMany(IEnumerable<T> entities, UpdateOptions options = null);

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        long Delete(string id);

        /// <summary>
        /// Deletes the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        long Delete(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// Deletes all entities in the repository.
        /// </summary>
        long DeleteAll();

        /// <summary>
        /// Counts the total entities matching predicate.
        /// </summary>
        /// <returns>Count of entities in the respository.</returns>
        long Count(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// Checks if the entity exists for given predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>True when an entity matching the predicate exists, false otherwise.</returns>
        bool Exists(Expression<Func<T, bool>> predicate = null);

        IMongoQueryable<T> Raw { get; }

        #endregion
        #region Asynchronous                
        /// <summary>
        /// Returns the T by its given id.
        /// </summary>
        /// <param name="id">The value representing the ObjectId of the entity to retrieve.</param>
        /// <returns>The Entity T.</returns>
        Task<T> GetAsync(string id);

        /// <summary>
        /// Returns the T by expression
        /// </summary>
        /// <param name="predicate">The expression</param>
        /// <returns></returns>
        Task<T> GetAsync(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// Returns the T by expression
        /// </summary>
        /// <param name="predicate">The expression</param>
        /// <param name="preFilters"></param>
        /// <returns></returns>
        Task<List<T>> GetsAsync(Expression<Func<T, bool>> predicate = null,
            Func<IMongoQueryable<T>, IMongoQueryable<T>> preFilters = null,
            params Func<IMongoQueryable<T>, IMongoQueryable<T>>[] postFilters);

        /// <summary>
        /// Returns the T by expression
        /// </summary>
        /// <param name="projector">Result projector after select</param>
        /// <param name="predicate">The condition expression</param>
        /// <param name="preFilters">The pre filter Queryable</param>
        /// <param name="postFilters">The post filter Queryable</param>
        /// <returns></returns>
        Task<List<TOutput>> GetsAsAsync<TOutput>(Expression<Func<T, TOutput>> projector,
            Expression<Func<T, bool>> predicate = null,
            Func<IMongoQueryable<T>, IMongoQueryable<T>> preFilters = null,
            params Func<IMongoQueryable<T>, IMongoQueryable<T>>[] postFilters);

        /// <summary>
        /// Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity including its new ObjectId.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        Task<List<T>> AddManyAsync(IEnumerable<T> entities);

        /// <summary>
        /// Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        Task<long> AddManyAsync(List<T> entities);

        /// <summary>
        /// Update entity async
        /// </summary>        
        /// <param name="entity">The entity need to update</param>
        /// <param name="options">Mongodb Update options</param>
        Task<long> UpdateAsync(T entity, UpdateOptions options = null);

        /// <summary>
        /// Update entity async with specific properties
        /// </summary>        
        /// <param name="predicate">Filter expression</param>
        /// <param name="updateDefinition">Mongodb update definition</param>
        /// <param name="options"></param>        
        Task<long> UpdateAsync(Expression<Func<T, bool>> predicate, UpdateDefinition<T> updateDefinition, UpdateOptions options = null);

        /// <summary>
        /// Update multiple async with filter definition and update definition
        /// </summary>
        /// <param name="filterDefinition">Mongodb filter definition</param>
        /// <param name="updateDefinition">Mongodb update definition</param>
        /// <param name="options">Mongodb Update options</param>
        /// <returns></returns>
        Task<long> UpdateAsync(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition, UpdateOptions options = null);

        /// <summary>
        /// Update Async by entity
        /// </summary>
        /// <param name="predicate">Filter expression</param>
        /// <param name="entity">Entity modified</param>
        /// <param name="options">Mongodb update options</param>
        /// <returns></returns>
        Task<long> UpdateAsync(Expression<Func<T, bool>> predicate, T entity, UpdateOptions options = null);

        /// <summary>
        /// Upserts the entities.
        /// </summary>        
        /// <param name="predicate">Mongodb filter definition</param>
        /// <param name="updateDefinition">Mongodb update definition</param>
        /// <param name="options">Mongodb Update options</param>
        Task<long> UpdateManyAsync(Expression<Func<T, bool>> predicate, UpdateDefinition<T> updateDefinition, UpdateOptions options = null);

        /// <summary>
        /// Upserts the entities.
        /// </summary>        
        /// <param name="filterDefinition">Mongodb filter definition</param>
        /// <param name="updateDefinition">Mongodb update definition</param>
        /// <param name="options">Mongodb Update options</param>
        Task<long> UpdateManyAsync(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition, UpdateOptions options = null);

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        Task<long> DeleteAsync(TKey id);

        /// <summary>
        /// Deletes the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        Task<long> DeleteAsync(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// Deletes the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        Task<long> DeleteManyAsync(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// Counts the total entities in the repository.
        /// </summary>
        /// <returns>Count of entities in the repository.</returns>
        Task<long> CountAysnc(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// Checks if the entity exists for given predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>True when an entity matching the predicate exists, false otherwise.</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate = null);
        #endregion
    }

    public interface IRepository<T, in TKey> : IMongoQueryable<T>, IMongoDbRepository<T, TKey>
        where T : IEntity<TKey>
    { }
}
