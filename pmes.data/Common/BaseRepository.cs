using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using pmes.entity.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace pmes.data.Common
{
    public class BaseRepository<T>(PmesContext context) : IBaseRepository<T> where T : class
    {
        public string[] Searchable = [];
        private readonly PmesContext _context = context;
        private readonly DbSet<T> _entities = context.Set<T>();

        public virtual async Task<(T, bool)> CreateAsync(T entity)
        {
            _context.Add(entity);

            var result = await _context.SaveChangesAsync();

            return result > 0 ? (entity, true) : (null, false);
        }

        public virtual async Task<IEnumerable<T>> CreateAsync(IEnumerable<T> entity)
        {
            _context.AddRange(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _entities.FindAsync(id);

            if (entity == null)
                return false;

            _context.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<(IEnumerable<T>, int)> GetAllAsync(
            IEnumerable<FilterUtility.FilterParams> FilterParam,
            IEnumerable<SortingUtility.SortingParams> SortingParams,
            string? quickSearch = null,
            IEnumerable<string>? GroupingColumns = null,
            int pageNumber = 1,
            int pageSize = 25,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null
            )
        {
            IQueryable<T> query = _entities.AsQueryable();

            #region [QuickSearch]
            if (!string.IsNullOrEmpty(quickSearch))
            {
                query = QuickSearchUtility.Search<T>.SearchData(query, Searchable, quickSearch);
            }
            #endregion

            #region [Filter]  
            if (FilterParam != null)
            {
                query = FilterUtility.Filter<T>.FilteredData(FilterParam, query);
            }
            #endregion

            #region [Custom Predicate]
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            #endregion

            #region [Sorting] 
            if (SortingParams.Count() <= 0)
            {
                SortingParams = new List<SortingUtility.SortingParams>()
                {
                    {
                        new SortingUtility.SortingParams()
                        {
                            ColumnName = "CreatedOn",
                            SortOrder = SortingUtility.SortOrders.Desc
                        }
                    }
                };
            }

            if (SortingParams != null && SortingParams.Count() > 0)
            {
                query = SortingUtility.Sorting<T>.SortData(query, SortingParams);
            }
            #endregion

            #region [Grouping]  
            if (GroupingColumns != null && GroupingColumns != null && GroupingColumns.Count() > 0)
            {
                query = SortingUtility.Sorting<T>.GroupingData(query, GroupingColumns);
            }
            #endregion

            #region [Includes]
            if (includes != null)
                query = includes(query);
            #endregion

            int totalRecords = query.Count();

            #region [Paging]  
            var paginatedRecords = await PaginationUtility<T>.CreateAsync(query, pageNumber, pageSize);
            #endregion

            return (paginatedRecords, totalRecords);
        }

        public virtual async Task<(T, bool)> UpdateAsync(T entity)
        {
            _context.Update(entity);
            var result = await _context.SaveChangesAsync();

            return result > 0 ? (entity, true) : (null, false);
        }

        public async Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, bool asSplitQuery = false)
        {
            var entity = _context.Set<T>().AsQueryable();

            if (includes != null)
                entity = includes(entity);

            if (asSplitQuery)
                entity = entity.AsSplitQuery();

            return await entity.FirstOrDefaultAsync(predicate);
        }

        public void ClearEFTracker()
        {

            _context.ChangeTracker.Clear();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, Func<IQueryable<T>, IOrderedQueryable<T>> order = null, int? take = null)
        {
            var entity = _context.Set<T>().AsQueryable().Where(predicate);

            if (includes != null)
                entity = includes(entity);

            if (take != null)
                entity = entity.Take((int)take);

            if (order != null)
                entity = order(entity);


            return await entity.ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
