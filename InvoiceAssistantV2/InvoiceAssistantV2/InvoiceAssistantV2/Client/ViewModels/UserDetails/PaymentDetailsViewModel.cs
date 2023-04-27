using InvoiceAssistantV2.Client.Classes.Server;
using InvoiceAssistantV2.Client.Components;
using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Client.Pages.UserDetails;
using InvoiceAssistantV2.Shared.Models.Database.Company;
using InvoiceAssistantV2.Shared.Models.Database.User;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

namespace InvoiceAssistantV2.Client.ViewModels.UserDetails
{
	public class PaymentDetailsViewModel
	{
		private HttpClient _HttpClient;
		private AppSettings _AppSettings;

		/// <summary>
		/// Will be true if we found all the information needed to load the page
		/// </summary>
		public bool CanPageLoad { get; set; } = false;

		/// <summary>
		/// if false, nothing should be editable
		/// </summary>
		public bool IsViewModelEditable { get; set; } = false;

        public PaymentMethod PaymentInfo { get; set; }


		/// <summary>
		/// Binds to the <input id="txtPaymentDetailName"/> Used in <see cref="UpdatePaymentMethodDetails"/>
		/// </summary>
		public string NewPaymentDetailName { get; set; } = string.Empty;
		/// <summary>
		/// Binds to the <input id="txtPaymentDetailValue"/> Used in <see cref="UpdatePaymentMethodDetails"/>
		/// </summary>
		public string NewPaymentDetailValue { get; set; } = string.Empty;

		/// <summary>
		/// Must be called before calling any methods on the view model
		/// </summary>
		/// <param name="httpClient"></param>
		/// <param name="appSettings"></param>
		/// <returns>true if view model can be used, else false</returns>
		public async Task<bool> Inishazlise(HttpClient httpClient, AppSettings appSettings, int PaymentMethodId)
		{
			bool CanUseViewModel = false;

			this._HttpClient = httpClient;
			this._AppSettings = appSettings;

			// if we don't have any payment info, we need to go off to the server to get it
			if(this.PaymentInfo == null || this.PaymentInfo.Id != PaymentMethodId)
			{
				PaymentMethod? paymentInfo;
				paymentInfo = await this.LoadPaymentMethodInfoFromServer(PaymentMethodId);

				if(paymentInfo != null)
				{
					CanUseViewModel = true;
					// set the property with the payment info
					this.PaymentInfo = paymentInfo;
				}
			}

			

			if(CanUseViewModel == true)
			{
				this.PaymentInfo.PaymetDetails = await this.LoadPaymentDetailsFromServer(PaymentMethodId);
				this.CanPageLoad = true;
				// allow view model to be edited by user
				this.IsViewModelEditable = true;

			}

			return CanUseViewModel;
		}


