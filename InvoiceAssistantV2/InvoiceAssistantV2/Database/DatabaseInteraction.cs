using Database.DbInteractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class DatabaseInteraction
    {
        private Data.InvoiceAssistantDbContext _DbContext;
        public DatabaseInteraction()
        {
            _DbContext = new Data.InvoiceAssistantDbContext();
        }

        public Data.InvoiceAssistantDbContext DbContext { get => this._DbContext; }

        private InvoiceDb _Invoices = null!;
        public InvoiceDb Invoices
        {
            get
            {
                if (this._Invoices == null)
                    this._Invoices = new InvoiceDb(this._DbContext);

                return this._Invoices;
            }
        }

        /// <summary>
        /// Will updated the database with any CRUD operations we have performed
        /// </summary>
        public void SaveChanges()
        {
            this._DbContext.SaveChanges();
        }
    }
}
