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
            modelBuilder.Entity<TenantData>(tenant =>
            {
                tenant.ToTable("Tenants");
                tenant.HasKey(c => c.Id).HasName("PK_Tenant_Id");
                tenant.Property(b => b.Name).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<UserData>(user =>
            {
                user.ToTable("Users");
                user.HasKey(c => c.Id).HasName("PK_User_Id");
                user.Property(b => b.FirstName).HasMaxLength(100).IsRequired();
                user.Property(b => b.LastName).HasMaxLength(100).IsRequired();
                user.Property(b => b.TenantId).IsRequired();
                user.HasOne(p => p.Tenant)
                    .WithMany(b => b.Users)
                    .HasForeignKey(p => p.TenantId)
                    .HasConstraintName("FK_User_Tenant")
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public DbSet<TenantData> Tenants { get; set; }

        public DbSet<UserData> Users { get; set; }
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
