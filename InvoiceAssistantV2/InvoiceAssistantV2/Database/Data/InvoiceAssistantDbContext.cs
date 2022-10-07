using InvoiceAssistantV2.Shared.Models.Database.Company;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;
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
        public DbSet<CompanyAddress> CompnayAddress { get; set; } = null!;


        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<InvoicePaymentBreakDown> InvoicesPaymentBreakDown { get; set; } = null!;
        public DbSet<PlacesVisitedForInvoice> PlacesVisitedForInvoice { get; set; } = null!;
        public DbSet<PaymentType> PaymentTypes { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
        {
            // set the location of the MySqlite database file
            OptionsBuilder.UseSqlite($"Data Source={this.GetConnectionURL()}");
            

            
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
    }
}
