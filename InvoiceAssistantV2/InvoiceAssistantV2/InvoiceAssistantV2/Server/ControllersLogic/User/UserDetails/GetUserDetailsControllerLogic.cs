﻿using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.User;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAssistantV2.Server.ControllersLogic.User.UserDetails
{
	public class GetUserDetailsControllerLogic
	{
		//this will be inishalized when Process method is called
		// used for setting the status code
		private HttpResponse _HttpResponse = null!;
		public ControllerLogicReturnValue Process(bool includePaymentDetails, HttpResponse HttpServerResponse)
		{
			ControllerLogicReturnValue returnValue;

			this._HttpResponse = HttpServerResponse;

			// get the users details, if they don't exist, create them
			returnValue = this.GetUsersDetails();
			if (returnValue.HasErrors == true)
				return returnValue;

			// if the user also wants the payment details, add that to the response
			if(includePaymentDetails == true)
			{
				this.GetPaymentDetails((Shared.Models.Database.User.UserDetails)returnValue.ReturnValue);
			}

			return returnValue;
		}

		

		/// <summary>
		/// Get the UserDetails row from the database. If it does not exist,
		/// create a new row and insert it into the database and return that new row just created
		/// </summary>
		/// <returns></returns>
		private ControllerLogicReturnValue GetUsersDetails()
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				UserDetailsDb userDb = new UserDetailsDb(dbContext);

				Shared.Models.Database.User.UserDetails? userModel = userDb.SelectUsersDetails();
				// if the row does not exist yet, create the row
				if(userModel == null)
				{
					// create a new user model that will will add to the database
					userModel = new Shared.Models.Database.User.UserDetails() { UsersName = "your name" };
					userDb.InsertUserDetails(userModel);
				}
				returnValue.ReturnValue = userModel;
			}

			return returnValue;
		}

		/// <summary>
		/// Get all payment details asoshiated with passed in user
		/// </summary>
		/// <param name="userDetails">the user who payment details we will look for</param>
		private void GetPaymentDetails(Shared.Models.Database.User.UserDetails userDetails)
		{
			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext()) 
			{
				PaymentMethodDb paymentMethodDb = new PaymentMethodDb(dbContext);
				// get all the payment methods
				userDetails.PaymentMethods = paymentMethodDb.SelectAllPaymentMethodsLinkedToUser(userDetails.Id);

				PaymentDetailsDb paymentDetailsDb = new PaymentDetailsDb(dbContext);
				// for each payment method, get all the payment details
				foreach(PaymentMethod paymentMethod in userDetails.PaymentMethods)
				{
					// get payment details
					paymentMethod.PaymetDetails = paymentDetailsDb.SelectAllPaymentDetailsRelatingToPaymentMethod(paymentMethod.Id);
				}
			}
		}
	}
}
