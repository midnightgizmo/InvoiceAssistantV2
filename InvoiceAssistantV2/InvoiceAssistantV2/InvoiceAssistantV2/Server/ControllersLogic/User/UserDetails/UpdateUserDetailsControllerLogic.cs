using Database.Data;
using Database.DbInteractions;

namespace InvoiceAssistantV2.Server.ControllersLogic.User.UserDetails
{
	public class UpdateUserDetailsControllerLogic
	{
		//this will be inishalized when Process method is called
		// used for setting the status code
		private HttpResponse _HttpResponse = null!;

		public ControllerLogicReturnValue Process(int id, string usersName, HttpResponse HttpServerResponse)
		{
			ControllerLogicReturnValue returnValue;

			this._HttpResponse = HttpServerResponse;



			returnValue = this.CheckInputParameters(id, usersName);
			// if there was somthing wrong with the input parameters
			if (returnValue.HasErrors == true)
				return returnValue;

			// attempt to update the users details with the passed in parameters.
			// sets status code to 400 if any errors occur
			returnValue = this.UpdateUserDetails(id, usersName);

			return returnValue;
		}

		private ControllerLogicReturnValue CheckInputParameters(int id, string usersName)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();
			
			if(id <= 0)
			{
				this._HttpResponse.StatusCode = 400;
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Invalid id");
			}

			if(usersName.Trim().Length == 0)
			{
				this._HttpResponse.StatusCode = 400;
				returnValue.HasErrors = true;
				returnValue.Errors.Add("usersName required");
			}

			return returnValue;

		}

		/// <summary>
		/// updates the UsersDetails Table with the passed in parameters
		/// </summary>
		/// <param name="id">the user to update</param>
		/// <param name="usersName">the new username to apply</param>
		/// <returns></returns>
		private ControllerLogicReturnValue UpdateUserDetails(int id, string usersName)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				UserDetailsDb userDetailsDb = new UserDetailsDb(dbContext);

				Shared.Models.Database.User.UserDetails? usersDetails;
				// find the user in the database we want to update
				usersDetails = userDetailsDb.SelectUsersDetails();

				// if we could not find the user
				if(usersDetails == null)
				{
					this._HttpResponse.StatusCode = 400;
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Unable to find user to update");
					return returnValue;
				}

				// we found the user so update there details with the passed in details
				usersDetails.UsersName = usersName.Trim();
				userDetailsDb.UpdateUsersDetails(usersDetails);

				// set the userDetails as the return
				returnValue.ReturnValue = usersDetails;
			}

			return returnValue;
		}

	}
}
