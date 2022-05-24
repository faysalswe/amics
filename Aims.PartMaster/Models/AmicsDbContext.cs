using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class AmicsDbContext : DbContext
    {
        public DbSet<AimcsSpLookUp> AmicsSpLookups { get; set; }
        public DbSet<LstWarehouse> LstWarehouses { get; set; }
        public DbSet<LstLocation> LstLocations { get; set; }

        public AmicsDbContext(DbContextOptions<AmicsDbContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
