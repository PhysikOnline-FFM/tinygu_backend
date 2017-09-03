using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Tinygubackend.Models;

namespace Tinygubackend
{
    public partial class TinyguContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Link> Links { get; set; }

        public TinyguContext(DbContextOptions<TinyguContext> options)
            : base(options)
        {
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
            }
        }
    }
}