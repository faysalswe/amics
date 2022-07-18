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
        public DbSet<InvStatus> DbxInvStatus { get; set; }
        public DbSet<LstDefaultsValues> ListDefaultsValues { get; set; }
        public DbSet<LstErLookup> ListErLookup { get; set; }        
        public DbSet<LstBomCount> LstBomCount { get; set; }
        public DbSet<LstMessage> LstMessage { get; set; }
        public DbSet<LstPacklist> LstPacklist { get; set; }
        public DbSet<LstViewLocationWh> LstViewLocationWh { get; set; }
        public DbSet<LstViewLocation> LstViewLocation { get; set; }
        public DbSet<LstBomGridItems> LstBomGridItems { get; set; }
        public DbSet<LstTransLog> ListTransLog { get; set; }
        public DbSet<LstInquiry> LstInquiry { get; set; }
        public DbSet<LstSerial> LstSerial { get; set; }
        public DbSet<TransNextNum> DbxTransNextNum { get; set; }
        public DbSet<InvReceipts> DbxInvReceipts { get; set; }                            
        public DbSet<LstNotes> LstNotes { get; set; }
        public DbSet<LstMessagetext> LstMessagetext { get; set; }
        public DbSet<LstChangeLocSearch> LstChangeLocSearch { get; set; }
        public DbSet<OutValidateSerTag> OutValidateSerTag { get; set; }
        public DbSet<LstMdat> LstMdat { get; set; }

        public AmicsDbContext(DbContextOptions<AmicsDbContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvReceipts>().HasNoKey();
            modelBuilder.Entity<TransNextNum>().HasNoKey();
            modelBuilder.Entity<InvSerLot>().HasNoKey();

            modelBuilder.Entity<InvTrans>().HasNoKey();
            modelBuilder.Entity<SpPick>().HasNoKey();


            modelBuilder.Entity<InvReceipts>().HasNoKey();
            modelBuilder.Entity<TransNextNum>().HasNoKey();
            modelBuilder.Entity<LstMessagetext>().HasNoKey();
            modelBuilder.Entity<LstChangeLocSearch>().HasNoKey();

            modelBuilder.Entity<InputValidateSerTag>().HasNoKey();
            modelBuilder.Entity<OutValidateSerTag>().HasNoKey();

        }
    }
}
