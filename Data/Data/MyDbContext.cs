using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Data.Data
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options):base(options) 
        {
        }
        public DbSet<DepartmentEntity> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DepartmentEntity>()
                .HasIndex(D => D.Name)
                .IsUnique();
        }
    }
}
