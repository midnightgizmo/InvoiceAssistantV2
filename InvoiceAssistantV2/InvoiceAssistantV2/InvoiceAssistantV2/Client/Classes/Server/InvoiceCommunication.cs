using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Shared.Models;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InvoiceAssistantV2.Client.Classes.Server
{
	public class InvoiceCommunication
	{
        private HttpClient _HttpClient;
        private ServerCommunication _ServerCommunication;
        public InvoiceCommunication(HttpClient httpClient, AppSettings appSettings)
        {
            this._HttpClient = httpClient;
            this._ServerCommunication = new ServerCommunication(this._HttpClient, appSettings);
        }

        public async Task<ServerResponseListOfInvoices> GetFilterdInvoiceList(InvoiceSearchParameters invoiceSearchParameters)
        {
            ServerResponse responseMessage;
            ServerResponseListOfInvoices responseData;

            Dictionary<string, string> DataToSend = new Dictionary<string, string>();

            // start date
            if (invoiceSearchParameters.StartDate == null)
                DataToSend.Add(nameof(InvoiceSearchParameters.StartDate), "");
            else
                DataToSend.Add(nameof(InvoiceSearchParameters.StartDate), invoiceSearchParameters.StartDate.ToString());

            // end date.
            if (invoiceSearchParameters.EndDate == null)
                DataToSend.Add(nameof(InvoiceSearchParameters.EndDate), "");
            else
                DataToSend.Add(nameof(InvoiceSearchParameters.EndDate), invoiceSearchParameters.EndDate.ToString());




            // StartAmmount.
            if (invoiceSearchParameters.StartAmmount == null)
                DataToSend.Add(nameof(InvoiceSearchParameters.StartAmmount), "");
            else
                DataToSend.Add(nameof(InvoiceSearchParameters.StartAmmount), invoiceSearchParameters.StartAmmount.ToString());

            // EndAmmount.
            if (invoiceSearchParameters.EndAmmount == null)
                DataToSend.Add(nameof(InvoiceSearchParameters.EndAmmount), "");
            else
                DataToSend.Add(nameof(InvoiceSearchParameters.EndAmmount), invoiceSearchParameters.EndAmmount.ToString());



            // DateRecievedMoneyStart.
            if (invoiceSearchParameters.DateRecievedMoneyStart == null)
                DataToSend.Add(nameof(InvoiceSearchParameters.DateRecievedMoneyStart), "");
            else
                DataToSend.Add(nameof(InvoiceSearchParameters.DateRecievedMoneyStart), invoiceSearchParameters.DateRecievedMoneyStart.ToString());

            // DateRecievedMoneyEnd.
            if (invoiceSearchParameters.DateRecievedMoneyEnd == null)
                DataToSend.Add(nameof(InvoiceSearchParameters.DateRecievedMoneyEnd), "");
            else
                DataToSend.Add(nameof(InvoiceSearchParameters.DateRecievedMoneyEnd), invoiceSearchParameters.DateRecievedMoneyEnd.ToString());




            // ReferenceNumber.
            if (invoiceSearchParameters.ReferenceNumber == null)
                DataToSend.Add(nameof(InvoiceSearchParameters.ReferenceNumber), "");
            else
                DataToSend.Add(nameof(InvoiceSearchParameters.ReferenceNumber), invoiceSearchParameters.ReferenceNumber.ToString());



            // TypeOfPaymentId.
            if (invoiceSearchParameters.TypeOfPaymentId == null)
                DataToSend.Add(nameof(InvoiceSearchParameters.TypeOfPaymentId), "");
            else
                DataToSend.Add(nameof(InvoiceSearchParameters.TypeOfPaymentId), invoiceSearchParameters.TypeOfPaymentId.ToString());



            // AddressToMakePaymentOutToId.
            if (invoiceSearchParameters.AddressToMakePaymentOutToId == null)
                DataToSend.Add(nameof(InvoiceSearchParameters.AddressToMakePaymentOutToId), "");
            else
                DataToSend.Add(nameof(InvoiceSearchParameters.AddressToMakePaymentOutToId), invoiceSearchParameters.AddressToMakePaymentOutToId.ToString());




            // Description.
            if (invoiceSearchParameters.Description == null)
                DataToSend.Add(nameof(InvoiceSearchParameters.Description), "");
            else
                DataToSend.Add(nameof(InvoiceSearchParameters.Description), invoiceSearchParameters.Description.ToString());


            // request all candidates that match the search criteria
            responseMessage = await this._ServerCommunication.SendPostRequestToServer("Invoice/Search", DataToSend);

            responseData = ServerCommunication.ParseServerResponse<ServerResponseListOfInvoices>(responseMessage);

            return responseData;

        }

        public async Task<ServerResponseSingleInvoice> AddNewInvoice(Invoice newInvoice)
        {
			ServerResponse responseMessage;
			

			Dictionary<string, string> DataToSend = new Dictionary<string, string>();

            DataToSend.Add(nameof(Invoice.DateOfInvoice), newInvoice.DateOfInvoice.ToString());
            DataToSend.Add(nameof(Invoice.ReferenceNumber), newInvoice.ReferenceNumber);
            DataToSend.Add(nameof(Invoice.Description), newInvoice.Description);
            DataToSend.Add(nameof(Invoice.AddressToMakeInvoiceOutToId), newInvoice.AddressToMakeInvoiceOutToId.ToString());
            
            for(int index = 0; index < newInvoice.PlacesVisitedForInvoice.Count; index++)
            {
                var visitedAddress = newInvoice.PlacesVisitedForInvoice[index];
                //DataToSend.Add($"IdsOfVisitedAddresses[{index}]", visitedAddress.CompanyAddressId.ToString());
                DataToSend.Add($"PlacesVisited[{index}].{nameof(PlacesVisitedForInvoice.CompanyAddressId)}", visitedAddress.CompanyAddressId.ToString());
                DataToSend.Add($"PlacesVisited[{index}].{nameof(PlacesVisitedForInvoice.NumberOfTimesVisited)}", visitedAddress.NumberOfTimesVisited.ToString());
			}
            
            responseMessage = await this._ServerCommunication.SendPostRequestToServer("Invoice/InsertNewInvoice", DataToSend);
            
            return ServerCommunication.ParseServerResponse<ServerResponseSingleInvoice>(responseMessage);
            
		}

        /// <summary>
        /// Asks the server to delete an invoice from the database
        /// </summary>
        /// <param name="InvoiceId">The Id of the invoice to look for and remove</param>
        /// <returns>true if deleted, else false</returns>
        public async Task<ServerResponseBool> DeleteInvoice(int InvoiceId)
        {
			ServerResponse responseMessage;


			Dictionary<string, string> DataToSend = new Dictionary<string, string>();
            DataToSend.Add("InvoiceId", InvoiceId.ToString());

            responseMessage = await this._ServerCommunication.SendPostRequestToServer("Invoice/Delete", DataToSend);
            return ServerCommunication.ParseServerResponse<ServerResponseBool>(responseMessage);
		}

        public async Task<ServerResponseSingleInvoicePayment> AddInvoicePayment(int InvoiceID, string Description, decimal Ammount)
        {
            ServerResponse responseMessage;

            Dictionary<string, string> DataToSend = new Dictionary<string, string>();

            DataToSend.Add("InvoiceID", InvoiceID.ToString());
            DataToSend.Add("Description", Description);
            DataToSend.Add("Ammount", Ammount.ToString());

			responseMessage = await this._ServerCommunication.SendPostRequestToServer("Invoice/AddPayment", DataToSend);

            return ServerCommunication.ParseServerResponse<ServerResponseSingleInvoicePayment>(responseMessage);
        }

        public async Task<ServerResponseBool> RemovePaymentFromInvoice(int InvoicePaymentId)
        {
			ServerResponse responseMessage;

			Dictionary<string, string> DataToSend = new Dictionary<string, string>();

			DataToSend.Add("InvoicePaymentId", InvoicePaymentId.ToString());

			responseMessage = await this._ServerCommunication.SendPostRequestToServer("Invoice/RemovePayment", DataToSend);

			return ServerCommunication.ParseServerResponse<ServerResponseBool>(responseMessage);
		}

        public async Task<ServerResponsePaymentTypes> GetPaymentTypes()
        {
            ServerResponse responseMessage;
            ServerResponsePaymentTypes responseData;


            // request all candidates that match the search criteria
            responseMessage = await this._ServerCommunication.SendGetRequestToServer("Invoice/Payment");

            responseData = ServerCommunication.ParseServerResponse<ServerResponsePaymentTypes>(responseMessage);

            return responseData;
        }

        /// <summary>
        /// Asks the server to generate a unique reference number for the given date
        /// </summary>
        /// <param name="DateOfInvoice">Date to use to generate reference number</param>
        /// <returns></returns>
        public async Task<ServerResponseString> GenerateInvoiceReferenceNumber(DateTime DateOfInvoice)
        {
            ServerResponse responseMessage;

            Dictionary<string, string> DataToSend = new Dictionary<string, string>();
            
            DataToSend.Add("InvoiceDate", DateOfInvoice.ToString());
            
            responseMessage = await this._ServerCommunication.SendPostRequestToServer("Invoice/GenerateReferenceNumber", DataToSend);

            return ServerCommunication.ParseServerResponse<ServerResponseString>(responseMessage);
        }
    }
}
