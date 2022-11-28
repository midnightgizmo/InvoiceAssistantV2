using InvoiceAssistantV2.Client.Classes.Server;
using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Shared.Models.Database.Company;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace InvoiceAssistantV2.Client.ViewModels.Invoices.Add
{
	public class AddInvoiceMainDetailsVM
	{
		private HttpClient _HttpClient;
		private AppSettings _AppSettings;

		
		public event Action RequestStateChangeInUI = delegate { };

		public AddInvoiceMainDetailsVM()
		{
			this.InvoiceDate = DateTime.Now;
			this.ListOfCompanys = new List<CompanyDetails>();

		}

		public async Task Inishazlie(HttpClient httpClient, AppSettings appSettings)
		{
			this._HttpClient = httpClient;
			this._AppSettings= appSettings;

			await this.GetAllCompanysFromServer();
			// if we have found some companies
			if (this.ListOfCompanys.Any() == true)
				// set the first company in the list as the selected company
				this.SelectedCompanyID = this.ListOfCompanys[0].Id;

			this.CaculateReferenceNumber();
		}

		#region public properties
		private DateTime _InvoiceDate;
		public DateTime InvoiceDate
		{
			get => this._InvoiceDate;
			set
			{
				this._InvoiceDate = value;
				this.CaculateReferenceNumber();
			}
		}

		private string _ReferenceNumber = string.Empty;
		public string ReferenceNumber
		{
			get => this._ReferenceNumber;
			set
			{
				this._ReferenceNumber = value;
				this.CheckIfSubmitButtonDisabledStateNeedsToChange();
			}
		}


		private string _Description = string.Empty;
		public string Description
		{
			get => this._Description;
			set
			{
				this._Description = value;
				this.CheckIfSubmitButtonDisabledStateNeedsToChange();
			}
		}
		public List<CompanyDetails> ListOfCompanys { get; set; }

		private int _SelectedCompanyID = 0;
		public int SelectedCompanyID 
		{ 
			get => this._SelectedCompanyID;
			set
			{
				this._SelectedCompanyID = value;

				this.ListOfCompanyAddressess_PlacesVisitedSelection.Clear();
				this.ListOfCompanyAddressess.Clear();
				this.CompanyAddressessVisited.Clear();
				this.GetAllCompanyAddressesForSelectedCompany(this._SelectedCompanyID)
					.ContinueWith((Task<List<CompanyAddress>> CompanyAddrressList) => 
					{
						// add all the found addressess to the list which gets shown in the drop down list box
						// for allowing users to add places they have visited agaist invoice
						this.ListOfCompanyAddressess_PlacesVisitedSelection.AddRange(CompanyAddrressList.Result);
						// add all the found addresses to the list which lets the user select which address
						// the invoice is made out to.
						this.ListOfCompanyAddressess.AddRange(CompanyAddrressList.Result);
						// if there are some addresses, select the first one as the default to show in the drop down list
						if (this.ListOfCompanyAddressess.Any())
						{
							this.SelectedCompanyAddressID = this.ListOfCompanyAddressess[0].Id;
							this.VisitedAddress_SelectedId = this.ListOfCompanyAddressess[0].Id;
						}
						else
						{
							this.SelectedCompanyAddressID = 0;
							this.VisitedAddress_SelectedId = 0;
							// if ther are no addresses addressess assoshiated with the view, then we dont want to include miles
							this.IncludeMiles = false;
						}

						// because we have new addresses, clear any addresses that were added to the company addressess visited list
						this.CompanyAddressessVisited.Clear();
						this.RequestStateChangeInUI();
						this.CheckIfSubmitButtonDisabledStateNeedsToChange();
					});

				



			}
		}

		public List<CompanyAddress> ListOfCompanyAddressess { get; set; } = new List<CompanyAddress>();

		private int _SelectedCompanyAddressID = 0;
		public int SelectedCompanyAddressID
		{
			get => this._SelectedCompanyAddressID;
			set
			{
				this._SelectedCompanyAddressID = value;
				this.CheckIfSubmitButtonDisabledStateNeedsToChange();
			}
		}

		public List<CompanyAddress> ListOfCompanyAddressess_PlacesVisitedSelection { get; set; } = new List<CompanyAddress>();

		/// <summary>
		/// Binds to the html <select/> element for adding a place the user visited 
		/// </summary>
		public int VisitedAddress_SelectedId { get; set; } = 0;
		/// <summary>
		/// Binds to the html <input/> element that says how many times the user visited the above <see cref="VisitedAddress_SelectedId"/>
		/// </summary>
		public int VisitedAddress_NoTimesVisited { get; set; } = 1;


		
		/// <summary>
		/// List of addressess the user have visited for the invoice we are createing
		/// </summary>
		public List<AddInvoicePlaceVisitedInInvoiceVM> CompanyAddressessVisited { get; set; } = new List<AddInvoicePlaceVisitedInInvoiceVM>();

		private bool _IncludeMiles = true;
		public bool IncludeMiles
		{
			get
			{
				// return false if the main details have all been added to the database
				// (disable the IncludeMiles if details have been added to database)
				//if (this.HasMainDetailsBeenAddedToDataBase == true)
				//	return false;
				//else
				//	return this._IncludeMiles;

				return this._IncludeMiles;
			}
			set
			{
				this._IncludeMiles = value;
				// if we dont want to include mile, make sure there are no addreses visited.
				if (this._IncludeMiles == false)
				{
					// remove each address the user has selected, and add it back into the drop down list box.
					//foreach (var anAddress in this.CompanyAddressessVisited)
					//	this.RemoveAdressUserVisited(anAddress.PlaceVisited.Id);

					this.CompanyAddressessVisited.Clear();
					this.ListOfCompanyAddressess_PlacesVisitedSelection.AddRange(this.ListOfCompanyAddressess);
				}

				this.CheckIfSubmitButtonDisabledStateNeedsToChange();

			}
		}

		//public int NumberOfTimesVisited { get; set; } = 1;

		private bool _IsSubmitButtonDisabled = true;
		/// <summary>
		/// Submit button will be enabled when all a company and its address have been selected.
		/// </summary>
		public bool IsSubmitButtonDisabled
		{
			get
			{
				// return false if the main details have all been added to the database
				// (disable the IncludeMiles if details have been added to database)
				if (this.HasMainDetailsBeenAddedToDataBase == true)
					return true;
				else
					return this._IsSubmitButtonDisabled;
			}
			set
			{
				this._IsSubmitButtonDisabled = value;
			}
		}

		/// <summary>
		/// Will become true when the details from this view model has been added to the database
		/// </summary>
		public bool HasMainDetailsBeenAddedToDataBase { get; set; } = false;

		#endregion


		#region public methods

		/// <summary>
		/// Adds the selected address from <see cref="VisitedAddress_SelectedId"/> to the database
		/// </summary>
		public void AddAddressUserVisited()
		{
			// if we are adding an address that we visited, the no time visited must be at least 1
			// This will be used when caculating the number of miles we have traveld to and from the company
			if (this.VisitedAddress_NoTimesVisited < 1)
				return;
			// make sure the place we want to add has not allready been added. if it has, just ignore it
			if (this.CompanyAddressessVisited.Find(c => c.PlaceVisited.Id == this.VisitedAddress_SelectedId) != null)
				return;

			// find the place visited we want to add
			CompanyAddress? foundAddress = this.ListOfCompanyAddressess.Find(c => c.Id == this.VisitedAddress_SelectedId);

			// if we could not find the address we want to add.
			if (foundAddress == null)
				return;

			// add the new place we visited to the list
			AddInvoicePlaceVisitedInInvoiceVM newPlaceVisited = new AddInvoicePlaceVisitedInInvoiceVM();
			newPlaceVisited.NumberOfTimesVisited = this.VisitedAddress_NoTimesVisited;
			newPlaceVisited.PlaceVisited = foundAddress;

			// remove the company address drop the drop down list box so the user does not try to add it more
			// than once
			this.ListOfCompanyAddressess_PlacesVisitedSelection.Remove(foundAddress);
			if (this.ListOfCompanyAddressess_PlacesVisitedSelection.Any())
				this.VisitedAddress_SelectedId = this.ListOfCompanyAddressess_PlacesVisitedSelection[0].Id;

			// add the place visited to the list
			this.CompanyAddressessVisited.Add(newPlaceVisited);

			this.CheckIfSubmitButtonDisabledStateNeedsToChange();
		}
		/// <summary>
		/// When we want to remove a place visited from the invoice
		/// </summary>
		/// <param name="AddressID"></param>
		/// <returns></returns>
		public Task RemoveAdressUserVisited(int AddressID)
		{
			// found the address visited we want to remove
			AddInvoicePlaceVisitedInInvoiceVM? foundAddress = this.CompanyAddressessVisited.Find(c => c.PlaceVisited.Id == AddressID);
			
			// if we found it
			if (foundAddress != null)
			{
				// remove it from the list
				this.CompanyAddressessVisited.Remove(foundAddress);

				// add it back into the list for the drop down select that will allow the user to add it again
				// if they changed there mind
				this.ListOfCompanyAddressess_PlacesVisitedSelection.Add(foundAddress.PlaceVisited);
			}

			// set the selected id to the first element in the list of addresses the user can pick from
			if (this.ListOfCompanyAddressess_PlacesVisitedSelection.Any())
				this.VisitedAddress_SelectedId = this.ListOfCompanyAddressess_PlacesVisitedSelection[0].Id;

			this.CheckIfSubmitButtonDisabledStateNeedsToChange();

			return Task.CompletedTask;
		}
		public async Task<bool> SubmitInvoiceDetailsToServer()
		{
			if (this.AreErrorsPresent() == true)
				return false;

			// send data off to the server and see if the invoice was created
			InvoiceCommunication server = new InvoiceCommunication(this._HttpClient, this._AppSettings);
			ServerResponseSingleInvoice serverResponse;

			Invoice InvoiceToCreate = this.CreateInvoiceFromModel();

			serverResponse = await server.AddNewInvoice(InvoiceToCreate);

			if (serverResponse.HasErrors == true)
				return false;

			this.CreatedInvoice = serverResponse.ReturnValue;

			return true;
		}

		/// <summary>
		/// An invoice object that will be inishalized when <see cref="SubmitInvoiceDetailsToServer()"/> method is called
		/// </summary>
		public Invoice? CreatedInvoice { get; private set; } = null;

		#endregion

		#region Private methods
		/// <summary>
		/// Stops multiple hits to the server at the same time
		/// </summary>
		private bool IsReferenceCaculationInProgress = false;
		private async void CaculateReferenceNumber()
		{
			if (this._HttpClient == null || this.IsReferenceCaculationInProgress == true)
				return;

			this.IsReferenceCaculationInProgress = true;
			// go off to the server and ask what the next avalable reference number is available for the given date
			InvoiceCommunication server = new InvoiceCommunication(this._HttpClient, this._AppSettings);

			ServerResponseString serverResposne = await  server.GenerateInvoiceReferenceNumber(this.InvoiceDate);
			if (serverResposne.HasErrors == false)
			{
				this.ReferenceNumber = serverResposne.ReturnValue;
				this.RequestStateChangeInUI();
			}

			this.IsReferenceCaculationInProgress = false;
		}


		private async Task GetAllCompanysFromServer()
		{
			CompanyCommunication server = new CompanyCommunication(this._HttpClient,this._AppSettings);
			ServerResponseListOfCompanys ServerResponse;
			ServerResponse = await server.GetListOfCompanys();
			
			// if we were unable to get a list of companys
			if (ServerResponse.HasErrors)
				return;

			this.ListOfCompanys.AddRange(ServerResponse.ReturnValue);

		}

		private async Task<List<CompanyAddress>> GetAllCompanyAddressesForSelectedCompany(int CompanyDetailsID)
		{
			CompanyAddressCommunication server = new CompanyAddressCommunication(this._HttpClient, this._AppSettings);
			ServerResponseListOfCompanyAddresses ServerResponse;
			ServerResponse = await server.GetListOfAddressesForCompany(CompanyDetailsID);

			if (ServerResponse.HasErrors)
				return new List<CompanyAddress>();

			return ServerResponse.ReturnValue;
		}

		/// <summary>
		/// Enables or disables the submit button based condition of this model
		/// </summary>
		private void CheckIfSubmitButtonDisabledStateNeedsToChange()
		{
			if(this.ReferenceNumber.Trim().Length < 1)
			{
				this.IsSubmitButtonDisabled = true;
				return;
			}

			if(this.Description.Trim().Length < 1)
			{
				this.IsSubmitButtonDisabled = true;
				return;
			}

			if(this._SelectedCompanyID < 1)
			{
				this.IsSubmitButtonDisabled = true;
				return;
			}

			if(this.SelectedCompanyAddressID < 1)
			{
				this.IsSubmitButtonDisabled = true;
				return;
			}

			// if the user has requested to keep track of the miles they have traveld
			// to the invoice addresses, but they have not acutaly added any addresses to where
			// they have travelved
			if(this.IncludeMiles == true && this.CompanyAddressessVisited.Count == 0)
			{
				this.IsSubmitButtonDisabled = true;
				return;
			}

			this.IsSubmitButtonDisabled = false;
		}

		/// <summary>
		/// Checks all the details needed to create an inovoice are present
		/// </summary>
		/// <returns>true if errors found, else false</returns>
		private bool AreErrorsPresent()
		{
			if (this.ReferenceNumber.Trim().Length < 1)
				return true;

			if (this.Description.Trim().Length < 1)
				return true;

			if (this._SelectedCompanyID < 1)
				return true;

			if (this.SelectedCompanyAddressID < 1)
				return true;


			return false;
		}


		/// <summary>
		/// Creates a new Invoice instance based on the data within this viewmodel
		/// </summary>
		/// <returns>an Invoice based on data from this view model</returns>
		private Invoice CreateInvoiceFromModel()
		{
			Invoice anInvoice = new Invoice();

			anInvoice.DateOfInvoice = this._InvoiceDate;
			anInvoice.Description = this._Description;
			anInvoice.ReferenceNumber= this._ReferenceNumber;
			anInvoice.AddressToMakeInvoiceOutToId = this._SelectedCompanyAddressID;
			anInvoice.TotalInvoiceAmmount = 0;
			anInvoice.PlacesVisitedForInvoice = new List<PlacesVisitedForInvoice>();


			// go through each address that was visited and add it to the invoice.
			// NOTE: we can't add the address id because this have not been created yet (the server will creat that value)
			foreach (var visitedAddress in this.CompanyAddressessVisited)
				anInvoice.PlacesVisitedForInvoice.Add(new PlacesVisitedForInvoice() 
				{
					CompanyAddressId = visitedAddress.PlaceVisited.Id,
					NumberOfTimesVisited = visitedAddress.NumberOfTimesVisited,
					Invoice = anInvoice
				});
			return anInvoice;
		}

		/// <summary>
		/// Can only be called after <see cref="Inishazlie"/> has been called.
		/// Resets the model ready for a new invoice to be created
		/// </summary>
		internal void Reset()
		{
			this.InvoiceDate = DateTime.Now;
			this.ListOfCompanys.Clear();
			this.Description = string.Empty;
			this.SelectedCompanyID = 0;
			this.SelectedCompanyAddressID = 0;
			this.CompanyAddressessVisited.Clear();
			this.VisitedAddress_SelectedId = 0;
			this.VisitedAddress_NoTimesVisited = 1;
			this.HasMainDetailsBeenAddedToDataBase = false;
			this.CreatedInvoice = null;
		}

		#endregion
	}
}
