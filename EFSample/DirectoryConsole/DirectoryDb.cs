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
        public Folder Parent { get; set; }
    }

    public class Folder : Entry
    {
        public ICollection<Entry> Subentries { get; set; }
    }

    public class File : Entry
    {
    }
}
