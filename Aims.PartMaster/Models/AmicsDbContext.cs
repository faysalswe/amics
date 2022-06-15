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
        public DbSet<LstItemDetails> LstItemDetails { get; set; }
        public DbSet<LstCompanyOption> LstCompanyOptions { get; set; }
        public DbSet<LstFieldProperties> LstFieldProperties { get; set; }
        public DbSet<LstItemsBom> LstItemsBom { get; set; }
        public DbSet<LstItemsPO> LstItemsPO { get; set; }
        public DbSet<LstItemInfo> LstItemsInfo { get; set; }
        public DbSet<LstReasonCodes> ListReasonCodes { get; set; }
        public DbSet<LstCompanyOptions> ListCompanyOptions { get; set; }         
        public DbSet<InvStatus> dbxInvStatus { get; set; }
        public DbSet<LstDefaultsValues> ListDefaultsValues { get; set; }
        public DbSet<LstErLookup> ListErLookup { get; set; }        
        public DbSet<LstBomCount> LstBomCount { get; set; }
        public DbSet<LstMessage> LstMessage { get; set; }
        public DbSet<LstViewLocation> LstViewLocation { get; set; }
        public DbSet<LstViewLocationWh> LstViewLocationWh { get; set; }
        public DbSet<LstBomGridItems> LstBomGridItems { get; set; }
        public DbSet<LstTransLog> ListTransLog { get; set; }

        
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
