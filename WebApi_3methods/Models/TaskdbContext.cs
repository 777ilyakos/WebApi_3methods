using Microsoft.EntityFrameworkCore;
namespace WebApi_3methods.Models
{
    public class TaskdbContext : DbContext
    {
        public DbSet<Files> Files { get; set; } = null!;  
        public DbSet<Values> Values { get; set; } = null!;  
        public DbSet<Results> Results { get; set; } = null!;  
        public TaskdbContext()
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=Taskdb;User=исп-41;Password=1234567890;Encrypt=false");
        }
    }
}
