using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Tinygubackend.Models;

namespace Tinygubackend
{
    public partial class TinyguContext : DbContext
    {
      public DbSet<User> Users { get; set; }
      public DbSet<Link> Links { get; set; }
      
      public TinyguContext(DbContextOptions<TinyguContext> options)
        : base(options)
      { }
    }
}