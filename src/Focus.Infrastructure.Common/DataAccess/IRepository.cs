using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Focus.Core.Common;

namespace Focus.Infrastructure.Common.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : IIdentifiable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Get(string id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        void DeleteRange(IEnumerable<string> ids);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        void DeleteBy(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void UpdateRange(IEnumerable<TEntity> entity);
    }
}