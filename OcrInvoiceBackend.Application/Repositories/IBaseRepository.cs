using OcrInvoiceBackend.Application.Common.Models;
using OcrInvoiceBackend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        void Create(T entity);
        void CreateRange(IEnumerable<T> entities);

        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);

        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

        Task<T?> Get(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties);
        Task<List<T>> GetAll(CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties);
        Task<List<T>> GetWhere(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties);
        Task<(List<T>, PaginationResponse)> GetFilteredAndOrderedPage(
        PaginationQuery pagination,
        Dictionary<Expression<Func<T, bool>>, bool> filterPredicates,
        Dictionary<string, Expression<Func<T, object>>> orderPredicates,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] includeProperties);
    }
}
