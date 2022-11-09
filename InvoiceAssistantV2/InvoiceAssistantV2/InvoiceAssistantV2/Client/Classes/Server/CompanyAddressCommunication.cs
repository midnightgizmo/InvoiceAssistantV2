using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Shared.Models.Database.Company;
using System.ComponentModel;

namespace InvoiceAssistantV2.Client.Classes.Server
{
    public class CompanyAddressCommunication
    {
        private HttpClient _HttpClient;
        private ServerCommunication _ServerCommunication;
        public CompanyAddressCommunication(HttpClient httpClient, AppSettings appSettings)
        {
            this._HttpClient = httpClient;
            this._ServerCommunication = new ServerCommunication(this._HttpClient, appSettings);
        }

        public async Task<ServerResponseListOfCompanyAddresses> GetListOfAddressesForCompany(int CompanyId)
        {
            ServerResponse responseMessage;         
            Dictionary<string, string> DataToSend = new Dictionary<string, string>();

            DataToSend.Add("CompanyId", CompanyId.ToString());

            // request all addresses assoshiated with company
            responseMessage = await this._ServerCommunication.SendPostRequestToServer("CompanyAddress/AddressesForCopmany", DataToSend);

            return ServerCommunication.ParseServerResponse<ServerResponseListOfCompanyAddresses>(responseMessage);

        }

        public async Task<ServerResponseSingleCompanyAddress> GetCompanyAddress(int CompanyAddressId)
        {
            ServerResponse responseMessage;
            Dictionary<string, string> DataToSend = new Dictionary<string, string>();

            DataToSend.Add("CompanyAddressId", CompanyAddressId.ToString());

            // request the company address details from the server
            responseMessage = await this._ServerCommunication.SendPostRequestToServer("CompanyAddress/Find",DataToSend);

            return ServerCommunication.ParseServerResponse<ServerResponseSingleCompanyAddress>(responseMessage);
        }
        public async Task<ServerResponseSingleCompanyAddress> AddNewCompanyAddress(CompanyAddress company)
        {
            ServerResponse responseMessage;
            Dictionary<string,string> DataToSend = new Dictionary<string, string>();

            DataToSend.Add(nameof(CompanyAddress.FriendlyName), company.FriendlyName);
            DataToSend.Add(nameof(CompanyAddress.CompanyDetailsID), company.CompanyDetailsID.ToString());
            DataToSend.Add(nameof(CompanyAddress.DrivingDistanceToAddress),company.DrivingDistanceToAddress.ToString());
            DataToSend.Add(nameof(CompanyAddress.AddressLine1), company.AddressLine1 ?? "");
            DataToSend.Add(nameof(CompanyAddress.AddressLine2), company.AddressLine2 ?? "");
            DataToSend.Add(nameof(CompanyAddress.AddressLine3), company.AddressLine3 ?? "");
            DataToSend.Add(nameof(CompanyAddress.AddressLine4), company.AddressLine4 ?? "");
            DataToSend.Add(nameof(CompanyAddress.AddressLine5), company.AddressLine5 ?? "");
            DataToSend.Add(nameof(CompanyAddress.PostCode), company.PostCode ?? "");

            // send the data to the server to be processed
            responseMessage = await this._ServerCommunication.SendPostRequestToServer("CompanyAddress/Insert", DataToSend);

            return ServerCommunication.ParseServerResponse<ServerResponseSingleCompanyAddress>(responseMessage);
        }

        public async Task<ServerResponseSingleCompanyAddress> EditCompanyAddressDetails(CompanyAddress CompanyAddressToEdit)
        {
            ServerResponse responseMessage;
            Dictionary<string, string> DataToSend = new Dictionary<string, string>();

            DataToSend.Add(nameof(CompanyAddress.Id),CompanyAddressToEdit.Id.ToString());
            DataToSend.Add(nameof(CompanyAddress.CompanyDetailsID),CompanyAddressToEdit.CompanyDetailsID.ToString());
            DataToSend.Add(nameof(CompanyAddress.FriendlyName), CompanyAddressToEdit.FriendlyName);
            DataToSend.Add(nameof(CompanyAddress.AddressLine1), CompanyAddressToEdit.AddressLine1 ?? String.Empty);
            DataToSend.Add(nameof(CompanyAddress.AddressLine2), CompanyAddressToEdit.AddressLine2 ?? String.Empty);
            DataToSend.Add(nameof(CompanyAddress.AddressLine3), CompanyAddressToEdit.AddressLine3 ?? String.Empty);
            DataToSend.Add(nameof(CompanyAddress.AddressLine4), CompanyAddressToEdit.AddressLine4 ?? String.Empty);
            DataToSend.Add(nameof(CompanyAddress.AddressLine5), CompanyAddressToEdit.AddressLine5 ?? String.Empty);
            DataToSend.Add(nameof(CompanyAddress.PostCode), CompanyAddressToEdit.PostCode ?? String.Empty);
            DataToSend.Add(nameof(CompanyAddress.DrivingDistanceToAddress), CompanyAddressToEdit.DrivingDistanceToAddress.ToString());

            responseMessage = await this._ServerCommunication.SendPostRequestToServer("CompanyAddress/Edit", DataToSend);

            return ServerCommunication.ParseServerResponse<ServerResponseSingleCompanyAddress>(responseMessage);
        }

        public async Task<ServerResponseBool> DeleteCompanyAddress(int CompanyAddressId)
        {
            ServerResponse responseMessage;
            Dictionary<string, string> DataToSend = new Dictionary<string, string>();

            DataToSend.Add("CompanyAddressId", CompanyAddressId.ToString());

            // request to remove the company address from the database
            responseMessage = await this._ServerCommunication.SendPostRequestToServer("CompanyAddress/Remove", DataToSend);

            return ServerCommunication.ParseServerResponse<ServerResponseBool>(responseMessage);
        }
    }
}
