using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Services.File.Dtos
{
    public class GetBulkUploadErrorMessageModel
    {
        public int RowNumber { get; set; }
        public string ErrorMessage { get; set; }
        public string Value { get; set; }
    }
}
