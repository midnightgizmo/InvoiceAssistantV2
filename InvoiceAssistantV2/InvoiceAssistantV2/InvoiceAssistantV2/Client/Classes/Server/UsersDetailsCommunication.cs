using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Shared.Models.Database.User;

namespace InvoiceAssistantV2.Client.Classes.Server
{
	public class UsersDetailsCommunication
	{
		private HttpClient _HttpClient;
		private ServerCommunication _ServerCommunication;
		public UsersDetailsCommunication(HttpClient httpClient, AppSettings appSettings)
		{
			this._HttpClient = httpClient;
			this._ServerCommunication = new ServerCommunication(this._HttpClient, appSettings);
		}

		/// <summary>
		/// Gets the Users details from the server, if they don't exist they are created and returned
		/// </summary>
		/// <returns></returns>
		public async Task<ServerResponseSingleUserDetails> GetUserDetailsAsync() 
		{
			ServerResponse responseMessage;
			ServerResponseSingleUserDetails responseData;

			Dictionary<string, string> DataToSend = new Dictionary<string, string>();

			DataToSend.Add("includePaymentDetails", "true");

			// request all candidates that match the search criteria
			responseMessage = await this._ServerCommunication.SendPostRequestToServer("User/UserDetails/GetUsersDetails/", DataToSend);

			responseData = ServerCommunication.ParseServerResponse<ServerResponseSingleUserDetails>(responseMessage);

			return responseData;
		}

		/// <summary>
		/// Update the server with the passed in models data
		/// </summary>
		/// <param name="usersDetails">the model to be updated on the server</param>
		/// <returns></returns>
		public async Task<ServerResponseSingleUserDetails> UpdateUsersDetailsAsync(UserDetails usersDetails)
		{
			ServerResponse responseMesssage;
			ServerResponseSingleUserDetails responseData;

			Dictionary<string, string> DataToSend = new Dictionary<string, string>();

			DataToSend.Add(nameof(UserDetails.Id), usersDetails.Id.ToString());
			DataToSend.Add(nameof(UserDetails.UsersName), usersDetails.UsersName.ToString());

			responseMesssage = await this._ServerCommunication.SendPostRequestToServer("User/UserDetails/UpdateUsersDetails/", DataToSend);

			responseData = ServerCommunication.ParseServerResponse<ServerResponseSingleUserDetails>(responseMesssage);

			return responseData;
		}
	}
}
