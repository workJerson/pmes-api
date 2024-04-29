using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Common.Models.Request
{
    public class BaseFilteringViewModel
    {
        private Dictionary<string, string> DefaultAdvanceFilter = new Dictionary<string, string>() { { "isDeleted", "false" }, { "isActive", "true" } };
        public string? OrderBy { get; set; } = "CreatedOn";
        public bool IsDescending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int RowPerPage { get; set; } = 10;
        public string? Search { get; set; }
        public Dictionary<string, string>? AdvanceFilter
        {
            get => DefaultAdvanceFilter;
            set
            {
                foreach (var item in value)
                    DefaultAdvanceFilter.Add(item.Key, item.Value);
            }
        }
        public int TimezoneOffset { get; set; } = 8;
    }
    public class BaseExportFilteringViewModel : BaseFilteringViewModel
    {
        public string ExportType { get; set; } = "csv";
    }
}
