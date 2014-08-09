using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace DirectoryConsole
{
    public class DirectoryDb : DbContext
    {
        public DbSet<Entry> Entries { get; set; }
    }

    [DebuggerDisplay(@"\{{GetType().Name}: {Name}\}")]
    public abstract class Entry
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        public virtual Folder Parent { get; set; }
    }

    public class Folder : Entry
    {
        public virtual ICollection<Entry> Subentries { get; set; }
    }

    public class File : Entry
    {
    }

    public class DirectoryDbInitializer : DropCreateDatabaseIfModelChanges<DirectoryDb>
    {
        protected override void Seed(DirectoryDb db)
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

            db.Entries.Add(root);

            db.SaveChanges();
        }
    }
}
