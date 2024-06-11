using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Services.File
{
    public interface ICSVService
    {
        public byte[] GenerateCsv<T>(List<T> data);
        public IEnumerable<T> ReadCSV<T>(CsvReader reader);
        public (bool, CsvReader? reader) ValidateCSVHeaders(Stream file, string[] headers);

    }
    public class CSVService : ICSVService
    {
        public string[] Headers;
        public byte[] GenerateCsv<T>(List<T> data)
        {
            string fileName = string.Concat(Directory.GetCurrentDirectory(), @"AccessManagement_UserExportListing.csv");
            if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);

            MemoryStream ms = new();
            using (StreamWriter sw = new(ms, Encoding.UTF8))
            {
                CsvWriter cw = new(sw, CultureInfo.CurrentCulture);

                cw.WriteHeader<T>();
                cw.NextRecord();

                foreach (var p in data)
                {
                    cw.WriteRecord(p);
                    cw.NextRecord();
                }

                sw.Flush();
            }

            ms.Close();
            return ms.ToArray();
        }
        public IEnumerable<T> ReadCSV<T>(CsvReader reader)
        {
            return reader?.GetRecords<T>();
        }

        public (bool, CsvReader? reader) ValidateCSVHeaders(Stream file, string[] headers)
        {
            Headers = headers;
            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();

            bool isValidHeader = csv.HeaderRecord.SequenceEqual(Headers);

            return isValidHeader ? (isValidHeader, csv) : (false, null);
        }
    }
}
