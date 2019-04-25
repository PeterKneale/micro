using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Micro.Services.Content.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
          : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogData>().ToTable("Blogs");
            modelBuilder.Entity<BlogData>().HasKey(c => c.Id).HasName("PK_Blog_Id");
            modelBuilder.Entity<BlogData>().Property(b => b.Title).HasMaxLength(100).IsRequired();

            modelBuilder.Entity<PostData>().ToTable("Posts");
            modelBuilder.Entity<PostData>().HasKey(c => c.Id).HasName("PK_Post_Id");
            modelBuilder.Entity<PostData>().Property(b => b.Title).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<PostData>().Property(b => b.Content);
            modelBuilder.Entity<PostData>().Property(b => b.BlogId).IsRequired();

            modelBuilder.Entity<PostData>()
                .HasOne(p => p.Blog)
                .WithMany(b => b.Posts)
                .HasForeignKey(p => p.BlogId)
                .HasConstraintName("FK_Post_Blog")
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<BlogData> Blogs { get; set; }

        public DbSet<PostData> Posts { get; set; }
    }

    public class BaseData
    {
        public int Id { get; set; }
    }

    public class BlogData : BaseData
    {
        public string Title { get; set; }
        public List<PostData> Posts { get; set; }
    }

    public class PostData : BaseData
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public BlogData Blog { get; set; }
    }
}
