using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.User;

namespace InvoiceAssistantV2.Server.ControllersLogic.User.Payment
{
	public class AddNewPaymentMethodControllerLogic
	{
		private HttpResponse _HttpResponse = null!;

		private string _PaymentMethodName = string.Empty;
		private int _UsersDetailsId = 0;

		public AddNewPaymentMethodControllerLogic(HttpResponse httpResponse)
		{
			this._HttpResponse = httpResponse;
		}

		public ControllerLogicReturnValue Process(int UsersDetailsId, string PaymentMethodName)
		{
			ControllerLogicReturnValue returnValue;

			//returnValue = this.CheckInputsAndAddPaymentDetail(PaymentMethodId, PaymentDetailKey, PaymentDetailsValue);
			returnValue = this.CheckInput(UsersDetailsId, PaymentMethodName);
			if (returnValue.HasErrors)
				return returnValue;

			// checks the input and removes any white space. sets the this._PaymentMethodName if no errors are found
			returnValue = this.DoesPaymentMethodNameAllreadyExist((string)returnValue.ReturnValue);
			if(returnValue.HasErrors) 
				return returnValue;


			returnValue = this.AddNewPaymentMethodName(this._PaymentMethodName);

			return returnValue;
		}





		/// <summary>
		/// Checks input to make sure it is ok and remove white space
		/// </summary>
		/// <param name="_UsersDetailsId"></param>
		/// <param name="PaymentMethodName"></param>
		/// <returns>ControllerLogicReturnValue.ReturnValue set to PaymentMethodName to use or ControllerLogicReturnValue.HasErrors set to true if errors found</returns>
		private ControllerLogicReturnValue CheckInput(int UsersDetailsId, string PaymentMethodName)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			string paymentMethod = PaymentMethodName.Trim();

			// if we have not recieved a payment method name
			if (paymentMethod.Length == 0)
			{
				this._HttpResponse.StatusCode = 400;
				returnValue.HasErrors = true;
				returnValue.Errors.Add("PaymentMethodName required");
			}

			if(UsersDetailsId < 1)
			{
				this._HttpResponse.StatusCode = 400;
				returnValue.HasErrors = true;
				returnValue.Errors.Add("UsersDetailsId must be greater than zero");
			}

			if(returnValue.HasErrors == false)
			{
				// set the private paymentMethodName for use later on when adding the new payment method
				this._PaymentMethodName = paymentMethod;
				this._UsersDetailsId = UsersDetailsId;
			}

			return returnValue;
		}

		/// <summary>
		/// Checks if the Payment method name allready exists and sets response status code to 409 if it does
		/// </summary>
		/// <param name="PaymentMethodName">name to check against database</param>
		/// <returns>HasErros set to true if payment method name exists</returns>
		private ControllerLogicReturnValue DoesPaymentMethodNameAllreadyExist(string PaymentMethodName)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using(InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				PaymentMethodDb paymentMethodDb = new PaymentMethodDb(dbContext);
				// if we found the payment method name
				if(paymentMethodDb.SelectPaymentMethod(PaymentMethodName) != null)
				{
					// return conflict (to indicate the payment method name allready exists)
					this._HttpResponse.StatusCode = 409;
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Payment method name allready exsists. Choose a differnet namae");
				}
			}

			// returnValue.HasErrors set to true if Payment method name allready exists
			return returnValue;
		}


		private ControllerLogicReturnValue AddNewPaymentMethodName(string paymentMethod)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				PaymentMethodDb paymentMethodDb = new PaymentMethodDb(dbContext);
				PaymentMethod paymentMethodModel = new PaymentMethod()
				{
					Name = this._PaymentMethodName,
					UserDetailsId = this._UsersDetailsId
				};
				paymentMethodDb.InsertPaymentMethod(paymentMethodModel);
				// we can check if the insert has sucseeded by checking the id on the model.
				// it should be greater than zero
				if(paymentMethodModel.Id > 0)
				{
					returnValue.ReturnValue = paymentMethodModel;
				}
				// Unable to add payment method to the database (somthing went wrong)
				else
				{
					this._HttpResponse.StatusCode = 500;
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Payment method name allready exsists. Choose a differnet namae");
				}
			}

			return returnValue;
		}
	}
}
