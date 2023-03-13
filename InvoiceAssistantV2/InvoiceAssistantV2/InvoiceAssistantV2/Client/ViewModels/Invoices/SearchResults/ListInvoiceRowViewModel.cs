using InvoiceAssistantV2.Client.Classes.Server;
using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Client.ViewModels.Invoices.Add;
using InvoiceAssistantV2.Shared.Models.Database.Company;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using System.Data.SqlTypes;
using System.Reflection;

namespace InvoiceAssistantV2.Client.ViewModels.Invoices.SearchResults
{
	public class ListInvoiceRowViewModel
	{
		/// <summary>
		/// Used to access the list of payment types there are for when payment type is updated by user
		/// </summary>
		private ListInvoicesViewModel _Parent;

		public ListInvoiceRowViewModel(Invoice invoiceData, HttpClient httpClient, AppSettings appSettings, ListInvoicesViewModel Parent)
		{
			this.InvoiceData = invoiceData;
			this.httpClient = httpClient;
			this.appSettings = appSettings;

			this._Parent = Parent;

			// create a blank invoice. When a cell is being edited, it will copy the read only value
			// into its equivelent edit value.
			this.InvoiceEditData = new Invoice();


			this.InvoiceDateCell = new InvoiceEditCellData(this, this.InvoiceData, this.InvoiceEditData, nameof(Invoice.DateOfInvoice));
			this.ReferenceNumberCell = new InvoiceEditCellData(this, this.InvoiceData, this.InvoiceEditData, nameof(Invoice.ReferenceNumber));
			this.InvoiceDescriptionCell = new InvoiceEditCellData(this, this.InvoiceData, this.InvoiceEditData, nameof(Invoice.Description));
			this.PaymentTypeIdCell = new InvoiceEditCellData(this, this.InvoiceData, this.InvoiceEditData, nameof(Invoice.PaymentTypeID));
			// we need to know when the payment type id has been updated so we can then updated the PaymentType property
			this.PaymentTypeIdCell.PropertyUpdated += delegate ()
			{
				// if we have a payment type id
				if (this.InvoiceEditData.PaymentTypeID != null && this.InvoiceEditData.PaymentTypeID > 0)
					// update the payment type
					this.InvoiceData.PaymentType = this._Parent.ListOfPaymentTypes.Where(s => s.Id == this.InvoiceEditData.PaymentTypeID).FirstOrDefault();
				// if we don't have a payment type id
				else
					// set the payment type to null
					this.InvoiceData.PaymentType = null;
			};
			this.DateRecievedMoneyCell = new InvoiceEditCellData(this, this.InvoiceData, this.InvoiceEditData, nameof(Invoice.DateRecievedMoney));
			this.AddressToMakeInvoiceOutToIdCell = new InvoiceEditCellData(this, this.InvoiceData, this.InvoiceEditData, nameof(Invoice.AddressToMakeInvoiceOutToId));


			// set up the addishnal info
			this.AddishanalInvoiceInfo = new ListInvoiceRowAdditionalInfoViewModel(this);
		}

		/// <summary>
		/// Parent class that creates this view model. Allows child class access to <see cref="_Parent"/> variable
		/// </summary>
		public ListInvoicesViewModel Parent { get => this._Parent; }

		/// <summary>
		/// if we have one or more of the main invoice details being edited
		/// </summary>
		public bool IsRowInEditState { get; set; } = false;

		/// <summary>
		/// View model for the Addishanal details in the left column of the UI once the info button has been clicked
		/// </summary>
		public ListInvoiceRowAdditionalInfoViewModel AddishanalInvoiceInfo { get; set; }

	
		/// <summary>
		/// The data to display on a cell when the cell is in read only mode (not edited)
		/// </summary>
		public Invoice InvoiceData { get; set; }
		/// <summary>
		/// The data to display when a cell is being edited
		/// </summary>
		public Invoice InvoiceEditData { get; set; }



		/// <summary>
		/// Determins if the Print, Info and Delete buttons should be shown (these are displayed in the last column of the row)
		/// If set to false it most likely means one of these buttons has been clicked and is displaying more information
		/// in that cell, so we need to hide these buttons to make room for the new information to be displayed.
		/// </summary>
		public bool ShouldShow_Print_Info_Delete { get; set; } = true;

		/// <summary>
		/// Needed when <see cref="httpClient"/> communicats with the server
		/// </summary>
		public AppSettings appSettings { get; set; }
		/// <summary>
		/// Used for communication with the sever
		/// </summary>
		public HttpClient httpClient { get; set; }





