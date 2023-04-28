using InvoiceAssistantV2.Shared.Models.Database.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DbInteractions
{
	public class PaymentMethodDb
	{
		private Data.InvoiceAssistantDbContext _DbContext;
		public PaymentMethodDb(Data.InvoiceAssistantDbContext dbContext)
		{
			_DbContext = dbContext;
		}

		/// <summary>
		/// Select all payment methods held in the database
		/// </summary>
		/// <returns>List of payment methods</returns>
		public List<PaymentMethod> SelectAllPaymentMethods()
		{
			return this._DbContext.PaymentMethods.ToList();
		}

		/// <summary>
		/// Finds the payment method that has the passed in name
		/// </summary>
		/// <param name="PaymentMethodName">payment method name to look for</param>
		/// <returns>found payment method or null if not found</returns>
		public PaymentMethod? SelectPaymentMethod(string PaymentMethodName)
		{
			return _DbContext.PaymentMethods.Where(p => p.Name == PaymentMethodName).FirstOrDefault();
		}

		/// <summary>
		/// Get all payment methods linked to user
		/// </summary>
		/// <param name="UserDetailsId">The Id of the user the payments are linked too</param>
		/// <returns>List of payments linked to user</returns>
		public List<PaymentMethod> SelectAllPaymentMethodsLinkedToUser(int UserDetailsId)
		{
			return this._DbContext.PaymentMethods.Where(p => p.UserDetailsId == UserDetailsId).ToList();
		}

		/// <summary>
		/// Selects the payment method row in the database that matches the passed in Id
		/// </summary>
		/// <param name="PaymentMethodId">The row to look for</param>
		/// <returns>null if could not be found</returns>
		public PaymentMethod? SelectPaymentMethod(int PaymentMethodId)
		{
			return this._DbContext.PaymentMethods.Where(i => i.Id == PaymentMethodId).FirstOrDefault();
		}

		/// <summary>
		/// Adds a payment method to the database (will update the passed in object with a primary key when inserted).
		/// </summary>
		/// <param name="paymentMethod">The data to add to the database</param>
		public void InsertPaymentMethod(PaymentMethod paymentMethod) 
		{
			this._DbContext.Add(paymentMethod);
			this._DbContext.SaveChanges();
		}

		/// <summary>
		/// Updates corisponding row in the datebase with the passed in payment method.
		/// </summary>
		/// <param name="paymentMethod">updates to apply</param>
		public bool UpdatePaymentMethod(PaymentMethod paymentMethod) 
		{
			this._DbContext.Update(paymentMethod);
			if (this._DbContext.SaveChanges() > 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Removes the passed in payment method from the database
		/// </summary>
		/// <param name="paymentMethod">the row to remove from the database</param>
		/// <returns>true if sucsefull, else false</returns>
		public bool DeletePaymentMethod(PaymentMethod paymentMethod) 
		{
			this._DbContext.PaymentMethods.Remove(paymentMethod);
			if (this._DbContext.SaveChanges() > 0)
				return true;
			else
				return false;
		}
	}
}
