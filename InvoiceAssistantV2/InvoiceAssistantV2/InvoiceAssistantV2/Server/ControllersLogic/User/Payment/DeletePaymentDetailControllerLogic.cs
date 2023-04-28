using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.User;

namespace InvoiceAssistantV2.Server.ControllersLogic.User.Payment
{
	/// <summary>
	/// Removes a <see cref="PaymetDetail"/> row in the database
	/// </summary>
	public class DeletePaymentDetailControllerLogic
	{
		private HttpResponse _HttpResponse = null!;



		public DeletePaymentDetailControllerLogic(HttpResponse httpResponse)
		{
			this._HttpResponse = httpResponse;
		}
		public ControllerLogicReturnValue Process(int PaymentDetailsId)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				PaymentDetailsDb dbPaymentDetails = new PaymentDetailsDb(dbContext);

				// see if the payment detail we want to remove exists
				PaymetDetail? model = dbPaymentDetails.SelectPaymentDetail(PaymentDetailsId);
				// if we did not find it
				if (model == null)
				{
					this._HttpResponse.StatusCode = 409;
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Unable to remove PaymentDetails, it was not found");

				}
				// if we did find it
				else
				{
					// if row was deleted sucsefully let the client know by passing true back
					if (dbPaymentDetails.DeletePaymentDetails(model) == true)
						returnValue.ReturnValue = true;
					else
					{
						this._HttpResponse.StatusCode = 520;
						returnValue.HasErrors = true;
						returnValue.Errors.Add("Unable to remove PaymentDetails, unsure why");

					}
				}
			}
			return returnValue;
		}
	}
}
