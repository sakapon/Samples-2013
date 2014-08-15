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
                // In case of Excel, the path separator is "/" not "\".
                var query = archive.Entries
                    .Where(e => e.FullName.StartsWith("xl/media/", StringComparison.InvariantCultureIgnoreCase));

                foreach (var entry in query)
                {
                    var filePath = Path.Combine(outputDirPath, entry.Name);
                    entry.ExtractToFile(filePath, true);
                }
            }
        }

        static void ExtractAndZip()
        {
            var targetFilePath = "Book1.xlsx";
            var extractDirPath = "Book1";
            var zipFilePath = "Book1.zip";

            if (Directory.Exists(extractDirPath))
            {
                Directory.Delete(extractDirPath, true);
            }
            ZipFile.ExtractToDirectory(targetFilePath, extractDirPath);

            File.Delete(zipFilePath);
            ZipFile.CreateFromDirectory(extractDirPath, zipFilePath);
        }
    }
}
