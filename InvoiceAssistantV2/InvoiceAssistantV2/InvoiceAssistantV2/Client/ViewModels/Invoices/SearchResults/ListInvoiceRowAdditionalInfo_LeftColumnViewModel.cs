using InvoiceAssistantV2.Client.Classes.Server;
using InvoiceAssistantV2.Client.Models.Server;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Client.ViewModels.Invoices.Add;
using InvoiceAssistantV2.Shared.Models.Database.Company;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Client.ViewModels.Invoices.SearchResults
{
	public class ListInvoiceRowAdditionalInfo_LeftColumnViewModel
	{
		/// <summary>
		/// The view model that creates this view model
		/// </summary>
		private ListInvoiceRowAdditionalInfoViewModel _Parent;


		/// <summary>
		/// will be set to true the first time <see cref="Inishalize"/> method has been called
		/// </summary>
		private bool _HasRowBeenInishalized = false;


		public ListInvoiceRowAdditionalInfo_LeftColumnViewModel(ListInvoiceRowAdditionalInfoViewModel parent)
        {
            this._Parent = parent;
        }


		#region public methods

		/// <summary>
		/// Gets all the information from the server needed to populate the view model.
		/// Should only be called when when <see cref="IsRowSelected"/> is true
		/// </summary>
		public async Task Inishalize()
		{
			if (this._HasRowBeenInishalized == true)
				return;
			// set to stop this function running multiple times
			this._HasRowBeenInishalized = true;

			// set to true while we are gettting all the info from the server
			this.IsAddishanlDetailsInSubmitState = true;

			// find the company linked to the address the invoice is mode out to
			CompanyDetails? companyDetails = await this.FindCompanyLinkedToAddress(this.Parent.Parent.InvoiceData.AddressToMakeInvoiceOutToId);
			// if for some reason we could ont find the company details
			if (companyDetails == null)
			{
				// we were unable to inishzlie row so set to false (will allow another attempt to me made)
				this._HasRowBeenInishalized = false;
				this.IsAddishanlDetailsInSubmitState = false;
				// set to zero (because null does not seem to work on combo box's for unselected) to indicate nothing is selected
				this._SelectedCompanyId = 0;
				// update the UI with any changes we have made
				//this.Parent.Parent.CallUiNeedsUpdatingEvent();
				return;
			}
			// set the UI Drop down Combo box for the Selected company address made out to.
			// NOTE, this will require us to force a UI Update
			// We wet the private veriable instead of the get setter because there is code
			// that would run if we ran the setter, and we don't want that code to run on inishzlie
			this._SelectedCompanyId = companyDetails.Id;
			this.SelectedCompany = companyDetails;

			// find all the addresses assishated with selected company id
			List<CompanyAddress> addresses = await this.FindAllAddressessAsociatedWithCompany(this._SelectedCompanyId);
			// Add the addresses to the view model
			this.AddressessThatSelectedCompanyHas.Clear();
			this.AddressessThatSelectedCompanyHas.AddRange(addresses);

			// set the address invoice made out to in the view model
			this._AddressToMakeInvoiceOutToId = this.Parent.Parent.InvoiceData.AddressToMakeInvoiceOutToId;
			this.AddressToMakeInvoiceOutTo = this.Parent.Parent.Parent.ListOfCompanyAddressess.Where(a => a.Id == this.Parent.Parent.InvoiceData.AddressToMakeInvoiceOutToId).FirstOrDefault();


			// do we need to get the addresses that were visited for this invoice 
			// (can be used to caculate milage in reports section)
			if (this.Parent.Parent.InvoiceData.PlacesVisitedForInvoice == null)
			{
				// get the places visited from the server
				List<PlacesVisitedForInvoice> placesVisited = await this.FindVisitedAddresses(this.Parent.Parent.InvoiceData.Id);
				// add the places visited to the invoice
				this.Parent.Parent.InvoiceData.PlacesVisitedForInvoice = placesVisited;
				// if some places have been visited
				if (this.Parent.Parent.InvoiceData.PlacesVisitedForInvoice.Count > 0)
				{
					this._IncludeMiles = true;
					// For each place visited, create a view model. This will
					// allow us to list in a table the places the user has visited
					// with the ability to remove the place if the user so wishes.
					this.InishzlizeCompanyAddressessVisitedVM();
				}
				// if no places were visited
				else
				{
					this._IncludeMiles = false;
				}
			}

			// load the drop down list that contains addresses the user can select
			// to add as a place visited for this invoice.
			// (will contain all addressess assoshiated with _SelectedCompanyId minus
			// any address allready held in this.Parent.InvoiceData.PlacesVisitedForInvoice)
			this.InishalizeCombobox_AddressesCouldVisit();

			// set to false to say we have finished communicating with the server
			this.IsAddishanlDetailsInSubmitState = false;

			

		}


		/// <summary>
		/// When the user clicks the add button to add a visited adderss to the invoice
		/// </summary>
		/// <returns></returns>
		public async Task AddAddressUserVisited()
		{
			// get the information to add
			int AddressIDToAdd;
			int NoTimesVisitedAddrss;
			int InvoiceID;

			AddressIDToAdd = this.Combobox_VisitedAddress_SelectedId;
			NoTimesVisitedAddrss = this.NewPlaceVisited_NoTimesVisited;
			InvoiceID = this.Parent.Parent.InvoiceData.Id;

			// if the above values are zero or below
			if (AddressIDToAdd <= 0 || NoTimesVisitedAddrss <= 0 || InvoiceID <= 0)
				// we can't add the address because one or more of the values are wrong
				return;



			// add address to server
			InvoiceCommunication server = new InvoiceCommunication(this.Parent.Parent.httpClient, this.Parent.Parent.appSettings);
			ServerResponseSinglePlaceVisitedForInvoice result = await server.AddPlaceVisited(InvoiceID, AddressIDToAdd, NoTimesVisitedAddrss);
			// check if there were any errors on the server side
			if (result.HasErrors == true)
				// there was an error so we cant add the visited address to the UI
				return;

			// remove the address visited from this.Combobox_AddressesCouldVisit
			this.Combobox_AddressesCouldVisit.Remove(this.Combobox_AddressesCouldVisit.Where(a => a.Id == AddressIDToAdd).First());

			// add visited address to the invoice model
			this.Parent.Parent.InvoiceData.PlacesVisitedForInvoice.Add(result.ReturnValue);

			// add address to view model
			this.AddVisitedAddressTo_CompanyAddressessVisitedVM(AddressIDToAdd, NoTimesVisitedAddrss);

			// set the default selected item to the next on in the list
			if (this.Combobox_AddressesCouldVisit.Any())
				this._Combobox_VisitedAddress_SelectedId = this.Combobox_AddressesCouldVisit[0].Id;
			else
				this._Combobox_VisitedAddress_SelectedId = 0;
		}

		/// <summary>
		/// When the user clicks the remove button to remove an address that was visited to invoice
		/// </summary>
		/// <param name="AddressId"></param>
		/// <returns></returns>
		public async Task RemoveAdressUserVisited(int AddressId)
		{
			// remove address from server
			InvoiceCommunication server = new InvoiceCommunication(this.Parent.Parent.httpClient, this.Parent.Parent.appSettings);
			ServerResponseBool response = await server.RemoveVisitedAddressFromInvoice(this.Parent.Parent.InvoiceData.Id, AddressId);

			// if we were able to remove the address from the invoice at the server end
			if (response.HasErrors == false && response.ReturnValue == true)
			{
				// remove address from view model
				this.RemoveVisitedAddressFrom_CompanyAddressessVisitedVMAndInvoiceModel(AddressId);
				// get the address that was just removed from the invoice
				CompanyAddress? AddressToAddBackToList = this.Parent.Parent.Parent.ListOfCompanyAddressess.Where(a => a.Id == AddressId).FirstOrDefault();
				// Address should not be null because it should be in the cache.
				// but just to make sure, lets check anyway
				if (AddressToAddBackToList != null) 
				{
					// add the address back to the drop down list that the user can choose to add back again.
					this.Combobox_AddressesCouldVisit.Add(AddressToAddBackToList);
					// set the default selected value to the first one in the list
					this._Combobox_VisitedAddress_SelectedId = this.Combobox_AddressesCouldVisit[0].Id;
				}
			}


		}
		#endregion






		#region public properties

		/// <summary>
		/// Allows the View to have access to the parent class
		/// </summary>
		public ListInvoiceRowAdditionalInfoViewModel Parent { get => this._Parent; }



		/// <summary>
		/// If set to true we are waiting for a response from the server.
		/// </summary>
		public bool IsAddishanlDetailsInSubmitState { get; set; } = false;

		#region Address invoice made out too

		public bool IsAddressInvoiceMadeOutToBeingEdited { get; set; } = false;

		private int? _SelectedCompanyId = null;
		/// <summary>
		/// Company we are making the invoice out too
		/// </summary>
		public int? SelectedCompanyId
		{
			get => this._SelectedCompanyId;
			set
			{
				this._SelectedCompanyId = value;

				SelectedCompanyChanged();

			}
		}
		/// <summary>
		/// The <see cref="CompanyDetails"/> assoshiated with <see cref="SelectedCompanyId"/>
		/// </summary>
		public CompanyDetails? SelectedCompany { get; private set; }



		private int? _AddressToMakeInvoiceOutToId = null;
		/// <summary>
		/// Company address id we are making invoice out too
		/// </summary>
		public int? AddressToMakeInvoiceOutToId
		{
			get => _AddressToMakeInvoiceOutToId;
			set
			{
				int? oldValue = this._AddressToMakeInvoiceOutToId;
				this._AddressToMakeInvoiceOutToId = value;

				this.SelectedAddressInvoiceMadeOutToChanged(oldValue);
			}
		}



		/// <summary>
		/// The <see cref="CompanyAddress"/> assoshiated with <see cref="AddressToMakeInvoiceOutToId"/>
		/// </summary>
		public CompanyAddress? AddressToMakeInvoiceOutTo { get; private set; }

		/// <summary>
		/// Will be populated each time <see cref="SelectedCompanyId"/> changes.
		/// List of addresses that <see cref="SelectedCompanyId"/> has
		/// </summary>
		public List<CompanyAddress> AddressessThatSelectedCompanyHas { get; set; } = new List<CompanyAddress>();
		#endregion

		#region Places visited and number of miles traveled

		private bool _IncludeMiles = true;
		public bool IncludeMiles
		{
			get => this._IncludeMiles;
			set
			{
				this._IncludeMiles = value;

				if (this._IncludeMiles == false)
				{
					// Remove all visited addresses for selected invoice from server and client
					// and repopulates the Combobox_AddressesCouldVisit with addressess user could add as visited.
					this.ResetVisitedAddressess();


				}
				// must be true
				else
				{
					// set the default number of times visited to 1
					// (might have been zero which does not make sence if you are visiting a place)
					this.NewPlaceVisited_NoTimesVisited = 1;
				}
			}
		}


		#region add address visited with number of miles
		private int _Combobox_VisitedAddress_SelectedId;
		/// <summary>
		/// The selected value in the <select/> node.
		/// This is the Address the user has selected (in the drop down list) that they want to add as a place they 
		/// have visited at this address. They will also need to say how many times they visisted address.
		/// Fiinaly they will then have to click the add button to add it to the list of addressess visited
		/// </summary>
		public int Combobox_VisitedAddress_SelectedId
		{
			get => this._Combobox_VisitedAddress_SelectedId;
			set
			{
				this._Combobox_VisitedAddress_SelectedId = value;
			}

		}

		/// <summary>
		/// List of addresses that can be added as a place visited for given invoice.
		/// Will not list any addressess already visting in invoice
		/// </summary>
		public List<CompanyAddress> Combobox_AddressesCouldVisit { get; set; } = new List<CompanyAddress>();

		/// <summary>
		/// The number of times <see cref="Combobox_VisitedAddress_SelectedId"/> was visited.
		/// Once this value has been set and <see cref="Combobox_VisitedAddress_SelectedId"/> the user
		/// will click the add button, which will add the address to one of the places visited for the given invoice
		/// </summary>
		public int NewPlaceVisited_NoTimesVisited { get; set; }
		#endregion


		#region List Addresses visited with number of miles
		/// <summary>
		/// List of addressess the user have visited for the invoice we are createing
		/// </summary>
		public List<AddInvoicePlaceVisitedInInvoiceVM> CompanyAddressessVisitedVM { get; set; } = new List<AddInvoicePlaceVisitedInInvoiceVM>();
		#endregion




		#endregion




		#endregion







		#region Private Methods
		

		/// <summary>
		/// Finds the <see cref="CompanyDetails"/> assigned to the passed in CompanyAddressId
		/// </summary>
		/// <param name="CompanyAddressId">Company Address Id to use to look for company details</param>
		/// <returns>null if <see cref="CompanyDetails"/> could not be found</returns>
		private async Task<CompanyDetails?> FindCompanyLinkedToAddress(int? CompanyAddressId)
		{
			CompanyAddress? companyAddress = null;
			CompanyDetails? companyDetails = null;

			if (CompanyAddressId == null || CompanyAddressId <= 0)
				return null;

			// check the local cache
			companyAddress = this.Parent.Parent.Parent.ListOfCompanyAddressess.Where(address => address.Id == CompanyAddressId).FirstOrDefault();
			// if we found the address in the local cache
			if (companyAddress != null)
			{
				companyDetails = this.Parent.Parent.Parent.ListOfCompanies.Where(company => company.Id == companyAddress.CompanyDetailsID).FirstOrDefault();
			}
			// if we found the company deatils in the local cache
			if (companyDetails != null)
				return companyDetails;

			// need to go off to the server and try and find the deatils

			// if in the bove code we did not find the company address 
			if (companyAddress == null)
			{
				// ask the server for the company address details
				CompanyAddressCommunication serverCompanyAddress = new CompanyAddressCommunication(this.Parent.Parent.httpClient, this.Parent.Parent.appSettings);
				// Ask the server the for list of addresses asinged to company
				ServerResponseSingleCompanyAddress ServerAddressResponse = await serverCompanyAddress.GetCompanyAddress((int)CompanyAddressId);

				// if we were unable to get address details
				if (ServerAddressResponse.HasErrors)
					return null;

				// assign the value that was returned from the server
				companyAddress = ServerAddressResponse.ReturnValue;
			}

			// we should now have a CompanyAddress, now try and find the Company Details from the server
			CompanyCommunication serverCompany = new CompanyCommunication(this.Parent.Parent.httpClient, this.Parent.Parent.appSettings);
			ServerResponseSingleCompanyDetails ServerCompanyResponse = await serverCompany.GetSingleCompanyDetails(companyAddress.CompanyDetailsID);

			// if we were unable to find the company details
			if (ServerCompanyResponse.HasErrors)
				return null;
			else
				return ServerCompanyResponse.ReturnValue;
		}

		/// <summary>
		/// retuns a list of addresses assosiated with passed in CompanyId. 
		/// Will first check local cache for addressess and if can't find any,
		/// will ask the server for a list of addressess (which will then be cached)
		/// </summary>
		/// <param name="CompanyId"></param>
		private async Task<List<CompanyAddress>> FindAllAddressessAsociatedWithCompany(int? CompanyId)
		{
			List<CompanyAddress> ListOfAddressesAsignedToCompany;

			if (CompanyId == null || CompanyId <= 0)
				return new List<CompanyAddress>();

			// try and get the addressess for this company from the local cache
			ListOfAddressesAsignedToCompany = this.Parent.Parent.Parent.ListOfCompanyAddressess.Where(address => address.CompanyDetailsID == CompanyId).ToList();

			// if we have found some addresses for the company in the local cache
			if (ListOfAddressesAsignedToCompany.Count > 0)
				return ListOfAddressesAsignedToCompany;

			// if we get this far, we need to make a request to the server for a list of
			// addrsses for the passed in CompanyId. We will stored the returned address
			// in the local cache so save keep hitting the server on subsiquent requests to this function


			CompanyAddressCommunication serverCompanyAddress = new CompanyAddressCommunication(this.Parent.Parent.httpClient, this.Parent.Parent.appSettings);
			// Ask the server the for list of addresses asinged to company
			ServerResponseListOfCompanyAddresses ServerResponse = await serverCompanyAddress.GetListOfAddressesForCompany((int)CompanyId);

			// if the server responded with any erorrs
			if (ServerResponse.HasErrors)
				return new List<CompanyAddress>();

			// cache the addresses
			this.Parent.Parent.Parent.ListOfCompanyAddressess.AddRange(ServerResponse.ReturnValue);

			// return the list of addresses
			return ServerResponse.ReturnValue;
		}


		/// <summary>
		/// Ask the server for a list of places visisted for the passed in invoice id
		/// </summary>
		/// <param name="InvoiceId">The invoice id to use to look for address visited</param>
		/// <returns>null if error, else list of places visited</returns>
		private async Task<List<PlacesVisitedForInvoice>> FindVisitedAddresses(int InvoiceId)
		{
			List<PlacesVisitedForInvoice> PlacesVisited;

			InvoiceCommunication server = new InvoiceCommunication(this.Parent.Parent.httpClient, this.Parent.Parent.appSettings);
			ServerResponseListOfPlacesVisitedForInvoice ServerResponse = await server.GetPlacesVisitedForInvoice(InvoiceId);

			// if we were unable to get addressess (somthing went wrong)
			if (ServerResponse.HasErrors)
				return new List<PlacesVisitedForInvoice>();

			PlacesVisited = ServerResponse.ReturnValue;

			return PlacesVisited;
		}

		/// <summary>
		/// Inishzlise the <see cref="Combobox_AddressesCouldVisit"/> with a list of addresses
		/// Will load all addressses assoshiated with <see cref="SelectedCompanyId"/> minus
		/// any addressess found in <see cref="this.Parent.InvoiceData.PlacesVisitedForInvoice"/>
		/// </summary>
		private void InishalizeCombobox_AddressesCouldVisit()
		{
			if (this.Parent.Parent.InvoiceData.PlacesVisitedForInvoice != null)
			{
				// get all address assoshiated with selected company minus any address held in the Combobox_AddressesCouldVisit list
				var result = this.AddressessThatSelectedCompanyHas.Where(p => !this.Parent.Parent.InvoiceData.PlacesVisitedForInvoice.Any(p2 => p2.CompanyAddressId == p.Id));
				// make sure the list is empty
				this.Combobox_AddressesCouldVisit.Clear();
				// inishalize the list with the found addressess
				this.Combobox_AddressesCouldVisit.AddRange(result);
			}
			else
			{
				// make sure the list is empty
				this.Combobox_AddressesCouldVisit.Clear();
				// inishalize the list with the found addressess
				this.Combobox_AddressesCouldVisit.AddRange(this.AddressessThatSelectedCompanyHas);
			}

			// set the default selected item in the combo box.
			if (this.Combobox_AddressesCouldVisit.Any())
				this._Combobox_VisitedAddress_SelectedId = this.Combobox_AddressesCouldVisit[0].Id;
			else
				this._Combobox_VisitedAddress_SelectedId = 0;
		}

		/// <summary>
		/// Load the drop down list (<see cref="CompanyAddressessVisitedVM"/>) that contains addresses the user can select
		/// to add as a place visited for this invoice
		/// </summary>
		private void InishzlizeCompanyAddressessVisitedVM()
		{
			// remove any previouse addresses that were in the list
			this.CompanyAddressessVisitedVM.Clear();
			// we now want to create a view model for each address the invoice visited.
			// we can then use this view model to list in a table the places the user visited for that invoice
			// each row in the table will have a remove/delete button to allow the user to remove this address
			// from a palce the invoice visited.
			foreach (var placeVisited in this.Parent.Parent.InvoiceData.PlacesVisitedForInvoice)
			{
				// create a view model for this address the user visited
				//				AddInvoicePlaceVisitedInInvoiceVM PlaceVisitedViewModel = new AddInvoicePlaceVisitedInInvoiceVM();
				// add the number of times the user visited this address to the view model
				//				PlaceVisitedViewModel.NumberOfTimesVisited = placeVisited.NumberOfTimesVisited;
				// the server only returned the address ID for the place that was visited.
				// we need the adress details. These should be held in ListOfCompanyAddressess
				// beacuase the selected company id should be the same company that these addreses belong to
				// and when the selected company id has been set, it retreves all the addresses assoshated 
				// to that company and stores them in ListOfCompanyAddressess
				//				PlaceVisitedViewModel.PlaceVisited = this._Parent.Parent.ListOfCompanyAddressess.Where(a => a.Id == placeVisited.CompanyAddressId).First();

				// add the view model to the list
				//				this.CompanyAddressessVisitedVM.Add(PlaceVisitedViewModel);

				// create a new view model for this address visted and add it to the CompanyAddressessVisitedVM list
				this.AddVisitedAddressTo_CompanyAddressessVisitedVM(placeVisited.CompanyAddressId, placeVisited.NumberOfTimesVisited);

			}
		}

		private void AddVisitedAddressTo_CompanyAddressessVisitedVM(int VisitedCompanyAddressID, int NoTimesVisited)
		{
			// create a view model for this address the user visited
			AddInvoicePlaceVisitedInInvoiceVM PlaceVisitedViewModel = new AddInvoicePlaceVisitedInInvoiceVM();
			// add the number of times the user visited this address to the view model
			PlaceVisitedViewModel.NumberOfTimesVisited = NoTimesVisited;
			// the server only returned the address ID for the place that was visited.
			// we need the adress details. These should be held in ListOfCompanyAddressess
			// beacuase the selected company id should be the same company that these addreses belong to
			// and when the selected company id has been set, it retreves all the addresses assoshated 
			// to that company and stores them in ListOfCompanyAddressess
			PlaceVisitedViewModel.PlaceVisited = this._Parent.Parent.Parent.ListOfCompanyAddressess.Where(a => a.Id == VisitedCompanyAddressID).First();

			// add the view model to the list
			this.CompanyAddressessVisitedVM.Add(PlaceVisitedViewModel);
		}

		/// <summary>
		/// Removes a Company address view model from <see cref="CompanyAddressessVisitedVM"/> & 
		/// <see cref="this.Parent.InvoiceData.PlacesVisitedForInvoice"/>
		/// </summary>
		/// <param name="VisitedCompanyAddressID">The Id to look for to determin which view model gets removed</param>
		private void RemoveVisitedAddressFrom_CompanyAddressessVisitedVMAndInvoiceModel(int VisitedCompanyAddressID)
		{
			// go through each company address view model and find the one we want to remove
			// from the view model list
			for (int index = 0; index < this.CompanyAddressessVisitedVM.Count; index++)
			{
				var vm = this.CompanyAddressessVisitedVM[index];
				// if we have found the one we want to remove
				if (vm.PlaceVisited.Id == VisitedCompanyAddressID)
				{
					// remove from the view model
					this.CompanyAddressessVisitedVM.RemoveAt(index);
					// remove from the invoice model
					var placeVisitedModel = this.Parent.Parent.InvoiceData.PlacesVisitedForInvoice.Where(p => p.CompanyAddressId == VisitedCompanyAddressID).FirstOrDefault();
					if (placeVisitedModel != null)
						this.Parent.Parent.InvoiceData.PlacesVisitedForInvoice.Remove(placeVisitedModel);
					break;
				}
			}
		}














		private async void SelectedCompanyChanged()
		{
			// set to true to say we are communicating with the server
			this.IsAddishanlDetailsInSubmitState = true;

			////////////////////////////////////////////
			// remove any visited addressess from server, view model, and invoice model


			InvoiceCommunication serverInvoice = new InvoiceCommunication(this.Parent.Parent.httpClient, this.Parent.Parent.appSettings);
			ServerResponseBool serverResponse;

			// remove places visited in server database
			//if (this.Parent.InvoiceData.PlacesVisitedForInvoice != null && this.Parent.InvoiceData.PlacesVisitedForInvoice.Count() > 0)
			//	serverResponse = await serverInvoice.RemoveAllVisitedAddressFromInvoice(this.Parent.InvoiceData.Id);

			// remove places visited in invoice model
			//this.Parent.InvoiceData.PlacesVisitedForInvoice?.Clear();
			// remove from view model visited addresses
			//this.CompanyAddressessVisitedVM.Clear();

			// remove places visisted from server and client
			await this.RemoveAllVisitedAddressFromInvoice_FromFromServerAndClient(this.Parent.Parent.InvoiceData.Id);
			//////////////////////////////////////////////
			// clear the list of addresses user can add for visited address (in drop down list)
			this.Combobox_AddressesCouldVisit.Clear();
			this.Combobox_VisitedAddress_SelectedId = 0;
			///////////////////////////////////////////////////
			// remove invoice address (person we are invoicing)
			serverResponse = await serverInvoice.UpdateAddressInvoiceMadeOutTo(this.Parent.Parent.InvoiceData.Id, null);
			this.AddressessThatSelectedCompanyHas.Clear();


			// if we have a company selected
			if (this._SelectedCompanyId != null && this._SelectedCompanyId > 0)
			{
				// find all the addresses assishated with selected company id
				List<CompanyAddress> addresses = await this.FindAllAddressessAsociatedWithCompany(this._SelectedCompanyId);
				// Add the addresses to the view model
				this.AddressessThatSelectedCompanyHas.AddRange(addresses);

			}

			// set address invoice made out to id to 0 to indicate no address is selected;
			this._AddressToMakeInvoiceOutToId = 0;
			this.AddressToMakeInvoiceOutTo = null;


			// set to false to say we have finished communicating with the server
			this.IsAddishanlDetailsInSubmitState = false;

			this.Parent.Parent.Parent.CallUiNeedsUpdatingEvent();


		}

		/// <summary>
		/// This will get called when the <see cref="AddressToMakeInvoiceOutToId"/> properys set method gets called.
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		private async void SelectedAddressInvoiceMadeOutToChanged(int? AddressInvoiceMadeOutTo_PreviouseValue)
		{

			// set to true to say we communicating with the server
			this.IsAddishanlDetailsInSubmitState = true;

			// update the server with the new address the invoice has been made out to.
			// (Don't think this should be null, but check just in case)

			InvoiceCommunication serverCommunication = new InvoiceCommunication(this.Parent.Parent.httpClient, this.Parent.Parent.appSettings);
			ServerResponseBool response = await serverCommunication.UpdateAddressInvoiceMadeOutTo(this.Parent.Parent.InvoiceData.Id, this._AddressToMakeInvoiceOutToId);

			// if we were unable to change the address invoice was made out to.
			if (response.HasErrors)
			{
				// set the address invoice made out to back to its previouse value.
				this._AddressToMakeInvoiceOutToId = AddressInvoiceMadeOutTo_PreviouseValue;

				// set to false to say we have finished communicating with the server
				this.IsAddishanlDetailsInSubmitState = false;
				this.Parent.Parent.Parent.CallUiNeedsUpdatingEvent();

				return;
			}

			// load a list of addresses the use can select to say they have visited the address invoice made out to.
			this.InishalizeCombobox_AddressesCouldVisit();

			// set the address model to the address that was selected
			this.AddressToMakeInvoiceOutTo = this.AddressessThatSelectedCompanyHas.Where(a => a.Id == this._AddressToMakeInvoiceOutToId).First();

			// set to false to say we have finished communicating with the server
			this.IsAddishanlDetailsInSubmitState = false;
			this.Parent.Parent.Parent.CallUiNeedsUpdatingEvent();

		}


		/// <summary>
		/// Removes all visited address for passed in Invoice Id. Removes from server and client.
		/// On the Client it removes from <see cref="CompanyAddressessVisitedVM"/> & <see cref="this.Parent.InvoiceData.PlacesVisitedForInvoice"/>
		/// </summary>
		/// <param name="InvoiceID">The invoice to remove all the addresses from</param>
		private async Task RemoveAllVisitedAddressFromInvoice_FromFromServerAndClient(int InvoiceID)
		{
			InvoiceCommunication serverInvoice = new InvoiceCommunication(this.Parent.Parent.httpClient, this.Parent.Parent.appSettings);
			ServerResponseBool serverResponse;

			// set to true to say we communicating with the server
			this.IsAddishanlDetailsInSubmitState = true;

			// remove places visited in server database
			if (this.Parent.Parent.InvoiceData.PlacesVisitedForInvoice != null && this.Parent.Parent.InvoiceData.PlacesVisitedForInvoice.Count() > 0)
				serverResponse = await serverInvoice.RemoveAllVisitedAddressFromInvoice(InvoiceID);


			// remove places visited in invoice model
			this.Parent.Parent.InvoiceData.PlacesVisitedForInvoice?.Clear();
			// remove from view model visited addresses
			this.CompanyAddressessVisitedVM.Clear();


			// set to false to say we have finished communicating with the server
			this.IsAddishanlDetailsInSubmitState = false;
			// let the UI know a change has been made
			//this.Parent.Parent.CallUiNeedsUpdatingEvent();
		}

		/// <summary>
		/// Removes all visited addresses for selected invoice from server and client
		/// and repopulates the <see cref="Combobox_AddressesCouldVisit"/> with addressess user could add as visited.
		/// </summary>
		private async void ResetVisitedAddressess()
		{
			// remove all addresses that have been visited at address.
			// (remove from server & remove from UI)
			await this.RemoveAllVisitedAddressFromInvoice_FromFromServerAndClient(this.Parent.Parent.InvoiceData.Id);

			// load a list of addresses the use can select to say they have visited the address invoice made out to.
			this.InishalizeCombobox_AddressesCouldVisit();

			// let the UI know a change has been made
			this.Parent.Parent.Parent.CallUiNeedsUpdatingEvent();
		}

		#endregion











	}
}
