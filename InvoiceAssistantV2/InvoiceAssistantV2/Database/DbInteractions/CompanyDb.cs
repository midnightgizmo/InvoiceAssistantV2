using InvoiceAssistantV2.Shared.Models.Database.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DbInteractions
{
    public class CompanyDb
    {
        private Data.InvoiceAssistantDbContext _DbContext;
        public CompanyDb(Data.InvoiceAssistantDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public List<CompanyDetails> SelectAllCompanies()
        {
            return this._DbContext.CompanyDetails.ToList();
        }
    }
}
