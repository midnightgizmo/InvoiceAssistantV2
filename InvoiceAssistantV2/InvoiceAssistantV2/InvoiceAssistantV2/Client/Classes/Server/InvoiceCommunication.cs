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

        public async Task<ServerResponseSingleInvoice> UpdateInvoice(int InvoiceId, DateTime DateOfInvoice, string ReferenceNumber,
                                                                     string Description, int? PaymentTypeID, int? AddressToMakeInvoiceOutToId,
                                                                     DateTime? DateRecievedMoney)
        {
			ServerResponse responseMessage;


			Dictionary<string, string> DataToSend = new Dictionary<string, string>();

			DataToSend.Add(nameof(Invoice.Id), InvoiceId.ToString());
			DataToSend.Add(nameof(Invoice.DateOfInvoice), DateOfInvoice.ToString());
			DataToSend.Add(nameof(Invoice.ReferenceNumber), ReferenceNumber);
			DataToSend.Add(nameof(Invoice.Description), Description);
            DataToSend.Add(nameof(Invoice.PaymentTypeID), PaymentTypeID != null ? PaymentTypeID.ToString() : "0");
			DataToSend.Add(nameof(Invoice.AddressToMakeInvoiceOutToId), AddressToMakeInvoiceOutToId != null ? AddressToMakeInvoiceOutToId.ToString() : "0");
            DataToSend.Add(nameof(Invoice.DateRecievedMoney),DateRecievedMoney == null ? "" : DateRecievedMoney.ToString());

            responseMessage = await this._ServerCommunication.SendPostRequestToServer("Invoice/UpdateMainDetails", DataToSend);

            return ServerCommunication.ParseServerResponse<ServerResponseSingleInvoice>(responseMessage);
		}

        /// <summary>
        /// Updates the address to make invoice out to on the server
        /// </summary>
        /// <param name="InvoiceId">The invoice id to look for to apply address to</param>
        /// <param name="AddressToMakeInvoiceOutToId">the new address id to apply to invoice, passing null is allowed</param>
        /// <returns>true if sucsefull, else false</returns>
		public async Task<ServerResponseBool> UpdateAddressInvoiceMadeOutTo(int InvoiceId, int? AddressToMakeInvoiceOutToId)
		{
            ServerResponse responseMessage;

            Dictionary<string,string> DataToSend = new Dictionary<string, string>();

            DataToSend.Add(nameof(InvoiceId), InvoiceId.ToString());
            DataToSend.Add(nameof(Invoice.AddressToMakeInvoiceOutToId), AddressToMakeInvoiceOutToId != null ? AddressToMakeInvoiceOutToId.ToString() : "");

            responseMessage = await this._ServerCommunication.SendPostRequestToServer("Invoice/UpdateAddressInvoiceMadeOutTo", DataToSend);

            return ServerCommunication.ParseServerResponse<ServerResponseBool>(responseMessage);
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

        public async Task<ServerResponseListOfInvoicePayments> ListAllPaymentsForInvoice(int InvoiceId)
        {
            ServerResponse responseMessage;

            Dictionary<string, string> DataToSend = new Dictionary<string, string>();

            DataToSend.Add("InvoiceId", InvoiceId.ToString());

            responseMessage = await this._ServerCommunication.SendPostRequestToServer("Invoice/ListAllPaymentsForInvoice", DataToSend);

            return ServerCommunication.ParseServerResponse<ServerResponseListOfInvoicePayments>(responseMessage);
        }

        /// <summary>
        /// Gets a lit of all possible payment types from server
        /// </summary>
        /// <returns></returns>
        public async Task<ServerResponsePaymentTypes> GetPaymentTypes()
        {
            ServerResponse responseMessage;
            ServerResponsePaymentTypes responseData;


            // request all candidates that match the search criteria
            responseMessage = await this._ServerCommunication.SendGetRequestToServer("Invoice/Payment");

            responseData = ServerCommunication.ParseServerResponse<ServerResponsePaymentTypes>(responseMessage);

            return responseData;
        }

        public async Task<ServerResponseListOfPlacesVisitedForInvoice> GetPlacesVisitedForInvoice(int InvoiceId)
        {
            ServerResponse responseMessage;
            ServerResponseListOfPlacesVisitedForInvoice responseData;

			Dictionary<string, string> DataToSend = new Dictionary<string, string>();

			DataToSend.Add("InvoiceId", InvoiceId.ToString());

            // request all places visited for passed in invoice id
            responseMessage = await this._ServerCommunication.SendPostRequestToServer("Invoice/ListPlacesVisited", DataToSend);

            responseData = ServerCommunication.ParseServerResponse<ServerResponseListOfPlacesVisitedForInvoice>(responseMessage);

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

        /// <summary>
        /// Gets the total amount that should be paid for an invoice (sum of all the payments on the invoice)
        /// </summary>
        /// <param name="InvoiceId">The invoice to look for</param>
        /// <returns>total amount to pay</returns>
        public async Task<ServerResponseDecimal> GetInvoiceTotalPaymentAmount(int InvoiceId)
        {
            ServerResponse responseMessage;
            Dictionary<string,string> DataToSend = new Dictionary<string,string>();

            DataToSend.Add("InvoiceId",InvoiceId.ToString());

            responseMessage = await this._ServerCommunication.SendPostRequestToServer("Invoice/GetInvoiceTotalPaymentAmount", DataToSend);

            return ServerCommunication.ParseServerResponse<ServerResponseDecimal>(responseMessage);
        }

		public async Task<ServerResponseSinglePlaceVisitedForInvoice> AddPlaceVisited(int InvoiceId, int AddressID, int NoTimesVisited)
		{
            ServerResponse responseMessage;

            Dictionary<string, string> DataToSend = new Dictionary<string, string>();

            DataToSend.Add("InvoiceId", InvoiceId.ToString());
            DataToSend.Add("AddressId", AddressID.ToString());
            DataToSend.Add("NoTimesVisited", NoTimesVisited.ToString());

            responseMessage = await this._ServerCommunication.SendPostRequestToServer("Invoice/AddPlaceVisited", DataToSend);

            return ServerCommunication.ParseServerResponse<ServerResponseSinglePlaceVisitedForInvoice>(responseMessage);
		}

		public async Task<ServerResponseBool> RemoveAllVisitedAddressFromInvoice(int InvoiceId)
		{
			ServerResponse responseMessage;

			Dictionary<string, string> DataToSend = new Dictionary<string, string>();

			DataToSend.Add("InvoiceId", InvoiceId.ToString());


			responseMessage = await this._ServerCommunication.SendPostRequestToServer("Invoice/RemoveAllPlacesVisited", DataToSend);

			return ServerCommunication.ParseServerResponse<ServerResponseBool>(responseMessage);
		}

        public async Task<ServerResponseBool> RemoveVisitedAddressFromInvoice(int InvoiceId, int AddressId)
        {
            ServerResponse responseMessage;

            Dictionary<string,string> DataToSend = new Dictionary<string, string>();

            DataToSend.Add("InvoiceId", InvoiceId.ToString());
            DataToSend.Add("AddressId", AddressId.ToString());

            responseMessage = await this._ServerCommunication.SendPostRequestToServer("Invoice/RemoveVisitedAddressFromInvoice",DataToSend);

            return ServerCommunication.ParseServerResponse<ServerResponseBool>(responseMessage);

        }

        
		public async Task<ServerResponseSingleInvoicePayment> UpdateInvoicePaymentRow(int InvoicePaymentId, string Description, decimal Ammount)
		{
			ServerResponse responseMessage;

			Dictionary<string, string> DataToSend = new Dictionary<string, string>();

			DataToSend.Add("InvoicePaymentId", InvoicePaymentId.ToString());
			DataToSend.Add("Description", Description);
			DataToSend.Add("Ammount", Ammount.ToString());

			responseMessage = await this._ServerCommunication.SendPostRequestToServer("Invoice/EditPayment", DataToSend);

			return ServerCommunication.ParseServerResponse<ServerResponseSingleInvoicePayment>(responseMessage);
		}
	}
}
