using InvoiceAssistantV2.Client.Classes.Server;
using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Shared.Models;
using InvoiceAssistantV2.Shared.Models.Database.Company;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using System.Runtime.CompilerServices;

namespace InvoiceAssistantV2.Client.ViewModels.Invoices
{
	public class SearchInvoiceVM
	{
        private HttpClient _HttpClient;
        private AppSettings _AppSettings;

        public InvoiceSearchParameters FormModel { get; set; } = new InvoiceSearchParameters();
        /// <summary>
        /// Any error that occur on the page will go in this string
        /// </summary>
        public string PageErrorText { get; set; } = string.Empty;

        public List<PaymentType> PaymentTypes { get; set; }

        // List of all companies
        public List<CompanyDetails> CompanyDetails { get; set; }

        public  SearchInvoiceVM(HttpClient httpClient, AppSettings appSettings)
        {
            this._HttpClient = httpClient;
            this._AppSettings = appSettings;

            
        }

        public async Task<ServerResponseListOfInvoices> SendSearchRquestToServer()
        {
            InvoiceCommunication server = new InvoiceCommunication(this._HttpClient, this._AppSettings);
            
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

        /// <summary>
        /// Loads a list of all Company and Payment types from the server
        /// </summary>
        /// <returns></returns>
        public async Task LoadPaymentAndCompanyDetailsFromServer()
        {
            Task<ServerResponsePaymentTypes> paymentTask = this.LoadPaymentTypesFromServerAsync();
            Task< ServerResponseListOfCompanys> companyDetailsTask = this.LoadCompanyDetailsFromServerAsync();

            await Task.WhenAll(paymentTask, companyDetailsTask);

            if (paymentTask.Result.HasErrors == true)
                this.PaymentTypes = new List<PaymentType>();
            else
                this.PaymentTypes = paymentTask.Result.ReturnValue;

            if (companyDetailsTask.Result.HasErrors == true)
                this.CompanyDetails = new List<CompanyDetails>();
            else
                this.CompanyDetails = companyDetailsTask.Result.ReturnValue;

            return;
        }

        /// <summary>
        /// Loads a list of all Payment types from the server
        /// </summary>
        /// <returns></returns>
        private Task<ServerResponsePaymentTypes> LoadPaymentTypesFromServerAsync()
        {
            InvoiceCommunication invoiceCommunication = new InvoiceCommunication(this._HttpClient, this._AppSettings);
            //ServerResponsePaymentTypes responseData;


            //responseData = await invoiceCommunication.GetPaymentTypes();
            return invoiceCommunication.GetPaymentTypes();

            //if (responseData.HasErrors == true)
            //    this.PaymentTypes = new List<PaymentType>();
            //else
            //    this.PaymentTypes = responseData.ReturnValue;

            
        }

        /// <summary>
        /// Loads a list of Companys from the server
        /// </summary>
        /// <returns></returns>
        private Task<ServerResponseListOfCompanys> LoadCompanyDetailsFromServerAsync()
        {
            CompanyCommunication companyCommunication = new CompanyCommunication(this._HttpClient, this._AppSettings);
            //ServerResponseListOfCompanys responseData;

            //responseData = await companyCommunication.GetListOfCompanys();
            return companyCommunication.GetListOfCompanys();

            //if (responseData.HasErrors == true)
            //    this.CompanyDetails = new List<CompanyDetails>();
            //else
            //    this.CompanyDetails = responseData.ReturnValue;
        }
    }
}
