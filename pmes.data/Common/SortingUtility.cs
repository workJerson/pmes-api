using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace pmes.data.Common
{
    public class SortingUtility
    {
        public enum SortOrders
        {
            Asc = 1,
            Desc = 2
        }
        public class SortingParams
        {
            public SortOrders SortOrder { get; set; } = SortOrders.Asc;
            public string ColumnName { get; set; } = "CreatedOn";
        }
        public class Sorting<T>
        {
            // TODO: Change to lambda expressions.
            public static IQueryable<T> GroupingData(IQueryable<T> data, IEnumerable<string> groupingColumns)
            {
                IOrderedQueryable<T> groupedData = null;

                foreach (string grpCol in groupingColumns.Where(x => !string.IsNullOrEmpty(x)))
                {
                    var col = typeof(T).GetProperty(grpCol, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                    if (col != null)
                    {
                        groupedData = groupedData == null ? data.OrderBy(x => col.GetValue(x, null))
                                                        : groupedData.ThenBy(x => col.GetValue(x, null));
                    }
                }

                return groupedData ?? data;
            }
            public static IQueryable<T> SortData(IQueryable<T> data, IEnumerable<SortingParams> sortingParams)
            {
                IOrderedQueryable<T> sortedData = null;
                foreach (var sortingParam in sortingParams.Where(x => !string.IsNullOrEmpty(x.ColumnName)))
                {
                    var col = typeof(T).GetProperty(sortingParam.ColumnName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                    if (col != null)
                    {
                        sortedData = sortedData == null ? sortingParam.SortOrder == SortOrders.Asc ? OrderingHelper(data, sortingParam.ColumnName, false, false) // OrderBy
                                                                                                   : OrderingHelper(data, sortingParam.ColumnName, true, false) // OrderByDescending
                                                        : sortingParam.SortOrder == SortOrders.Asc ? OrderingHelper(sortedData, sortingParam.ColumnName, false, true) // ThenBy
                                                                                            : OrderingHelper(sortedData, sortingParam.ColumnName, true, true); //ThenByDescending
                    }

                    if (sortingParam.ColumnName.Contains('.'))
                    {
                        sortedData = sortedData == null ? sortingParam.SortOrder == SortOrders.Asc ? data.OrderBy(sortingParam.ColumnName)
                                                                        : data.OrderBy($"{sortingParam.ColumnName} desc")
                                                        : sortingParam.SortOrder == SortOrders.Asc ? sortedData.ThenBy(sortingParam.ColumnName)
                                                                        : sortedData.ThenBy($"{sortingParam.ColumnName} desc");
                    }

                    if (sortingParam.ColumnName.Contains('|'))
                    {
                        string[] columns = sortingParam.ColumnName.Split("|");
                        foreach (var column in columns)
                        {
                            sortedData = sortedData == null ? sortingParam.SortOrder == SortOrders.Asc ? data.OrderBy(column)
                                                                            : data.OrderBy($"{column} desc")
                                                            : sortingParam.SortOrder == SortOrders.Asc ? sortedData.ThenBy(column)
                                                                            : sortedData.ThenBy($"{column} desc");
                        }
                    }
                }
                return sortedData ?? data;
            }
            private static IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
            {
                ParameterExpression param = Expression.Parameter(typeof(T), string.Empty); // I don't care about some naming
                MemberExpression property = Expression.PropertyOrField(param, propertyName);
                LambdaExpression sort = Expression.Lambda(property, param);

                MethodCallExpression call = Expression.Call(
                    typeof(Queryable),
                    (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
                    new[] { typeof(T), property.Type },
                    source.Expression,
                    Expression.Quote(sort));

                return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
            }
        }
    }
}

