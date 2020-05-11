using Collateral.SupplierPortal.Apis.Core.Helpers;
using Collateral.SupplierPortal.Apis.Core.Interfaces.Gateways.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Collateral.SupplierPortal.Apis.Infrastructure.Data.EntityFramework
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbContext _ctx;
        private readonly DbSet<T> Entity;
        public bool MultipleWorkThreads { get; private set; }

        public BaseRepository(DbContext context, DbSet<T> entity, bool multipleWorkThreads = false)
        {
            if (context == null || entity == null)
                return;

            MultipleWorkThreads = multipleWorkThreads;
            _ctx = context;
            Entity = entity;
        }

        public virtual async Task<bool> Add(T entity)
        {
            _ = await Entity.AddAsync(entity).ConfigureAwait(false);
            return true;
        }

        /// <summary>
        /// Finds by predicate.
        /// http://appetere.com/post/passing-include-statements-into-a-repository
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        public virtual async Task<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return await Entity.FirstOrDefaultAsync(predicate).ConfigureAwait(false);
        }

        public virtual async Task<bool> Update(T entity)
        {
            if (entity == null)
                return false;

            if (MultipleWorkThreads)
            {
                var type = entity.GetType();

                var et = _ctx.Model.FindEntityType(type);
                if (et == null)
                {
                    type = type.BaseType;
                    et = _ctx.Model.FindEntityType(type);
                }

                var key = et.FindPrimaryKey();

                var keys = new object[key.Properties.Count];
                var x = 0;
                foreach (var keyName in key.Properties)
                {
                    var keyProperty = type.GetProperty(keyName.Name, BindingFlags.Public | BindingFlags.Instance);
                    keys[x++] = keyProperty.GetValue(entity);
                }

                var originalEntity = _ctx.Find(type, keys);
                _ctx.Entry(originalEntity).CurrentValues.SetValues(entity);
                _ = _ctx.Update(originalEntity);
            }
            else
            {
                Entity.Attach(entity);
                _ctx.Entry(entity).State = EntityState.Modified;
            }
            return await Task.FromResult(true).ConfigureAwait(false);
        }

        public virtual async Task<bool> Delete(Expression<Func<T, bool>> identity)
        {
            var results = Entity.Where(identity);

            Entity.RemoveRange(results);
            return await Task.FromResult(true).ConfigureAwait(false);
        }

        public virtual async Task<bool> Delete(T entity)
        {
            Entity.Remove(entity);
            return await Task.FromResult(true).ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await Entity.ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            var result = Entity.Where(predicate);
            return await result.ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<PagedResult<T>> GetAll(int page, int pageSize, Expression<Func<T, bool>> predicate)
        {
            var result = Entity.Where(predicate);
            return await result.GetPaged(page, pageSize).ConfigureAwait(false);
        }

        public virtual async Task<PagedResult<T>> GetAll(int page, int pageSize)
        {
            return await Entity.GetPaged(page, pageSize).ConfigureAwait(false);
        }

        public async Task<bool> Save()
        {
            _ = await _ctx.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public IQueryable<T> UsingSelect()
        {
            return Entity;
        }

        public void ChangeMultipleWorkThreads(bool isBatch)
        {
            MultipleWorkThreads = isBatch;
        }
    }
}