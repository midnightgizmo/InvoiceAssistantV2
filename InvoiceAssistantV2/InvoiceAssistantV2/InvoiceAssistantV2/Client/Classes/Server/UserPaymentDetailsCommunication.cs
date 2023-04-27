using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Client.Models.Server;
using InvoiceAssistantV2.Shared.Models.Database.User;
using InvoiceAssistantV2.Client.Pages.UserDetails;
using InvoiceAssistantV2.Client.ViewModels.UserDetails;

namespace InvoiceAssistantV2.Client.Classes.Server
{
	public class UserPaymentDetailsCommunication
	{
		private HttpClient _HttpClient;
		private ServerCommunication _ServerCommunication;
		public UserPaymentDetailsCommunication(HttpClient httpClient, AppSettings appSettings)
		{
			this._HttpClient = httpClient;
			this._ServerCommunication = new ServerCommunication(this._HttpClient, appSettings);
		}

		public async Task<ServerResponseSinglePaymentMethod> GetPaymentMethodAsync(int PaymentMethodId)
		{
			ServerResponse responseMessage;
			ServerResponseSinglePaymentMethod responseData;

			Dictionary<string, string> DataToSend = new Dictionary<string, string>();

			DataToSend.Add("PaymentMethodId", PaymentMethodId.ToString());

			// get payment method from server
			responseMessage = await this._ServerCommunication.SendPostRequestToServer("User/UserDetails/Payment/GetPaymentMethod", DataToSend);

			responseData = ServerCommunication.ParseServerResponse<ServerResponseSinglePaymentMethod>(responseMessage);

			return responseData;
		}

		public async Task<ServerResponseSinglePaymentMethod> AddPaymentMethodAsync(int UsersDetailsId, string PaymentMethodName)
		{
			ServerResponse responseMessage;
			ServerResponseSinglePaymentMethod responseData;

			Dictionary<string, string> DataToSend = new Dictionary<string, string>();

			DataToSend.Add("UsersDetailsId", UsersDetailsId.ToString());
			DataToSend.Add("PaymentMethodName", PaymentMethodName);

			// add payment method on server
			responseMessage = await this._ServerCommunication.SendPostRequestToServer("User/UserDetails/Payment/AddPaymentMethod", DataToSend);

			responseData = ServerCommunication.ParseServerResponse<ServerResponseSinglePaymentMethod>(responseMessage);

			return responseData;
		}

		public async Task<ServerResponseBool> UpdatePaymentMethodName(PaymentMethod paymentDetails)
		{
			ServerResponse responseMessage;
			ServerResponseBool responseData;

			Dictionary<string, string> DataToSend = new Dictionary<string, string>();

			DataToSend.Add("PaymentMethodId", paymentDetails.Id.ToString());
			DataToSend.Add("PaymentMethodName", paymentDetails.Name);

			// Update payment method name on server
			responseMessage = await this._ServerCommunication.SendPostRequestToServer("User/UserDetails/Payment/UpdatePaymentMethodName", DataToSend);

			responseData = ServerCommunication.ParseServerResponse<ServerResponseBool>(responseMessage);

			return responseData;
		}






		public async Task<ServerResponseListOfPaymentDetails> GetPaymentDetailsAsync(int paymentMethodInfoId)
		{
			ServerResponse responseMessage;
			ServerResponseListOfPaymentDetails responseData;

			Dictionary<string, string> DataToSend = new Dictionary<string, string>();

			DataToSend.Add("paymentMethodInfoId", paymentMethodInfoId.ToString());

			responseMessage = await this._ServerCommunication.SendPostRequestToServer("User/UserDetails/Payment/GetPaymentDetails", DataToSend);
			responseData = ServerCommunication.ParseServerResponse<ServerResponseListOfPaymentDetails>(responseMessage);

			return responseData;
		}

		public async Task<ServerResponseSinglePaymentDetail> AddPaymentDetail(PaymetDetail paymetDetail)
		{
			ServerResponse responseMessage;
			ServerResponseSinglePaymentDetail responseData;

			Dictionary<string, string> DataToSend = new Dictionary<string, string>();

			DataToSend.Add("PaymentMethodId", paymetDetail.PaymentMethodId.ToString());
			DataToSend.Add("PaymentDetailKey", paymetDetail.Key);
			DataToSend.Add("PaymentDetailValue", paymetDetail.Value == null ? string.Empty : paymetDetail.Value);

			// Add a payment detail to the payment method on the server
			responseMessage = await this._ServerCommunication.SendPostRequestToServer("User/UserDetails/Payment/AddPaymentDetail", DataToSend);

			responseData = ServerCommunication.ParseServerResponse<ServerResponseSinglePaymentDetail>(responseMessage);

			return responseData;
		}


		public async Task<ServerResponseSinglePaymentDetail> UpdatePaymentDetails(PaymetDetail paymentDetail)
		{
			ServerResponse responseMessage;
			ServerResponseSinglePaymentDetail responseData;

			Dictionary<string, string> DataToSend = new Dictionary<string, string>();

			DataToSend.Add("PaymentDetailsId", paymentDetail.Id.ToString());
			DataToSend.Add("PaymentDetailKey", paymentDetail.Key);
			DataToSend.Add("PaymentDetailValue", paymentDetail.Value == null ? string.Empty : paymentDetail.Value);

			// update payment detail on the server
			responseMessage = await this._ServerCommunication.SendPostRequestToServer("User/UserDetails/Payment/UpdatePaymentDetails", DataToSend);

			responseData = ServerCommunication.ParseServerResponse<ServerResponseSinglePaymentDetail>(responseMessage);

			return responseData;
		}

		public async Task<ServerResponseBool> DeletePaymentDetail(PaymetDetail paymentDetail)
		{
			ServerResponse responseMessage;
			ServerResponseBool responseData;

			Dictionary<string, string> DataToSend = new Dictionary<string, string>();

			DataToSend.Add("PaymentDetailsId", paymentDetail.Id.ToString());

			// remove payment details on the server
			responseMessage = await this._ServerCommunication.SendPostRequestToServer("User/UserDetails/Payment/DeletePaymentDetail", DataToSend);

			responseData = ServerCommunication.ParseServerResponse<ServerResponseBool>(responseMessage);

			return responseData;

		}
	}
}
