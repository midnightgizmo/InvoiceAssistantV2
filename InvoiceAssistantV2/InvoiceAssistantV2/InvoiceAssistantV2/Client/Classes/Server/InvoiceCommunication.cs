using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Shared.Models;
using System.ComponentModel;

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
    }
}
