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

        /// <summary>
        /// Get the payment Type that corrisponds to the passed in payment type
        /// </summary>
        /// <param name="PaymentTypeId">The id to look up in the database</param>
        /// <returns>PaymentType object or null if not found</returns>
        public PaymentType? Select(int PaymentTypeId)
        {
            return this._DbContext.PaymentTypes.Where(i => i.Id == PaymentTypeId).FirstOrDefault();
        }
    }
}
