using InvoiceAssistantV2.Shared.Models.Database.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DbInteractions
{
	public class PaymentDetailsDb
	{
		private Data.InvoiceAssistantDbContext _DbContext;
		public PaymentDetailsDb(Data.InvoiceAssistantDbContext dbContext)
		{
			_DbContext = dbContext;
		}

		public PaymetDetail? SelectPaymentDetail(int PaymentDetailsId)
		{
			return this._DbContext.PaymentDetails.Where(p => p.Id == PaymentDetailsId).FirstOrDefault();
		}

		public List<PaymetDetail> SelectAllPaymentDetailsRelatingToPaymentMethod(int PaymentMethodId)
		{
			return this._DbContext.PaymentDetails.Where(p => p.PaymentMethodId == PaymentMethodId).ToList();
		}

		public bool UpdatePaymentDetails(PaymetDetail paymentDetails) 
		{
			this._DbContext.PaymentDetails.Update(paymentDetails);
			if (this._DbContext.SaveChanges() > 0)
				return true;
			else
				return false;
		}

		public bool InsertPaymentDetails(PaymetDetail paymetDetail)
		{
			this._DbContext.PaymentDetails.Add(paymetDetail);
			if (this._DbContext.SaveChanges() > 0)
				return true;
			else 
				return false;
		}

		public bool DeletePaymentDetails(PaymetDetail paymentDetail)
		{
			this._DbContext.PaymentDetails.Remove(paymentDetail);
			if (this._DbContext.SaveChanges() > 0)
				return true;
			else
				return false;
		}
	}
}
