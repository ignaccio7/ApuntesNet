using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // EF crea SQL automáticamente a partir de tus 
    // null! esto significa que el compilador que se inicializara en runtime
    public DbSet<User> Users { get; set; } = null!;
}
