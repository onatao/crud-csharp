using ApiCrud.Students;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Data
{
    public class AppDbContext : DbContext 
    {
        public DbSet<Student> Students { get; set; }

        // configuring how entitiy fw communicate with sql
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Testing.Database");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
