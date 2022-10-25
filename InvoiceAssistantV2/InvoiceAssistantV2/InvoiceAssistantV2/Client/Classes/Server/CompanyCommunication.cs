using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Client.Models.Server;

namespace InvoiceAssistantV2.Client.Classes.Server
{
    public class CompanyCommunication
    {
        private HttpClient _HttpClient;
        private ServerCommunication _ServerCommunication;
        public CompanyCommunication(HttpClient httpClient, AppSettings appSettings)
        {
            this._HttpClient = httpClient;
            this._ServerCommunication = new ServerCommunication(this._HttpClient, appSettings);
        }

        public async Task<ServerResponseListOfCompanys> GetListOfCompanys()
        {
            ServerResponse responseMessage;
            ServerResponseListOfCompanys responseData;


            // request all candidates that match the search criteria
            responseMessage = await this._ServerCommunication.SendGetRequestToServer("Company/ListOfCompanys");

            responseData = ServerCommunication.ParseServerResponse<ServerResponseListOfCompanys>(responseMessage);

            return responseData;
        }
    }
}
