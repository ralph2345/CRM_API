using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Crm.Domain.Entities;

namespace Crm.Persistence
{
    public class CrmDbContext : DbContext
    {
        public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<ApiTokenModel> ApiTokens { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<CompanyDetails> CompanyDetails { get; set; }
        public DbSet<ContactPerson> ContactPersons { get; set; }
        public DbSet<ClientDetails> ClientDetails { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<TaskDetails> TaskDetails { get; set; }
        public DbSet<LeadTbl> LeadTbl { get; set; }
        public DbSet<DealTbl> DealTbl { get; set; }
        public DbSet<Payment> Payment { get; set; }

        //public DbSet<PasswordResetTokens> PasswordResetTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiTokenModel>().ToTable("ApiTokenModel");
            modelBuilder.Entity<Clients>().ToTable("Clients");
            modelBuilder.Entity<CompanyDetails>().ToTable("CompanyDetails");
            modelBuilder.Entity<ContactPerson>().ToTable("ContactPerson");
            modelBuilder.Entity<ClientDetails>().ToTable("ClientDetails");
            modelBuilder.Entity<Comments>().ToTable("Comments");
            modelBuilder.Entity<TaskDetails>().ToTable("TaskDetails");
            modelBuilder.Entity<LeadTbl>().ToTable("LeadTbl");
            modelBuilder.Entity<DealTbl>().ToTable("DealTbl");
            modelBuilder.Entity<Payment>().ToTable("Payment");
           

            modelBuilder.Entity<Clients>(entity =>
            {
                // One-to-One relationships
                entity.HasOne(c => c.CompanyDetails)
                      .WithOne(cd => cd.Clients)
                      .HasForeignKey<CompanyDetails>(cd => cd.ClientID);

                entity.HasOne(c => c.ClientDetails)
                      .WithOne(cd => cd.Clients)
                      .HasForeignKey<ClientDetails>(ct => ct.ClientID);

                entity.HasOne(c => c.LeadTbl)
                      .WithOne(l => l.Clients)
                      .HasForeignKey<LeadTbl>(l => l.ClientID);

                // One-to-Many relationships
                entity.HasMany(c => c.ContactPerson)
                      .WithOne(cp => cp.Clients)
                      .HasForeignKey(cp => cp.ClientID);

                entity.HasMany(c => c.TaskDetails)
                      .WithOne(t => t.Clients)
                      .HasForeignKey(t => t.ClientID);

            });

            modelBuilder.Entity<ClientDetails>(entity =>
            {
                // ClientDetails -> Notes (One-to-Many)
                entity.HasMany(cd => cd.Notes)
                      .WithOne(n => n.ClientDetails)
                      .HasForeignKey(n => n.ClientDetailsId);

            });

            modelBuilder.Entity<LeadTbl>(entity =>
            {
                entity.HasOne(l => l.DealTbl) // Lead has one SalesRep
                      .WithOne(sr => sr.LeadTbl) // SalesRep belongs to one Lead
                      .HasForeignKey<DealTbl>(sr => sr.LeadId);

                entity.HasOne(p => p.Payment) // Lead has one SalesRep
                      .WithOne(pt => pt.LeadTbl) // SalesRep belongs to one Lead
                      .HasForeignKey<Payment>(pt => pt.LeadId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
