using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DbInteractions
{
	public class PaymentTypeDb
	{
        private Data.InvoiceAssistantDbContext _DbContext;
        public PaymentTypeDb(Data.InvoiceAssistantDbContext dbContext)
        {
            _DbContext = dbContext;
        }

		public List<PaymentType> SelectAll()
        {
            return this._DbContext.PaymentTypes.ToList();
        }
    }
}
