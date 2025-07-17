using FileBrowser.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileBrowser.Data.Context
{
    public class FileBrowserDbContext : DbContext
    {
        public FileBrowserDbContext(DbContextOptions<FileBrowserDbContext> options) 
            : base(options) 
        {
        }

        public DbSet<FileEntity> Files { get; set; }
        public DbSet<Folder> Folders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folder>()
                .HasMany(f => f.SubFolders)
                .WithOne(f => f.ParentFolder)
                .HasForeignKey(f => f.ParentFolderId);

            modelBuilder.Entity<Folder>()
                .HasMany(f => f.Files)
                .WithOne(f => f.Folder)
                .HasForeignKey(f => f.FolderId);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.ModifiedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    entry.Entity.ModifiedAt = now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
