using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static pmes.data.Common.SortingUtility;

namespace pmes.data.Common
{
    public class FilterUtility
    {
        public enum FilterOptions
        {
            Equals = 1,
            StartsWith,
            EndsWith,
            Contains,
            DoesNotContain,
            IsEmpty,
            IsNotEmpty,
            IsGreaterThan,
            IsGreaterThanOrEqualTo,
            IsLessThan,
            IsLessThanOrEqualTo,
            IsEqualTo,
            IsNotEqualTo,
            DateBetween
        }

        public class FilterParams
        {
            public string ColumnName { get; set; } = string.Empty;
            public string FilterValue { get; set; } = string.Empty;
            public FilterOptions FilterOption { get; set; } = FilterOptions.Contains;
        }

        public class Filter<T>
        {
            private static MethodInfo equalsMethod = typeof(string).GetMethod("Equals", new Type[] { typeof(string) });
            private static MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string), typeof(StringComparison) });
            private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
            private static MethodInfo isNullOrEmptyMethod = typeof(string).GetMethod("IsNullOrWhiteSpace", new Type[] { typeof(string) });
            public static IQueryable<T> FilteredData(IEnumerable<FilterParams> filterParams, IQueryable<T> data)
            {
                IEnumerable<string> distinctColumns = filterParams.Where(x => !string.IsNullOrEmpty(x.ColumnName)).Select(x => x.ColumnName).Distinct();

                foreach (string colName in distinctColumns)
                {
                    var filterColumn = typeof(T).GetProperty(colName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

                    IEnumerable<FilterParams> filterValues = filterParams.Where(x => x.ColumnName.Equals(colName)).Distinct();

                    if (filterColumn != null)
                    {

                        if (filterValues.Count() > 1)
                        {
                            IEnumerable<T> sameColData = Enumerable.Empty<T>();

                            foreach (var val in filterValues)
                            {
                                sameColData = sameColData.Concat(FilterData(val.FilterOption, data, filterColumn, val.FilterValue));
                            }

                            data = data.Intersect(sameColData);
                        }
                        else
                        {
                            data = FilterData(filterValues.FirstOrDefault().FilterOption, data, filterColumn, filterValues.FirstOrDefault().FilterValue);
                        }
                    }

                    if (colName.Contains('.'))
                    {
                        data = data.Where($"Convert.ToString({colName}).ToLower().Equals(@0)", filterValues.FirstOrDefault().FilterValue);
                    }

                    if (colName.Contains('+'))
                    {
                        string predicate = "";
                        string[] columns = colName.Split("+");

                        foreach (var item in columns)
                        {
                            if (columns.Last().Equals(item))
                            {
                                predicate += $"Convert.ToString({item}).ToLower()";
                            }
                            else
                            {
                                predicate += $"Convert.ToString({item}).ToLower() + \" \" +";
                            }
                        }

                        data = data.Where($"({predicate}).Contains(@0)", filterValues.FirstOrDefault().FilterValue);
                    }
                }
                return data;
            }

            private static IQueryable<T> FilterData(FilterOptions filterOption, IQueryable<T> data, PropertyInfo filterColumn, string filterValue)
            {
                Type type = typeof(T);
                ParameterExpression parameter = Expression.Parameter(type);
                MemberExpression memberAccess = Expression.MakeMemberAccess(parameter, type.GetProperty(filterColumn.Name));
                MethodCallExpression? methodCallExpression = null;
                Expression<Func<T, bool>> lambda = null;

                int outValue;
                DateTime dateValue;

                if (memberAccess.Type == typeof(string))
                {
                    switch (filterOption)
                    {
                        #region [StringDataType]  
                        case FilterOptions.Equals:
                            methodCallExpression = Expression.Call(memberAccess, equalsMethod, Expression.Constant(filterValue));
                            lambda = Expression.Lambda<Func<T, bool>>(methodCallExpression, parameter); break;
                        case FilterOptions.StartsWith:
                            methodCallExpression = Expression.Call(memberAccess, startsWithMethod, Expression.Constant(filterValue));
                            lambda = Expression.Lambda<Func<T, bool>>(methodCallExpression, parameter); break;
                        case FilterOptions.EndsWith:
                            methodCallExpression = Expression.Call(memberAccess, endsWithMethod, Expression.Constant(filterValue));
                            lambda = Expression.Lambda<Func<T, bool>>(methodCallExpression, parameter); break;
                        case FilterOptions.Contains:
                            methodCallExpression = Expression.Call(memberAccess, containsMethod, Expression.Constant(filterValue));
                            lambda = Expression.Lambda<Func<T, bool>>(methodCallExpression, parameter);
                            break;
                        case FilterOptions.DoesNotContain:
                            methodCallExpression = Expression.Call(memberAccess, containsMethod, Expression.Constant(filterValue));
                            lambda = Expression.Lambda<Func<T, bool>>(Expression.Not(methodCallExpression), parameter);
                            break;
                        case FilterOptions.IsEmpty:
                            methodCallExpression = Expression.Call(typeof(string), nameof(string.IsNullOrWhiteSpace), null, Expression.PropertyOrField(parameter, filterColumn.Name));
                            lambda = Expression.Lambda<Func<T, bool>>(methodCallExpression, parameter);
                            break;
                        case FilterOptions.IsNotEmpty:
                            methodCallExpression = Expression.Call(typeof(string), nameof(string.IsNullOrWhiteSpace), null, Expression.PropertyOrField(parameter, filterColumn.Name));
                            lambda = Expression.Lambda<Func<T, bool>>(Expression.Not(methodCallExpression), parameter);
                            break;
                            #endregion

                            #region [Custom]  
                            //case FilterOptions.IsGreaterThan:
                            //    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                            //    {
                            //        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) > outValue);
                            //    }
                            //    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                            //    {
                            //        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) > dateValue);

                            //    }
                            //    break;

                            //case FilterOptions.IsGreaterThanOrEqualTo:
                            //    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                            //    {
                            //        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) >= outValue);
                            //    }
                            //    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                            //    {
                            //        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) >= dateValue);
                            //        break;
                            //    }
                            //    break;

                            //case FilterOptions.IsLessThan:
                            //    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                            //    {
                            //        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) < outValue);
                            //    }
                            //    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                            //    {
                            //        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) < dateValue);
                            //        break;
                            //    }
                            //    break;

                            //case FilterOptions.IsLessThanOrEqualTo:
                            //    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                            //    {
                            //        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) <= outValue);
                            //    }
                            //    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                            //    {
                            //        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) <= dateValue);
                            //        break;
                            //    }
                            //    break;

                            //case FilterOptions.IsEqualTo:
                            //    if (filterValue == string.Empty)
                            //    {
                            //        data = data.Where(x => filterColumn.GetValue(x, null) == null
                            //                        || (filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower() == string.Empty));
                            //    }
                            //    else
                            //    {
                            //        if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                            //        {
                            //            data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) == outValue);
                            //        }
                            //        else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                            //        {
                            //            data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) == dateValue);
                            //            break;
                            //        }
                            //        else
                            //        {
                            //            data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower() == filterValue.ToLower());
                            //        }
                            //    }
                            //    break;

                            //case FilterOptions.IsNotEqualTo:
                            //    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                            //    {
                            //        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) != outValue);
                            //    }
                            //    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                            //    {
                            //        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) != dateValue);
                            //        break;
                            //    }
                            //    else
                            //    {
                            //        data = data.Where(x => filterColumn.GetValue(x, null) == null ||
                            //                         (filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower() != filterValue.ToLower()));
                            //    }
                            //    break;
                            //case FilterOptions.DateBetween:
                            //        string[] dates = filterValue.Split("|");
                            //        data = data.Where(x => (Convert.ToDateTime(filterColumn.GetValue(x, null)) >= DateTime.Parse(dates[0]) 
                            //                            && Convert.ToDateTime(filterColumn.GetValue(x, null)) <= DateTime.Parse(dates[1])));
                            //        break;
                            #endregion
                    }

                }

                if (memberAccess.Type == typeof(DateOnly) || memberAccess.Type == typeof(DateOnly?))
                {
                    if (filterValue.Contains('|'))
                    {
                        string[] dates = filterValue.Split("|");

                        var fromDate = Expression.GreaterThanOrEqual(memberAccess, Expression.Constant(DateOnly.Parse(dates[0]), typeof(DateOnly)));

                        var toDate = Expression.LessThanOrEqual(memberAccess, Expression.Constant(DateOnly.Parse(dates[1]), typeof(DateOnly)));

                        var filterExpression = Expression.And(fromDate, toDate);

                        lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
                    }
                    else
                    {
                        var filterExpression = Expression.Equal(memberAccess, Expression.Constant(DateOnly.Parse(filterValue), memberAccess.Type));
                        lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
                    }

                }

                if (memberAccess.Type == typeof(DateTime) || memberAccess.Type == typeof(DateTime?))
                {
                    if (filterValue.Contains('|'))
                    {
                        string[] dates = filterValue.Split("|");

                        var fromDate = Expression.GreaterThanOrEqual(memberAccess, Expression.Constant(DateTime.Parse(dates[0]), typeof(DateTime)));

                        var toDate = Expression.LessThanOrEqual(memberAccess, Expression.Constant(DateTime.Parse(dates[1]), typeof(DateTime)));

                        var filterExpression = Expression.And(fromDate, toDate);

                        lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
                    }
                    else
                    {
                        var filterExpression = Expression.Equal(memberAccess, Expression.Constant(DateTime.Parse(filterValue), memberAccess.Type));
                        lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
                    }
                }

                if (memberAccess.Type == typeof(int) || memberAccess.Type == typeof(int?))
                {
                    string[] zeroOrNullValues = new string[] { "null", "0" };
                    switch (filterOption)
                    {
                        case FilterOptions.Contains:
                        case FilterOptions.Equals:
                        default:
                            var filterExpression = Expression.Equal(memberAccess, Expression.Constant(zeroOrNullValues.Contains(filterValue) ? null : int.Parse(filterValue), memberAccess.Type));
                            lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
                            break;
                    }
                }

                if (memberAccess.Type == typeof(decimal) || memberAccess.Type == typeof(decimal?))
                {
                    string[] zeroOrNullValues = new string[] { "null", "0" };
                    var filterExpression = Expression.Equal(memberAccess, Expression.Constant(zeroOrNullValues.Contains(filterValue) ? null : decimal.Parse(filterValue), memberAccess.Type));
                    lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
                }

                if (memberAccess.Type == typeof(bool) || memberAccess.Type == typeof(bool?))
                {
                    var filterExpression = Expression.Equal(memberAccess, Expression.Constant(bool.Parse(filterValue), memberAccess.Type));
                    lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
                }

                data = data.Where(x => filterColumn != null).Where(lambda);

                return data;
            }
        }
    }
}
