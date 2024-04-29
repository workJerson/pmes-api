using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace pmes.data.Common
{
    public class QuickSearchUtility
    {
        public class Search<T>
        {

            //TODO: Add quick search functionality for related entities.
            public static IQueryable<T> SearchData(IQueryable<T> data, string[] columns, string? value)
            {
                string parsedQuery = "";

                foreach (var item in columns)
                {
                    if (columns.Last().Equals(item))
                    {
                        parsedQuery += item.Contains('+')
                            ? Predicate(item)
                            : $"Convert.ToString({item}).ToLower().Contains(@0)";
                    }
                    else
                    {
                        parsedQuery += item.Contains('+')
                            ? $"{Predicate(item)} or "
                            : $"Convert.ToString({item}).ToLower().Contains(@0) or ";
                    }
                }

                data = data.Where(parsedQuery, value);

                return data;
            }

            private static string Predicate(string column)
            {
                string predicate = "";

                if (column.Contains('+'))
                {
                    string[] columns = column.Split("+");

                    foreach (string item in columns)
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
                }

                return $"({predicate}).Contains(@0)";
            }
        }
    }
}
