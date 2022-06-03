using Aims.PartMaster.Models;
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
        public DbSet<LstLocaton> LstLocations { get; set; }
        public DbSet<LstItemSearch> LstItemSearchs { get; set; }
        public DbSet<LstItemType> LstItemTypes { get; set; }
        public DbSet<LstItemCode> LstItemCodes { get; set; }
        public DbSet<LstItemClass> LstItemClasses { get; set; }
        public DbSet<LstUom> LstUoms { get; set; }
        public DbSet<ListItems> LstItemInfo { get; set; }
        public DbSet<LstCompanyOption> LstCompanyOptions { get; set; }
        public DbSet<LstFieldProperties> LstFieldProperties { get; set; }

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
