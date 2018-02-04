using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tinygubackend.Models;

namespace Tinygubackend.Contexts
{
    public class TinyguContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Link> Links { get; set; }
        public virtual DbSet<Group> Groups { get; set; }


        public TinyguContext(DbContextOptions<TinyguContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Indexed properties need to be restricted in length!
            modelBuilder.Entity<Link>().HasIndex(l => l.ShortUrl).IsUnique();
            modelBuilder.Entity<Link>(entity =>
            {
            });
            modelBuilder.Entity<User>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>(entity =>
            {
            });

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            AddTimeStamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddTimeStamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void AddTimeStamps()
        {
            var addedEntities = ChangeTracker.Entries()
                .Where(e => e.Entity is Base && e.State == EntityState.Added);

            foreach (var addedEntity in addedEntities)
            {
                ((Base)addedEntity.Entity).DateCreated = DateTime.UtcNow;
                ((Base)addedEntity.Entity).DateModified = DateTime.UtcNow;
            }

            var modifiedEntities = ChangeTracker.Entries()
                .Where(e => e.Entity is Base && e.State == EntityState.Modified);

            foreach (var addedEntity in modifiedEntities)
            {
                ((Base)addedEntity.Entity).DateModified = DateTime.UtcNow;
            }
        }
    }
}