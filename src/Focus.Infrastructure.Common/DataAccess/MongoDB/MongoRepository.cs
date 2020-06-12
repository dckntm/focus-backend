using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Focus.Application.Common.DataAccess;
using Focus.Core.Common;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Focus.Infrastructure.Common.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TSource"></typeparam>
    public class MongoRepository<TEntity, TId> : IAsyncRepository<TEntity, TId> where TEntity : IIdentifiable<TId>
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly IMongoConfiguration _configuration;

        /// <summary>
        /// 
        /// </summary>
        protected readonly IMongoDatabase _database;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IMongoCollection<TEntity> Collection { get; }

        /// <summary>
        /// Establishes connection to the MongoDB database
        /// </summary>
        /// <param name="configuration">Configuration for MongoDB database</param>
        public MongoRepository(IMongoConfiguration configuration)
        {
            _configuration = configuration;
            var client = new MongoClient(_configuration.ConnectionString);
            _database = client.GetDatabase(_configuration.Database);

            Collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        /// <summary>
        /// Asynchronously inserts one entity into collection based on the name of the entity
        /// If inserted entity has non-empty id than it will be forcefully set to empty
        /// </summary>
        /// <param name="entity">Inserted entity with empty id</param>
        public async Task AddAsync(TEntity entity)
            => await Collection.InsertOneAsync(entity);

        /// <summary>
        /// Asynchronously searches for entity with specified Id
        /// </summary>
        /// <param name="id">Id seed for executing search</param>
        /// <returns>Entity if it was found successfully. Null if search failed</returns>
        public async Task<TEntity> GetAsync(TId id)
            => await Collection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();

        /// <summary>
        /// Returns all elements in collection and returns them as list. Not for using in large collections, better use and cache cursors
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await Collection.Find(_ => true).ToListAsync();

        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRangeAsync(IEnumerable<TId> ids)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}