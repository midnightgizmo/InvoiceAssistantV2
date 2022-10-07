using InvoiceAssistantV2.Shared.Models.Database.Company;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Data
{
    public class InvoiceAssistantDbContext : DbContext
    {
        private readonly string _DatabaseLocation;

        /// <summary>
        /// The location of the mysql database file
        /// </summary>
        /// <param name="DatabaseLocation"></param>
        public InvoiceAssistantDbContext(string DatabaseLocation)
        {
            this._DatabaseLocation = DatabaseLocation;
            
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
            OptionsBuilder.UseSqlite($"Data Source={this._DatabaseLocation}");
        }
    }
}
