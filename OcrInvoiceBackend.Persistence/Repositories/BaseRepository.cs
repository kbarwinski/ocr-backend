using Microsoft.EntityFrameworkCore;
using OcrInvoiceBackend.Application.Common.Models;
using OcrInvoiceBackend.Application.Repositories;
using OcrInvoiceBackend.Domain.Common;
using OcrInvoiceBackend.Persistence.Context;
using OcrInvoiceBackend.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Persistence.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly DataContext Context;

        public BaseRepository(DataContext context)
        {
            Context = context;
        }

        public virtual void Create(T entity)
        {
            entity.DateCreated = DateTime.Now.ToUniversalTime();

            Context.Set<T>().Add(entity);
        }

        public virtual void CreateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                entity.DateCreated = DateTime.Now.ToUniversalTime();
            Context.Set<T>().AddRange(entities);
        }

        public virtual void Update(T entity)
        {
            entity.DateUpdated = DateTime.Now.ToUniversalTime();
            Context.Set<T>().Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                entity.DateUpdated = DateTime.Now.ToUniversalTime();

            Context.Set<T>().UpdateRange(entities);
        }

        public virtual void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
        }

        public virtual Task<T?> Get(Expression<Func<T, bool>> predicate,
                            CancellationToken cancellationToken,
                            params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual Task<List<T>> GetAll(CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.ToListAsync(cancellationToken);
        }

        public virtual Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return Context.Set<T>().CountAsync(cancellationToken);
        }

        public virtual async Task<(List<T>, PaginationResponse)> GetFilteredAndOrderedPage(
                PaginationQuery pagination,
                Dictionary<Expression<Func<T, bool>>, bool> filterPredicates,
                Dictionary<string, Expression<Func<T, object>>> orderPredicates,
                CancellationToken cancellationToken,
                params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> query = Context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            var filtered = query.Filter(filterPredicates);

            var totalSize = await filtered.CountAsync();

            var page = await filtered
                .Order(pagination.SortingOrders, orderPredicates)
                .Paginate(pagination.Page, pagination.PageSize)
                .ToListAsync();

            return (page, new PaginationResponse()
            {
                TotalCount = totalSize,
                Count = page.Count,
                Page = pagination.Page,
                HasNext = (pagination.Page + 1) * pagination.PageSize <= totalSize,
                HasPrevious = pagination.Page > 0
            });
        }

        public virtual Task<List<T>> GetWhere(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            query = query.Where(predicate);

            return query.ToListAsync(cancellationToken);
        }

        public Task<List<T>> GetOrdered(Expression<Func<T, bool>> predicate, string sortingOrders, Dictionary<string, Expression<Func<T, object>>> orderPredicates, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            query = query.Where(predicate);
            query = query.Order(sortingOrders, orderPredicates);

            return query.ToListAsync(cancellationToken);
        }
    }
}