		/// <summary>
		/// When the user clicks the update button to say they want to confirm the payment details name edit.
		/// Sends request to the server to get details updated. If any errors occur, user is left in
		/// edit mode
		/// </summary>
		/// <returns></returns>
		public async Task UpdatePaymentMethodDetails()
		{
			if (this.IsViewModelEditable == false)
				return;

			// don't allow any edits to take place while we are talking to the server
			this.IsViewModelEditable = false;

			UserPaymentDetailsCommunication server = new UserPaymentDetailsCommunication(this._HttpClient, this._AppSettings);
			ServerResponseBool serverResponse = await server.UpdatePaymentMethodName(this.PaymentInfo);
			
			// if we were unable to make the update on the server
			if (serverResponse.HasErrors == true)
			{
				// keep us in edit mode and allow the user to carry on editing
				// (will reenable the submit button for them to try again)
				this.IsViewModelEditable = true;
				return;
			}

			// details were updated on the server.
			this.PaymentInfo.EndEdit();
			this.IsViewModelEditable = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="paymentDetails">The payment details to update on the server</param>
		/// <returns></returns>
		public async Task UpdatePaymentDetails(PaymetDetail paymentDetails)
		{
			if (this.IsViewModelEditable == false)
				return;

			// don't allow any edits to take place while we are talking to the server
			this.IsViewModelEditable = false;

			UserPaymentDetailsCommunication server = new UserPaymentDetailsCommunication(this._HttpClient, this._AppSettings);
			ServerResponseSinglePaymentDetail serverResponse = await server.UpdatePaymentDetails(paymentDetails);

			if(serverResponse.HasErrors == true) 
			{
				// keep us in edit mode and allow the user to carry on editing
				this.IsViewModelEditable = true;
				return;
			}
			// details were updated on the server
			paymentDetails.EndEdit();
			this.IsViewModelEditable = true;
		}

		public async Task AddNewPaymentDetail()
		{
			
			if(this.IsViewModelEditable == false)
				return;

			this.IsViewModelEditable = false;

			PaymetDetail newPaymentDetail = new PaymetDetail();
			newPaymentDetail.PaymentMethodId = this.PaymentInfo.Id;
			newPaymentDetail.Key = this.NewPaymentDetailName.Trim();
			newPaymentDetail.Value = this.NewPaymentDetailValue.Trim();

			
			// we must have at least a key
			if (newPaymentDetail.Key.Length < 1)
			{
				this.IsViewModelEditable = true;
				return;
			}

			UserPaymentDetailsCommunication server = new UserPaymentDetailsCommunication(this._HttpClient, this._AppSettings);

			ServerResponseSinglePaymentDetail response = await server.AddPaymentDetail(newPaymentDetail);

			// if server was unable to add payment
			if (response.HasErrors == true) 
			{
				this.IsViewModelEditable = true;
				return;
			}

			if (this.PaymentInfo.PaymetDetails == null)
				this.PaymentInfo.PaymetDetails = new List<PaymetDetail>();

			// add the new payment detail to the list of payment details for this payment method
			this.PaymentInfo.PaymetDetails.Add(response.ReturnValue);
			this.IsViewModelEditable = true;

			return;
		}

		public async Task<bool> DeletePaymentDetail(PaymetDetail paymentDetail)
		{
			UserPaymentDetailsCommunication server = new UserPaymentDetailsCommunication(this._HttpClient,this._AppSettings);
			ServerResponseBool response = await server.DeletePaymentDetail(paymentDetail);

			// if we were unable to delete payment detail on the server
			if (response.HasErrors == true)
				return false;

			// if we get this far payment has been deleted on the server.
			// we now need to remove the payment detail from the client
			this.PaymentInfo.PaymetDetails.RemoveAll(p => p.Id == paymentDetail.Id);
			
			return true;

		}
			

		/// <summary>
		/// Gets the specified payment method from the server
		/// </summary>
		/// <param name="PaymentMethodInfoId">id of teh payment method to look for on the server</param>
		/// <returns></returns>
		private async Task<PaymentMethod?> LoadPaymentMethodInfoFromServer(int PaymentMethodInfoId)
		{
			UserPaymentDetailsCommunication server = new UserPaymentDetailsCommunication(this._HttpClient, this._AppSettings);
			ServerResponseSinglePaymentMethod serverResponse;

			serverResponse = await server.GetPaymentMethodAsync(PaymentMethodInfoId);
			if (serverResponse.HasErrors == true)
				return null;
			else
				return serverResponse.ReturnValue;
		}

		/// <summary>
		/// Gets all the rows of data assoshiated with the passed in PaymentInfoId
		/// </summary>
		/// <param name="PaymentMethodInfoId"></param>
		/// <returns></returns>
		private async Task<List<PaymetDetail>> LoadPaymentDetailsFromServer(int PaymentMethodInfoId)
		{
			UserPaymentDetailsCommunication server = new UserPaymentDetailsCommunication(this._HttpClient,this._AppSettings);
			ServerResponseListOfPaymentDetails serverResponse;

			serverResponse = await server.GetPaymentDetailsAsync(PaymentMethodInfoId);
			if (serverResponse.HasErrors == true)
				return new List<PaymetDetail>();
			else
				return serverResponse.ReturnValue;

		}



	}
}
