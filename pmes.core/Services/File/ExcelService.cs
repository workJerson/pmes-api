using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Services.File
{
    public interface IExcelService
    {
        Task<byte[]> GenerateExcel<T>(List<T> data, string sheetName = "DEFAULT");
    }
    public class ExcelService : IExcelService
    {
        public async Task<byte[]> GenerateExcel<T>(List<T> data, string sheetName = "DEFAULT")
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using ExcelPackage pack = new();
            var workSheet = pack.Workbook.Worksheets.Add(sheetName);

            //Fill data            
            workSheet.Cells["A1"].LoadFromCollection(data, true, OfficeOpenXml.Table.TableStyles.Medium1);
            return await pack.GetAsByteArrayAsync();
        }
    }
}
