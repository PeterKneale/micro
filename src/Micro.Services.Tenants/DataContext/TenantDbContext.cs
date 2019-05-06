using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Micro.Services.Tenants.Data;
using Micro.Services.Tenants.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Micro.Services.Tenants.DataContext
{
    public class TenantDbContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TeamRole> TeamRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        private int tenantId { get; }

        public TenantDbContext(ITenantContext context, DbContextOptions<TenantDbContext> options) : base(options)
        {
            tenantId = context.TenantId;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            SetTableNameConventions(builder);
            FilterByTenantId(builder);

            RoleMapping(builder);
            RolePermissionMapping(builder);
            TeamMapping(builder);
            TeamRoleMapping(builder);
            TenantMapping(builder);
            UserMapping(builder);
            UserTeamMapping(builder);
            RelationshipsMapping(builder);
        }

        public void FilterByTenantId(ModelBuilder builder)
        {
            builder.Entity<Tenant>().HasQueryFilter(x => x.Id == tenantId);
            builder.Entity<User>().HasQueryFilter(x => x.TenantId == tenantId);
            builder.Entity<Team>().HasQueryFilter(x => x.TenantId == tenantId);
            builder.Entity<Role>().HasQueryFilter(x => x.TenantId == tenantId);
            builder.Entity<UserTeam>().HasQueryFilter(x => x.TenantId == tenantId);
            builder.Entity<TeamRole>().HasQueryFilter(x => x.TenantId == tenantId);
            builder.Entity<RolePermission>().HasQueryFilter(x => x.TenantId == tenantId);
        }

        public void SetTenantId()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                // only on adds
                if (entry.State != EntityState.Added)
                {
                    continue;
                }
                // only on tenant data
                if (!(entry.Entity is ITenantData data))
                {
                    continue;
                }
                data.TenantId = tenantId;
            }
        }

        public void SetTableNameConventions(ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }
        }
        
        public bool HasChanges()
        {
            return ChangeTracker.Entries().Any(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetTenantId();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            SetTenantId();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void RoleMapping(ModelBuilder builder)
        {
            builder.Entity<Role>().ToTable(@"Role", @"dbo");
            builder.Entity<Role>().Property<int>(x => x.Id).HasColumnName(@"Id").HasColumnType(@"int").IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Role>().Property<int>(x => x.TenantId).HasColumnName(@"TenantId").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            builder.Entity<Role>().Property<string>(x => x.Name).HasColumnName(@"Name").HasColumnType(@"nvarchar(255)").IsRequired().ValueGeneratedNever().HasMaxLength(255);
            builder.Entity<Role>().HasKey(@"Id");
            builder.Entity<Role>().HasIndex(@"TenantId", @"Name").IsUnique(true).HasName(@"KEY1");
        }

        private void RolePermissionMapping(ModelBuilder builder)
        {
            builder.Entity<RolePermission>().ToTable(@"RolePermission", @"dbo");
            builder.Entity<RolePermission>().Property<int>(x => x.Id).HasColumnName(@"Id").HasColumnType(@"int").IsRequired().ValueGeneratedOnAdd();
            builder.Entity<RolePermission>().Property<int>(x => x.TenantId).HasColumnName(@"TenantId").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            builder.Entity<RolePermission>().Property<int>(x => x.RoleId).HasColumnName(@"RoleId").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            builder.Entity<RolePermission>().Property<string>(x => x.Name).HasColumnName(@"Name").HasColumnType(@"nvarchar(255)").IsRequired().ValueGeneratedNever().HasMaxLength(255);
            builder.Entity<RolePermission>().HasKey(@"Id");
            builder.Entity<RolePermission>().HasIndex(@"RoleId", @"Name").IsUnique(true).HasName(@"KEY1");
        }

        private void TeamMapping(ModelBuilder builder)
        {
            builder.Entity<Team>().ToTable(@"Team", @"dbo");
            builder.Entity<Team>().Property<int>(x => x.Id).HasColumnName(@"Id").HasColumnType(@"int").IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Team>().Property<int>(x => x.TenantId).HasColumnName(@"TenantId").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            builder.Entity<Team>().Property<string>(x => x.Name).HasColumnName(@"Name").HasColumnType(@"nvarchar(255)").IsRequired().ValueGeneratedNever().HasMaxLength(255);
            builder.Entity<Team>().HasKey(@"Id");
            builder.Entity<Team>().HasIndex(@"TenantId", @"Name").IsUnique(true).HasName(@"KEY1");
        }

        private void TeamRoleMapping(ModelBuilder builder)
        {
            builder.Entity<TeamRole>().ToTable(@"TeamRole", @"dbo");
            builder.Entity<TeamRole>().Property<int>(x => x.Id).HasColumnName(@"Id").HasColumnType(@"int").IsRequired().ValueGeneratedOnAdd();
            builder.Entity<TeamRole>().Property<int>(x => x.TenantId).HasColumnName(@"TenantId").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            builder.Entity<TeamRole>().Property<int>(x => x.TeamId).HasColumnName(@"TeamId").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            builder.Entity<TeamRole>().Property<int>(x => x.RoleId).HasColumnName(@"RoleId").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            builder.Entity<TeamRole>().HasKey(@"Id");
            builder.Entity<TeamRole>().HasIndex(@"TeamId", @"RoleId").IsUnique(true).HasName(@"KEY1");
        }

        private void TenantMapping(ModelBuilder builder)
        {
            builder.Entity<Tenant>().ToTable(@"Tenant", @"dbo");
            builder.Entity<Tenant>().Property<int>(x => x.Id).HasColumnName(@"Id").HasColumnType(@"int").IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Tenant>().Property<string>(x => x.Name).HasColumnName(@"Name").HasColumnType(@"nvarchar(255)").IsRequired().ValueGeneratedNever().HasMaxLength(255);
            builder.Entity<Tenant>().Property<string>(x => x.Host).HasColumnName(@"Host").HasColumnType(@"nvarchar(255)").IsRequired().ValueGeneratedNever().HasMaxLength(255);
            builder.Entity<Tenant>().HasKey(@"Id");
            builder.Entity<Tenant>().HasIndex(@"Name").IsUnique(true);
            builder.Entity<Tenant>().HasIndex(@"Host").IsUnique(true);
        }

        private void UserMapping(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable(@"User", @"dbo");
            builder.Entity<User>().Property<int>(x => x.Id).HasColumnName(@"Id").HasColumnType(@"int").IsRequired().ValueGeneratedOnAdd();
            builder.Entity<User>().Property<int>(x => x.TenantId).HasColumnName(@"TenantId").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            builder.Entity<User>().Property<string>(x => x.FirstName).HasColumnName(@"FirstName").HasColumnType(@"nvarchar(255)").IsRequired().ValueGeneratedNever().HasMaxLength(255);
            builder.Entity<User>().Property<string>(x => x.LastName).HasColumnName(@"LastName").HasColumnType(@"nvarchar(255)").IsRequired().ValueGeneratedNever().HasMaxLength(255);
            builder.Entity<User>().Property<string>(x => x.Email).HasColumnName(@"Email").HasColumnType(@"nvarchar(255)").IsRequired().ValueGeneratedNever().HasMaxLength(255);
            builder.Entity<User>().Property<string>(x => x.Password).HasColumnName(@"Password").HasColumnType(@"nvarchar(255)").IsRequired().ValueGeneratedNever().HasMaxLength(255);
            builder.Entity<User>().HasKey(@"Id");
            builder.Entity<User>().HasIndex(@"TenantId", @"Email").IsUnique(true).HasName(@"KEY1");
        }

        private void UserTeamMapping(ModelBuilder builder)
        {
            builder.Entity<UserTeam>().ToTable(@"UserTeam", @"dbo");
            builder.Entity<UserTeam>().Property<int>(x => x.Id).HasColumnName(@"Id").HasColumnType(@"int").IsRequired().ValueGeneratedOnAdd();
            builder.Entity<UserTeam>().Property<int>(x => x.TenantId).HasColumnName(@"TenantId").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            builder.Entity<UserTeam>().Property<int>(x => x.UserId).HasColumnName(@"UserId").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            builder.Entity<UserTeam>().Property<int>(x => x.TeamId).HasColumnName(@"TeamId").HasColumnType(@"int").IsRequired().ValueGeneratedNever();
            builder.Entity<UserTeam>().HasKey(@"Id");
            builder.Entity<UserTeam>().HasIndex(@"UserId", @"TeamId").IsUnique(true).HasName(@"KEY1");
        }

        private void RelationshipsMapping(ModelBuilder builder)
        {
            builder.Entity<Role>().HasOne(x => x.Tenant).WithMany(op => op.Roles).IsRequired(true).HasForeignKey(@"TenantId");
            builder.Entity<Role>().HasMany(x => x.RolePermissions).WithOne(op => op.Role).IsRequired(true).HasForeignKey(@"RoleId");
            builder.Entity<Role>().HasMany(x => x.TeamRoles).WithOne(op => op.Role).IsRequired(true).HasForeignKey(@"RoleId");

            builder.Entity<RolePermission>().HasOne(x => x.Role).WithMany(op => op.RolePermissions).IsRequired(true).HasForeignKey(@"RoleId");

            builder.Entity<Team>().HasOne(x => x.Tenant).WithMany(op => op.Teams).IsRequired(true).HasForeignKey(@"TenantId");
            builder.Entity<Team>().HasMany(x => x.TeamRoles).WithOne(op => op.Team).IsRequired(true).HasForeignKey(@"TeamId");
            builder.Entity<Team>().HasMany(x => x.UserTeams).WithOne(op => op.Team).IsRequired(true).HasForeignKey(@"TeamId");

            builder.Entity<TeamRole>().HasOne(x => x.Team).WithMany(op => op.TeamRoles).IsRequired(true).HasForeignKey(@"TeamId");
            builder.Entity<TeamRole>().HasOne(x => x.Role).WithMany(op => op.TeamRoles).IsRequired(true).HasForeignKey(@"RoleId");

            builder.Entity<Tenant>().HasMany(x => x.Roles).WithOne(op => op.Tenant).IsRequired(true).HasForeignKey(@"TenantId");
            builder.Entity<Tenant>().HasMany(x => x.Teams).WithOne(op => op.Tenant).IsRequired(true).HasForeignKey(@"TenantId");
            builder.Entity<Tenant>().HasMany(x => x.Users).WithOne(op => op.Tenant).IsRequired(true).HasForeignKey(@"TenantId");

            builder.Entity<User>().HasOne(x => x.Tenant).WithMany(op => op.Users).IsRequired(true).HasForeignKey(@"TenantId");
            builder.Entity<User>().HasMany(x => x.UserTeams).WithOne(op => op.User).IsRequired(true).HasForeignKey(@"UserId");

            builder.Entity<UserTeam>().HasOne(x => x.User).WithMany(op => op.UserTeams).IsRequired(true).HasForeignKey(@"UserId");
            builder.Entity<UserTeam>().HasOne(x => x.Team).WithMany(op => op.UserTeams).IsRequired(true).HasForeignKey(@"TeamId");
        }
    }
}