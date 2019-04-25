using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Micro.Services.Tenants.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
          : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TenantData>().ToTable("Tenants");
            modelBuilder.Entity<TenantData>().HasKey(c => c.Id).HasName("PK_Tenants_Id");
            modelBuilder.Entity<TenantData>().Property(b => b.Name).HasMaxLength(100).IsRequired();

            modelBuilder.Entity<UserData>().ToTable("Users");
            modelBuilder.Entity<UserData>().HasKey(c => c.Id).HasName("PK_Users_Id");
            modelBuilder.Entity<UserData>().Property(b => b.FirstName).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<UserData>().Property(b => b.LastName).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<UserData>().Property(b => b.TenantId).IsRequired();

            modelBuilder.Entity<UserData>()
                .HasOne(p => p.Tenant)
                .WithMany(b => b.Users)
                .HasForeignKey(p => p.TenantId)
                .HasConstraintName("FK_User_Tenant")
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<TenantData> Blogs { get; set; }

        public DbSet<UserData> Posts { get; set; }
    }

    public class BaseData
    {
        public int Id { get; set; }
    }

    public class TenantData : BaseData
    {
        public string Name { get; set; }
        public List<UserData> Users { get; set; }
    }

    public class UserData : BaseData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int TenantId { get; set; }
        public TenantData Tenant { get; set; }
    }
}