		#region private methods



		/// <summary>
		/// Sends the server the new invoice data for the server to update it in the database.
		/// On server response, it will update <see cref="InvoiceData"/> with the value the user asked to be edited.
		/// </summary>
		/// <param name="PropertyNameToUpdate">The property name from the <see cref="Invoice"/> class that we want updatd on the server </param>
		/// <returns>true if sucsesfull, else false</returns>
		private async Task<bool> UpdateInvoiceDataWithServer(string PropertyNameToUpdate)
		{
			// get the data to send to the server
			Invoice InvoiceToSendToServer = this.CreateInvoiceDataToSendToServer(PropertyNameToUpdate);
			

			// Ask the server to update the invoice with the new invoice data we have
			InvoiceCommunication server = new InvoiceCommunication(this.httpClient, this.appSettings);
			ServerResponseSingleInvoice serverResponse = await server.UpdateInvoice(InvoiceToSendToServer.Id, InvoiceToSendToServer.DateOfInvoice, InvoiceToSendToServer.ReferenceNumber,
																					InvoiceToSendToServer.Description, InvoiceToSendToServer.PaymentTypeID, 
																					InvoiceToSendToServer.AddressToMakeInvoiceOutToId, InvoiceToSendToServer.DateRecievedMoney);
			// if the invoice was updated on the server
			if(serverResponse.HasErrors == false)
			{
				// copy the values that from the invoice data the server has sent back
				// (this should give us the updated values the user asked for)
				this.InvoiceData.DateOfInvoice = serverResponse.ReturnValue.DateOfInvoice;
				this.InvoiceData.ReferenceNumber = serverResponse.ReturnValue.ReferenceNumber;
				this.InvoiceData.Description = serverResponse.ReturnValue.Description;
				this.InvoiceData.PaymentTypeID = serverResponse.ReturnValue.PaymentTypeID;
				this.InvoiceData.PaymentType = serverResponse.ReturnValue.PaymentType;
				this.InvoiceData.DateRecievedMoney = serverResponse.ReturnValue.DateRecievedMoney;


				this.InvoiceData.AddressToMakeInvoiceOutToId = serverResponse.ReturnValue.AddressToMakeInvoiceOutToId;
			}

			// return true if sucsefull, else false
			return !serverResponse.HasErrors;
		}


		/// <summary>
		/// returns <see cref="Invoice"/> object with the updated value the user has changed.
		/// </summary>
		/// <param name="PropertyNameToUpdate">The property on <see cref="Invoice"/> that has been updated</param>
		/// <returns>A copy of all the values from <see cref="InvoiceData"/> with the exception of the property that was passed in. </returns>
		private Invoice CreateInvoiceDataToSendToServer(string PropertyNameToUpdate)
		{
			

			// this will hold the invoice data we want to send to the server
			Invoice InvoiceCopy = new Invoice();
			// make a copy of the passed in invoice data
			Invoice.Copy(this.InvoiceData, InvoiceCopy);
			
			// find the property the user wants to use to get updated
			PropertyInfo propInfo = typeof(Invoice).GetProperty(PropertyNameToUpdate);
			// find the value of the property we want to copy
			object NewPropertyValue = propInfo.GetValue(this.InvoiceEditData, null);
			// update the InvoiceCopy's property with the edited value from the user
			propInfo.SetValue(InvoiceCopy, NewPropertyValue);


			return InvoiceCopy;
		}
		#endregion



		#region Editing
		

		public InvoiceEditCellData InvoiceDateCell;
		public InvoiceEditCellData ReferenceNumberCell;
		public InvoiceEditCellData InvoiceDescriptionCell;
		public InvoiceEditCellData PaymentTypeIdCell;
		public InvoiceEditCellData DateRecievedMoneyCell;
		public InvoiceEditCellData AddressToMakeInvoiceOutToIdCell;

		/// <summary>
		/// checks all the IsBeingEdited_<name>. If any of them are set to true,
		/// this function will return true, else false;
		/// </summary>
		/// <returns>true if we are in an edit state, else false</returns>
		protected bool DeterminIfRowIsInEditState()
		{

			if (this.InvoiceDateCell.IsBeingEdited)
				return true;
			if (this.ReferenceNumberCell.IsBeingEdited)
				return true;
			if (this.InvoiceDescriptionCell.IsBeingEdited)
				return true;
			if (this.PaymentTypeIdCell.IsBeingEdited)
				return true;
			if (this.DateRecievedMoneyCell.IsBeingEdited)
				return true;
			if (this.AddressToMakeInvoiceOutToIdCell.IsBeingEdited)
				return true;


			return false;
		}



