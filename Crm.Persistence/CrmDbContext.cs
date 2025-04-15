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
        public DbSet<SalesRep> SalesReps { get; set; }
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
            modelBuilder.Entity<SalesRep>().ToTable("SalesRep");
            modelBuilder.Entity<Payment>().ToTable("Payment");

            /* modelBuilder.Entity<PasswordResetTokens>()
                 .ToTable("PasswordResetTokens")
                 .HasOne(p => p.User)
                 .WithMany()
                 .HasForeignKey(p => p.UserId)
                 .OnDelete(DeleteBehavior.Cascade); // Deletes tokens if user is deleted*/

            modelBuilder.Entity<Clients>(entity =>
            {
                // One-to-One relationships
                entity.HasOne(c => c.CompanyDetails)
                      .WithOne(cd => cd.Clients)
                      .HasForeignKey<CompanyDetails>(cd => cd.ClientID);

                entity.HasOne(c => c.ClientDetails)
                      .WithOne(cd => cd.Clients)
                      .HasForeignKey<ClientDetails>(ct => ct.ClientID);

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
                entity.HasOne(l => l.SalesRep) // Lead has one SalesRep
                      .WithOne(sr => sr.LeadTbl) // SalesRep belongs to one Lead
                      .HasForeignKey<SalesRep>(sr => sr.LeadId);

                entity.HasOne(p => p.Payment) // Lead has one SalesRep
                      .WithOne(pt => pt.LeadTbl) // SalesRep belongs to one Lead
                      .HasForeignKey<Payment>(pt => pt.LeadId);
            });



            //Entities for clients
            /*modelBuilder.Entity<Clients>()
                .HasOne(c => c.CompanyDetails)
                .WithOne(cd => cd.Clients)// one to one setup
                .HasForeignKey<CompanyDetails>(cd => cd.ClientID);

            modelBuilder.Entity<Clients>()
                .HasMany(c => c.ContactPerson)//one client can have many contact persons
                .WithOne(cp => cp.Clients)
                .HasForeignKey(cp => cp.ClientID);

            modelBuilder.Entity<Clients>()
                .HasOne(c => c.ClientDetails)
                .WithOne(cp => cp.Clients)
                .HasForeignKey<ClientDetails>(ct => ct.ClientID);

            modelBuilder.Entity<ClientDetails>()
                .HasMany(c => c.Notes) // one ClientDetails can have many Comments
                .WithOne(cm => cm.ClientDetails) // each Comment belongs to one ClientDetails
                .HasForeignKey(cm => cm.ClientDetailsId);

            modelBuilder.Entity<Clients>()
                .HasMany(c => c.TaskDetails)// one client can have many tasks
                .WithOne(c => c.Clients)
                .HasForeignKey(t => t.ClientID);*/

            //entities for Leads
            /*modelBuilder.Entity<LeadTbl>()
                .HasOne(l => l.SalesRep) // Lead has one SalesRep
                .WithOne(sr => sr.LeadTbl) // SalesRep belongs to one Lead
                .HasForeignKey<SalesRep>(sr => sr.LeadId);  // Foreign key in SalesRep

            modelBuilder.Entity<LeadTbl>()
                .HasOne(p =>  p.Payment)//lead has one payment
                .WithOne(pt => pt.LeadTbl)
                .HasForeignKey<Payment>(pt => pt.LeadId); // Foreign key in Payment*/

            base.OnModelCreating(modelBuilder);
        }
    }
}
