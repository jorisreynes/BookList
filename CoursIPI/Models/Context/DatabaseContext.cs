using CoursIPI.NewFolder;
using Microsoft.EntityFrameworkCore;

namespace CoursIPI.Models.Context;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<User> Users { get; set; }
}
