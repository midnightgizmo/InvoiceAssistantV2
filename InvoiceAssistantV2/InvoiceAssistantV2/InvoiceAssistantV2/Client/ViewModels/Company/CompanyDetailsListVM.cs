using InvoiceAssistantV2.Client.Classes.Server;
using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Shared.Models.Database.Company;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using System.Runtime.CompilerServices;

namespace InvoiceAssistantV2.Client.ViewModels.Company
{
    public class CompanyDetailsListVM
    {
        private HttpClient _HttpClient;
        private AppSettings _AppSettings;



        /// <summary>
        /// Holds a list of all companys
        /// </summary>
        private List<CompanyDetails> CompanyDetails;
        /// <summary>
        /// Holds a filterd list of companys, based on users search parameters
        /// </summary>
        

        public CompanyDetailsListVM(HttpClient httpClient, AppSettings appSettings)
        {
            this._HttpClient = httpClient;
            this._AppSettings = appSettings;


        }

        /// <summary>
        /// List of company details to display on the web page
        /// </summary>
        public List<CompanyDetails> FilterdCompanyDetails { get; set; }

        /// <summary>
        /// Filter the<see cref="FilterdCompanyDetails"/> based on the passed in serch criteria.
        /// Passing String.Empty to both input parameters will result in a full list populated.
        /// </summary>
        /// <param name="FriendlyName"></param>
        /// <param name="CompanyName"></param>
        public void FilterCompanyDetails(string FriendlyName, string CompanyName)
        {
            // get a list of all the companies
            IEnumerable<CompanyDetails> query = this.CompanyDetails;

            // if we have some text in FriendlyName, filter by FriendlyName
            if (FriendlyName.Length > 0)
                query = query.Where(c => c.FriendlyName.Contains(FriendlyName,StringComparison.OrdinalIgnoreCase));

            // If we have some text in CompanyName, filter by Company Name
            if (CompanyName.Length > 0)
                query = query.Where(c => c.CompanyName.Contains(CompanyName, StringComparison.OrdinalIgnoreCase));

            // Sort the filterd results alphabeticly by FriendlyName
            query = query.OrderBy(o => o.FriendlyName);

            // Clear the current filterd results
            this.FilterdCompanyDetails.Clear();

            // Add the new Filterd results
            this.FilterdCompanyDetails.AddRange(query);
        }

        /// <summary>
        /// Load a list of Companys from the server
        /// </summary>
        /// <returns></returns>
        public async Task LoadCompanyDetailsFromServerAsync()
        {
            CompanyCommunication companyCommunication = new CompanyCommunication(this._HttpClient, this._AppSettings);
            ServerResponseListOfCompanys responseData;

            responseData = await companyCommunication.GetListOfCompanys();

            // if we were unable to get a list of Company details, inishalize an empty list of Company Details
            if (responseData.HasErrors == true)
                this.CompanyDetails = new List<CompanyDetails>();
            // Get the list of Compnay Details we recieved from the server
            else
                this.CompanyDetails = responseData.ReturnValue;

            // Set the FilterdList to a list of all the company details.
            this.FilterdCompanyDetails = new List<CompanyDetails>(this.CompanyDetails);
        }


        public void RemoveCompanyFromList(CompanyDetails company)
        {
            this.CompanyDetails.Remove(company);
            this.FilterdCompanyDetails.Remove(company);
        }
    }
}
