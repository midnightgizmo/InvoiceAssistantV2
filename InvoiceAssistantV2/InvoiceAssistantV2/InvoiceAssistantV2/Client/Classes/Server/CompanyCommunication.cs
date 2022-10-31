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

        public async Task<ServerResponseBool> DeleteCompany(int CompanyID)
        {
            ServerResponse responseMessage;
            ServerResponseBool responseData;

            Dictionary<string, string> DataToSend = new Dictionary<string, string>();

            DataToSend.Add("CompanyId", CompanyID.ToString());

            // remove the company from the server
            responseMessage = await this._ServerCommunication.SendPostRequestToServer("Company/Remove", DataToSend);

            responseData = ServerCommunication.ParseServerResponse<ServerResponseBool>(responseMessage);

            return responseData;
        }
    }
}
