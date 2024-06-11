using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Helpers
{
    internal static class DateHelper
    {
        public static DateOnly? ConvertStringToDate(string value)
        {
            return DateOnly.TryParse(value, out var date) ? date : null;
        }
        public static string ConvertDateToString(DateOnly? value)
        {
            if (value == null) return null;
            return value.Value.ToString("MM-dd-yyyy");
        }
    }
}
