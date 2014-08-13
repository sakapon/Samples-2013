using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ExcelMediaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var excelFilePath = "Book1.xlsx";
            var outputDirPath = "Media";

            Directory.CreateDirectory(outputDirPath);

            using (var archive = ZipFile.OpenRead(excelFilePath))
            {
                var query = archive.Entries
                    .Where(e => e.FullName.StartsWith("xl/media/", StringComparison.InvariantCultureIgnoreCase));

                foreach (var entry in query)
                {
                    var filePath = Path.Combine(outputDirPath, entry.Name);
                    entry.ExtractToFile(filePath, true);
                }
            }
        }
    }
}
