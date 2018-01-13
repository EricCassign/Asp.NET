using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryData
{
    public class LibraryContext: DbContext
    {
    public LibraryContext(DbContextOptions options):base(options) { }
    public DbSet<Patron> Patrons { get; set; }
    }
}
