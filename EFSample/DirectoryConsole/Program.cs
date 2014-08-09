using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DirectoryConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //InsertData();
            GetData();
        }

        static void GetData()
        {
            using (var db = new DirectoryDb())
            {
                var files = db.Entries
                    .OfType<File>()
                    .Include(f => f.Parent)
                    .ToArray();

                var dir1 = db.Entries
                    .OfType<Folder>()
                    .Where(f => f.Name == "Dir 1")
                    .Include(f => f.Subentries)
                    .Single();

                foreach (var item in dir1.Subentries)
                {
                    Console.WriteLine(item.Name);
                }
            }
        }

        static void InsertData()
        {
            var root = new Folder
            {
                Name = "Root",
                Subentries = new HashSet<Entry>
                {
                    new Folder
                    {
                        Name = "Dir 1",
                        Subentries = new HashSet<Entry>
                        {
                            new File { Name = "File 1-1" },
                            new File { Name = "File 1-2" },
                        },
                    },
                    new Folder { Name = "Dir 2" },
                    new File { Name = "File 1" },
                },
            };

            using (var db = new DirectoryDb())
            {
                db.Entries.Add(root);

                db.SaveChanges();
            }
        }
    }
}
