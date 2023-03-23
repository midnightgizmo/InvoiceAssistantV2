using InvoiceAssistantV2.Shared.Models.Database.Company;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using InvoiceAssistantV2.Shared.Models.Database.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Data
{
    public class InvoiceAssistantDbContext : DbContext
    {
       
        public InvoiceAssistantDbContext() : base()
        {
            
            
        }

        public DbSet<CompanyDetails> CompanyDetails { get; set; } = null!;
        public DbSet<CompanyAddress> CompanyAddress { get; set; } = null!;


        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<InvoicePaymentBreakDown> InvoicesPaymentBreakDown { get; set; } = null!;
        public DbSet<PlacesVisitedForInvoice> PlacesVisitedForInvoice { get; set; } = null!;
        public DbSet<PaymentType> PaymentTypes { get; set; } = null!;

        public DbSet<UserDetails> UserDetails { get; set; } = null!;
        public DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public DbSet<PaymetDetail> PaymentDetails { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
        {
            // set the location of the MySqlite database file
            OptionsBuilder.UseSqlite($"Data Source={this.GetConnectionURL()}");
            OptionsBuilder.LogTo(Console.WriteLine);

            
        }

        protected string GetConnectionURL()
        {
            // The following code gets the connection string for the database connection.

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // requires packages
            // Microsoft.Extensions.Configuration;
            // Microsoft.Extensions.Configuration.FileExtension
            // Microsoft.Extensions.Configuration.Json
            // Microsoft.Extensions.Configuration.EnvironmentVariable
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            
            string ConnectionString = config["AppSettings:DataBaseLocation"];

            return ConnectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // In the PlacesVisitedForInvoice class use the foreign keys to
            // create a primary key (InvoiceID & CompanyAddressID)
            modelBuilder.Entity<PlacesVisitedForInvoice>()
                .HasKey(PV => new { PV.InvoiceId, PV.CompanyAddressId });

            // Tell EF for about the one to many from Invoice to PlacesVisitedForInvoice
            modelBuilder.Entity<PlacesVisitedForInvoice>()
                .HasOne(PV => PV.Invoice)
                .WithMany(I => I.PlacesVisitedForInvoice)
                .HasForeignKey(PV => PV.InvoiceId);

            // Tell EF Core about the one to many from CompanyAddress to PlacesVisitedForInvoice
            modelBuilder.Entity<PlacesVisitedForInvoice>()
                .HasOne(PV => PV.CompanyAddress)
                .WithMany(CA => CA.PlacesVisitedForInvoice)
                .HasForeignKey(PV => PV.CompanyAddressId);

            modelBuilder.Entity<PaymentType>().HasData(
                new PaymentType() 
                {
                    Id = 1,
                    Name = "Cheque"
                });
            modelBuilder.Entity<PaymentType>().HasData(
                new PaymentType()
                {
                    Id = 2,
                    Name = "Cash"
                });
            modelBuilder.Entity<PaymentType>().HasData(
                new PaymentType()
                {
                    Id = 3,
                    Name = "Bank Transfer"
                });
        }
    }
}
