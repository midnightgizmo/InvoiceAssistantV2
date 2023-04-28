using InvoiceAssistantV2.Client.Classes.Server;
using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Shared.Models.Database.User;
using Microsoft.AspNetCore.Components;
using System;
using shared = InvoiceAssistantV2.Shared.Models.Database;
namespace InvoiceAssistantV2.Client.ViewModels.UserDetails
{
	public class UsersDetailsVM
	{
		private HttpClient _HttpClient;
		private AppSettings _AppSettings;
		private NavigationManager _NavManager;
		private ApplicationSharedData _SharedData;


        public UsersDetailsVM()
        {
			this.UsersDetails.PaymentMethods = new List<PaymentMethod>();
		}

		/// <summary>
		/// Must be called before using the view model
		/// </summary>
		/// <param name="httpClient"></param>
		/// <param name="appSettings"></param>
        public async Task Inishazlise(HttpClient httpClient, AppSettings appSettings, NavigationManager NavManager, ApplicationSharedData SharedData)
		{
			this._HttpClient = httpClient;
			this._AppSettings = appSettings;
			this._NavManager = NavManager;
			this._SharedData = SharedData;

			await this.LoadUsersDetailsFromServer();
			return;
		}



		/// <summary>
		/// Set inishaly to false becase we have to get the data from the server before
		/// we can edit anything
		/// </summary>
		public bool IsViewModelEditable { get; set; } = false;


		public shared.User.UserDetails UsersDetails { get; set; } = new shared.User.UserDetails();

	
		/// <summary>
		/// lets us know when we have recieved the users info from the server so it can be displayed on the screen
		/// </summary>
		public bool HaveRecievedUsersDataFromServer { get; set;} = false;

		/// <summary>
		/// When the user clicks the update button to say they want to confirm the User details edit.
		/// Sends request to the server to get details updated. If any errors occur, user is left in
		/// edit mode
		/// </summary>
		/// <returns></returns>
		public async Task UpdateUsersDetails()
		{
			// don't allow any edits to take place while we are talking to the server
			this.IsViewModelEditable = false;

			UsersDetailsCommunication server = new UsersDetailsCommunication(this._HttpClient, this._AppSettings);
			ServerResponseSingleUserDetails serverResponse = await server.UpdateUsersDetailsAsync(UsersDetails);

			// if we were unable to make the update on the server
			if(serverResponse.HasErrors == true)
			{
				// keep us in edit mode and allow the user to carry on editing
				// (will reenable the submit button for them to try again)
				this.IsViewModelEditable = true;
				return;
			}

			// details were updated on the server.
			this.UsersDetails.EndEdit();
			this.UsersDetails.UsersName = serverResponse.ReturnValue.UsersName;
			this.IsViewModelEditable = true;
		}



		#region Add new payment method

		public async void AddNewPaymentAndOnSucsessNavigateToPaymentDetailsPage()
		{
			PaymentMethod? paymentMethod = await this.InsertNewPaymentMethod();

			if (paymentMethod != null)
			{
				// Payment details page needs needs the paymetn method model data.
				// It could get this from the server usin the paymentMethod ID we are passing
				// but we can save a trip to the server adding it to the shared data for it to pick up
				this._SharedData.PaymentMethodToEdit = paymentMethod;
				this._NavManager.NavigateTo($"/PaymentDetails/{paymentMethod.Id.ToString()}");
				
			}

			
		}

		public string NewPaymentMethodName { get; set; } = string.Empty;

		/// <summary>
		/// Take the value from <see cref="NewPaymentMethodName"/> and ask the server to create a 
		/// new <see cref="PaymentMethod"/> from it. Then add that Payment method to the this.UsersDetails.PaymentMethods
		/// </summary>
		/// <returns>The new <see cref="PaymentMethod"/> or null if not inserted</returns>
		public async Task<PaymentMethod?> InsertNewPaymentMethod()
		{
			// only carry on if the view model is edisable, else do nothing.
			if (this.IsViewModelEditable == false)
				return null;

			this.IsViewModelEditable = false;

			string paymentMethodName = this.NewPaymentMethodName.Trim();

			// don't add payment if there is no name
			if (paymentMethodName.Length < 1)
				return null;

			// check the name does not allready exist
			bool doesPaymentMethodExist = this.UsersDetails.PaymentMethods.Any(p => p.Name.Equals(paymentMethodName, StringComparison.OrdinalIgnoreCase));
			if (doesPaymentMethodExist == true)
			{
				// should display something saying name allready exists
				this.IsViewModelEditable = true;
				return null;
			}

			// if we get this far, the payment method name does not exist (at least not on the client)
			// attempt to add payment method to server
			UserPaymentDetailsCommunication serverCommunication = new UserPaymentDetailsCommunication(this._HttpClient, this._AppSettings);
			ServerResponseSinglePaymentMethod serverResponse = await serverCommunication.AddPaymentMethodAsync(this.UsersDetails.Id, this.NewPaymentMethodName);

			// if there were errors, we could not add the payment method
			if(serverResponse.HasErrors == true) 
			{
				// should display something saying somthing went wrong
				this.IsViewModelEditable = true;
				return null;
			}
			// if we get this far, the payment method has been added on the server.
			this.UsersDetails.PaymentMethods.Add(serverResponse.ReturnValue);

			this.IsViewModelEditable = true;

			// return the payment method that was created on the server
			return serverResponse.ReturnValue;
			
		}

		#endregion

		public async Task<bool> DeletePaymentMethod(PaymentMethod paymentMethod)
		{
			return false;
		}


		#region private methods

		/// <summary>
		/// Get users details and payment details from the server
		/// </summary>
		/// <returns></returns>
		private async Task LoadUsersDetailsFromServer()
		{
			this.HaveRecievedUsersDataFromServer = false;

			UsersDetailsCommunication server = new UsersDetailsCommunication(this._HttpClient, this._AppSettings);
			ServerResponseSingleUserDetails response = await server.GetUserDetailsAsync();
			if (response.HasErrors == true)
			{
				this.HaveRecievedUsersDataFromServer = false;
				return;
			}

			// make the users details (name, payment methods etc) avalable to all pages.
			// this will allow the printing page access to the users details so they dont
			// have to go off to the server and fetch them again.
			this._SharedData.UsersDetails = response.ReturnValue;

			this.UsersDetails = response.ReturnValue;
			this.IsViewModelEditable = true;
			this.HaveRecievedUsersDataFromServer = true;

			return;
		}

		#endregion
	}
}
