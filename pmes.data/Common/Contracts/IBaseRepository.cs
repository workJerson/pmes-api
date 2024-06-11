using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace pmes.data.Common.Contracts
{
    public interface IBaseRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, Func<IQueryable<T>, IOrderedQueryable<T>> order = null, int? take = null);
        Task<IEnumerable<T>> GetAllAsync();
        Task<(IEnumerable<T>, int)> GetAllAsync(IEnumerable<FilterUtility.FilterParams> FilterParam, IEnumerable<SortingUtility.SortingParams> SortingParams, string? quickSearch = null, IEnumerable<string>? GroupingColumns = null, int pageNumber = 1, int pageSize = 25, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null);
        Task<(T, bool)> CreateAsync(T entity);
        Task<IEnumerable<T>> CreateAsync(IEnumerable<T> entity);
        Task<(T, bool)> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, bool asSplitQuery = false);
        void ClearEFTracker();
        Task<int> SaveChangesAsync();
    }
}