		public class InvoiceEditCellData
		{
			/// <summary>
			/// The parent class that inishalizes this class
			/// </summary>
			private ListInvoiceRowViewModel _Parent;
			/// <summary>
			/// Determins if we are in an edit mode.
			/// </summary>
			public bool IsBeingEdited { get; set; } = false;

			/// <summary>
			/// The Invoice data that is held in the database
			/// </summary>
			private Invoice _LiveInvoiceData;
			/// <summary>
			/// The invoice data that we use when we want to Edit one or more of the propertys.
			/// These Value will be then applied to the <see cref="_LiveInvoiceData"/> if the edit is accepted
			/// </summary>
			private Invoice _InvoiceDataWhenInEditMode;
			/// <summary>
			/// The property name we want to keep track of and edit.
			/// </summary>
			private string _PropertyNameToUpdate;

			/// <summary>
			/// The The property on the <see cref="Invoice"/> class we want to edit.
			/// Will be found using the property <see cref="_PropertyNameToUpdate"/> 
			/// </summary>
			private PropertyInfo _PropInfo;


			/// <summary>
			/// An event that wil be called from <see cref="EndEdit"/> when the property has been updated on the live data
			/// </summary>
			public event Action PropertyUpdated = delegate () { };

			/// <summary>
			/// Will be set to true when <see cref="EndEdit"/> is called and then set to false when <see cref="EndEdit"/> finishes
			/// </summary>
			public bool IsInSubmitState { get; set; } = false;



			public InvoiceEditCellData(ListInvoiceRowViewModel Parent, Invoice LiveInvoiceData, Invoice InvoiceDataWhenInEditMode, string InvoicePropertyNameToUpdate)
			{
				this._Parent = Parent;
				this._LiveInvoiceData = LiveInvoiceData;
				this._InvoiceDataWhenInEditMode = InvoiceDataWhenInEditMode;
				this._PropertyNameToUpdate = InvoicePropertyNameToUpdate;

				// find the property the user wants to use to get updated
				this._PropInfo = typeof(Invoice).GetProperty(this._PropertyNameToUpdate);
			}

			/// <summary>
			/// Call this when we want the row to go into an edit state
			/// </summary>
			public void BeginEdit()
			{
				// copy the read only value into the edit value
				this.CopyPropertyValue(this._LiveInvoiceData, this._InvoiceDataWhenInEditMode);
				// keep track of if we are in an edit mode for this property
				this.IsBeingEdited = true;


				// set the row to an editing state
				this._Parent.IsRowInEditState = true;

			}
			/// <summary>
			/// Cancel the edit and undo any changes made
			/// </summary>
			public void CancelEdit()
			{
				this.IsBeingEdited = false;
				this._Parent.IsRowInEditState = this._Parent.DeterminIfRowIsInEditState();

				

			}
			/// <summary>
			/// Commit the edit changes to the server and bring out of edit (will stay in edit mode if anything goes wrong)
			/// </summary>
			/// <returns></returns>
			public async Task EndEdit()
			{
				// indicate we are in a submit state to the server
				this.IsInSubmitState = true;

				// update the invoice data on the server end with the changes we want
				bool WasSucsefull = await this._Parent.UpdateInvoiceDataWithServer(this._PropertyNameToUpdate);
				// was the server suscsufull in updateing the invoice data
				if(WasSucsefull == true)
				{
					this.CopyPropertyValue(this._InvoiceDataWhenInEditMode, this._LiveInvoiceData);
					this.IsBeingEdited = false;
					this._Parent.IsRowInEditState = this._Parent.DeterminIfRowIsInEditState();

					// call the event to say the property has been updated.
					this.PropertyUpdated();

				}

				// we have finished talking to the sever and no longer in a submit state
				this.IsInSubmitState = false;


			}

			/// <summary>
			/// Uses <see cref="_PropInfo"/> to copy the propertys value from the one invoice to the other
			/// </summary>
			/// <param name="From"></param>
			/// <param name="To"></param>
			private void CopyPropertyValue(Invoice From, Invoice To)
			{
				// find the value of the property we want to copy
				object NewPropertyValue = this._PropInfo.GetValue(From, null);
				// update the property value on the invoice we are copying too.
				this._PropInfo.SetValue(To, NewPropertyValue);
			}
		}
		#endregion


	}


}
