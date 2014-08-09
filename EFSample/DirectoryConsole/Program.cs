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
            Database.SetInitializer(new DirectoryDbInitializer());

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
    }
}
