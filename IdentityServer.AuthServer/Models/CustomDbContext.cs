using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.AuthServer.Models
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> opts) : base(opts)
        {

        }

        public DbSet<CustomUser> customUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CustomUser>().HasData(
                new CustomUser() { Id = 1, Email = "hakan@gmail.com", Password = "hakan", City = "İstanbul", UserName = "hakan46" },
                new CustomUser() { Id = 2, Email = "bayazit@gmail.com", Password = "hakan", City = "Ankara", UserName = "bayazit46" },
                new CustomUser() { Id = 3, Email = "habes@gmail.com", Password = "hakan", City = "İzmir", UserName = "habes46" }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
