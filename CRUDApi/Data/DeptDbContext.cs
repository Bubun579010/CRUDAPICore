using CRUDApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDApi.Data
{
    public class DeptDbContext : DbContext
    {
        public DeptDbContext(DbContextOptions<DeptDbContext> options) : base(options)
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
