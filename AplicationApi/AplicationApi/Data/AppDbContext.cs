using AplicationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AplicationApi.Data;

public class AppDbContext : DbContext
{
    public DbSet<Usuario> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> opts) : base (opts)
    {
    }
}
