using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Services.File.Validators
{
    public abstract class ExcelSheetValidator
    {
        protected string[] Headers { get; set; }
        public abstract Task<(bool, string[])> Validate(ExcelWorksheet excelWorksheet);
    }
}
