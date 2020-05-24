using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Focus.Application.Common.Repository;
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
    public class MongoRepository<TEntity, TSource> : IRepository<TEntity>, IAsyncRepository<TEntity> where TEntity : IIdentifiable
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
        protected readonly string _entityName;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IMongoCollection<TEntity> Entities => _database.GetCollection<TEntity>(_entityName);

        /// <summary>
        /// Establishes connection to the MongoDB database
        /// </summary>
        /// <param name="configuration">Configuration for MongoDB database</param>
        public MongoRepository(IMongoConfiguration configuration)
        {
            _configuration = configuration;
            var client = new MongoClient(_configuration.ConnectionString);
            _database = client.GetDatabase(_configuration.Database);
            _entityName = typeof(TEntity).Name;
        }

        /// <summary>
        /// Inserts one entity into collection based on the name of the entity
        /// If inserted entity has non-empty id than it will be forcefully set to empty
        /// </summary>
        /// <param name="entity">Inserted entity with empty id</param>
        public void Add(TEntity entity)
        {
            if (!string.IsNullOrEmpty(entity.Id))
                entity.Id = ObjectId.GenerateNewId().ToString();

            Entities.InsertOne(entity);
        }

        /// <summary>
        /// Asynchronously inserts one entity into collection based on the name of the entity
        /// If inserted entity has non-empty id than it will be forcefully set to empty
        /// </summary>
        /// <param name="entity">Inserted entity with empty id</param>
        public Task AddAsync(TEntity entity)
        {
            if (!string.IsNullOrEmpty(entity.Id))
                entity.Id = ObjectId.GenerateNewId().ToString();

            return Entities.InsertOneAsync(entity);
        }

        /// <summary>
        /// Inserts range of entities into the database. Each entity is validated on empty id value
        /// </summary>
        /// <param name="entities">Array of entities to be inserted with empty id value</param>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (!string.IsNullOrEmpty(entity.Id))
                    entity.Id = ObjectId.GenerateNewId().ToString(); ;
            }

            Entities.InsertMany(entities);
        }

        /// <summary>
        /// Asynchronously inserts range of entities into the database. Each entity is validated on empty id value 
        /// </summary>
        /// <param name="entities">Array of entities to be inserted with empty id value</param>
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            var idValidationTasks = new List<Task>();

            foreach (var entity in entities)
                idValidationTasks.Add(Task.Run(() =>
                {
                    if (!string.IsNullOrEmpty(entity.Id))
                        entity.Id = ObjectId.GenerateNewId().ToString();
                }));

            await Task.WhenAll(idValidationTasks);

            await Entities.InsertManyAsync(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TEntity entity)
        {
            Entities.FindOneAndDelete(e => e.Id == entity.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task DeleteAsync(TEntity entity)
        {
            return Entities.FindOneAndDeleteAsync(e => e.Id == entity.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        public void DeleteBy(Expression<Func<TEntity, bool>> filter)
        {
            Entities.DeleteMany(filter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task DeleteByAsync(Expression<Func<TEntity, bool>> filter)
        {
            return Entities.DeleteManyAsync(filter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteRange(IEnumerable<string> ids)
        {
            Entities.DeleteMany(
                Builders<TEntity>.Filter.In(e => e.Id, ids));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task DeleteRangeAsync(IEnumerable<string> ids)
        {
            return Entities.DeleteManyAsync(
                Builders<TEntity>.Filter.In(e => e.Id, ids));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter)
        {
            return Entities
                .Find(filter)
                .ToEnumerable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Entities
                .Find(filter)
                .ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity Get(string id)
        {
            return Entities
                .Find(e => e.Id == id)
                .FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll()
        {
            return Entities
                .Find(_ => true)
                .ToEnumerable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Entities
                .Find(_ => true)
                .ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<TEntity> GetAsync(string id)
        {
            return Entities
                .Find(e => e.Id == id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            Entities.FindOneAndReplace(
                Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id),
                entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task UpdateAsync(TEntity entity)
        {
            return Entities.FindOneAndReplaceAsync(
                Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id),
                entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                Entities.FindOneAndReplace(
                    Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id),
                    entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            var updateTasks = new List<Task>();

            foreach (var entity in entities)
                updateTasks.Add(
                    Entities.FindOneAndReplaceAsync(
                        Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id),
                        entity));

            return Task.WhenAll(updateTasks);
        }
    }
}