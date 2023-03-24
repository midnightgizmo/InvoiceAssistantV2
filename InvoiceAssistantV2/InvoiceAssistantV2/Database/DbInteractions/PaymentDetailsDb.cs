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

		public void UpdatePaymentDetails(PaymetDetail paymentDetails) 
		{
			this._DbContext.PaymentDetails.Update(paymentDetails);
			this._DbContext.SaveChanges();
		}

		public void InsertPaymentDetails(PaymetDetail paymetDetail)
		{
			this._DbContext.PaymentDetails.Add(paymetDetail);
			this._DbContext.SaveChanges();
		}

		public void DeletePaymentDetails(PaymetDetail paymentDetail)
		{
			this._DbContext.PaymentDetails.Remove(paymentDetail);
		}
	}
}
