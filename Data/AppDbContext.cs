using BackEndMimimal.Models;
using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    =>optionsBuilder.UseSqlite("DataSource=app.db;Cache=Shared");   
}