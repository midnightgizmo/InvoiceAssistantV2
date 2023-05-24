using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.User;
using System.Diagnostics.CodeAnalysis;

namespace InvoiceAssistantV2.Server.ControllersLogic.User.UserAddress
{
	public class UpdateUsersAddressControllerLogic
	{
		private HttpResponse _HttpResponse = null!;

		private string _PaymentMethodName = string.Empty;
		private int _UsersDetailsId = 0;

		public UpdateUsersAddressControllerLogic(HttpResponse httpResponse)
		{
			this._HttpResponse = httpResponse;
		}

		public ControllerLogicReturnValue Process(Shared.Models.Database.User.UserAddress userAddress)
		{
			ControllerLogicReturnValue returnValue;


			// make sure the address we are being sent exists in the database.
			// Also make sure we use the UserAddress.UserDetailsId value found in the database 
			// and not the one passed in from the user (we don't want the user to edit this value)
			returnValue = this.DoesAddressExist(userAddress);

			// if it does not exist
			if(returnValue.HasErrors == true)
				return returnValue;

			// check the input parameters are ok, and modify them if needed
			this.CheckAndModifyInputDataIfNecessary(userAddress);

			

			// try and update the databse with the new changes
			returnValue = this.UpdateAddressInDatabase(userAddress);

			// return the updated userAddress or any errorr that occured
			return returnValue;

		}

		/// <summary>
		/// Checks if the UserAddressId exists in the UserAddress Table in the database and if found sets the
		/// userAddress.userDetailsId to the value currently set in the database
		/// </summary>
		/// <param name="userAddress">modelthat contains the ID to search for in UserAddress.Id table</param>
		/// <returns>ControllerLogicReturnValue.HasErrors set to true if addresss not found</returns>
		private ControllerLogicReturnValue DoesAddressExist(Shared.Models.Database.User.UserAddress userAddress)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using(InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext()) 
			{
				UserAddressDb userAddressDb = new UserAddressDb(dbContext);
				Shared.Models.Database.User.UserAddress? userAddressModel = userAddressDb.Select(userAddress.Id);
				// if the address does not exist in the datbase
				if (userAddressModel == null)
				{
					returnValue.HasErrors = true;
					returnValue.Errors.Add("UserAddress Does not exist");
					this._HttpResponse.StatusCode = 404;
				}
				// we have found the row in the database
                else
                {
					// Dont allow userDetailsId to be modified.
					// make sure the userDetailsId value is set to the value held in the database.
					userAddress.UserDetailsId = userAddressModel.UserDetailsId;
				}

			}

			// HasErrors set to true if errors were found (address not found indatabase)
			return returnValue;
		}

		/// <summary>
		/// Checks the passed in model's values are ok to to be used and modifys them if they are not
		/// </summary>
		/// <param name="userAddress">the model to check</param>
		private void CheckAndModifyInputDataIfNecessary(Shared.Models.Database.User.UserAddress userAddress)
		{
			// make sure all the address lines are not null, and if they are, set them to string.Empty
			userAddress.AddressLine1 = userAddress.AddressLine1 ?? string.Empty;
			userAddress.AddressLine2 = userAddress.AddressLine2 ?? string.Empty;
			userAddress.AddressLine3 = userAddress.AddressLine3 ?? string.Empty;
			userAddress.AddressLine4 = userAddress.AddressLine4 ?? string.Empty;
			userAddress.AddressLine5 = userAddress.AddressLine5 ?? string.Empty;
			userAddress.PostCode = userAddress.PostCode ?? string.Empty;
		}

		/// <summary>
		/// Attempts to update the datbase with the updated values for the user addresss
		/// </summary>
		/// <param name="userAddress">The data to updated in the UserAddress table</param>
		/// <returns>sets returnValue to the updated address data or set HasErrors to true if errors were found</returns>
		private ControllerLogicReturnValue UpdateAddressInDatabase(Shared.Models.Database.User.UserAddress userAddress)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				UserAddressDb userAddressDb = new UserAddressDb(dbContext);

				// try and update the database with the new address information
				if(userAddressDb.Update(userAddress) == true)
				{// if sucsefull in update
					returnValue.ReturnValue = userAddress;
				}
				// unable to udate row in database
				else
				{
					// set up an error
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Unable to update Address:unknown error");
					this._HttpResponse.StatusCode = 500;
				}
			}

			// set ReturnValue.ReturnValue to the userAddress model or set HasErrors to true if errors occured
			return returnValue;
		}
	}
}
