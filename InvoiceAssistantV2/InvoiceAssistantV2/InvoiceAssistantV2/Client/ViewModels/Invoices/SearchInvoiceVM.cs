using InvoiceAssistantV2.Client.Classes.Server;
using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Shared.Models;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Client.ViewModels.Invoices
{
	public class SearchInvoiceVM
	{
        public InvoiceSearchParameters FormModel { get; set; } = new InvoiceSearchParameters();
        /// <summary>
        /// Any error that occur on the page will go in this string
        /// </summary>
        public string PageErrorText { get; set; } = string.Empty;

        public List<PaymentType> PaymentTypes { get; set; }


        public async Task<ServerResponseListOfInvoices> SendSearchRquestToServer(HttpClient httpClient, AppSettings appSettings)
        {
            InvoiceCommunication server = new InvoiceCommunication(httpClient, appSettings);
            
            return await server.GetFilterdInvoiceList(this.FormModel);
        }

        /// <summary>
        /// Returns true if errors were found from the server response data. Also Adds
        /// the error message to the <see cref="PageErrorText"/> property
        /// </summary>
        /// <param name="ResponseData"></param>
        /// <returns></returns>
        public bool AreThereErrorsInResponseData(ServerResponseListOfInvoices ResponseData)
        {
            this.PageErrorText = string.Empty;
            if(ResponseData.HasErrors == true)
            {

                foreach (string anError in ResponseData.Errors)
                    this.PageErrorText += anError + " : ";
            }

            return ResponseData.HasErrors;
        }
    }
}
