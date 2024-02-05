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
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=OMEGA003\SQLEXPRESS;Database=Taskdb;User Id=исп-41;Password=1234567890;");
        }
    }
}
