using InvoiceAssistantV2.Client.Classes.Server;
using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Client.ViewModels.Invoices.Add;
using InvoiceAssistantV2.Shared.Models.Database.Company;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using System.ComponentModel;
using System.Runtime.Intrinsics.X86;

namespace InvoiceAssistantV2.Client.ViewModels.Invoices.SearchResults
{
	/// <summary>
	/// View model for Additional info section of an invoice 
	/// (invoice address, places visited in invoice, payment break downs)
	/// </summary>
	public class ListInvoiceRowAdditionalInfoViewModel
	{
		#region private variables

		/// <summary>
		/// The view model that creates this view model
		/// </summary>
		private ListInvoiceRowViewModel _Parent;

		/// <summary>
		/// will be set to true the first time <see cref="Inishalize"/> method has been called
		/// </summary>
		//private bool _HasRowBeenInishalized = false;

		#endregion 

		public ListInvoiceRowAdditionalInfoViewModel(ListInvoiceRowViewModel parent)
		{
			_Parent = parent;
			this.LeftColumnViewModel = new ListInvoiceRowAdditionalInfo_LeftColumnViewModel(this);
			this.RightColumnViewModel = new ListInvoiceRowAdditionalInfo_RightColumnViewModel(this);
		}

        


		#region public properties

		/// <summary>
		/// Allows the View to have access to the parent class
		/// </summary>
		public ListInvoiceRowViewModel Parent { get => this._Parent; }


		/// <summary>
		/// View model for the left column addishanal invoice info
		/// </summary>
		public ListInvoiceRowAdditionalInfo_LeftColumnViewModel LeftColumnViewModel { get; set; }
		/// <summary>
		/// View model for the right colulmn addishanal invoice info
		/// </summary>
		public ListInvoiceRowAdditionalInfo_RightColumnViewModel RightColumnViewModel { get; set; }

		private bool _IsRowSelected = false;
		/// <summary>
		/// If true, the user will be presented with additional data on the invoice 
		/// </summary>
		public bool IsRowSelected
		{
			get => this._IsRowSelected;
			set
			{
				this._IsRowSelected = value;

				// if the row has been selected
				if (this._IsRowSelected == true)
				{
					Task LetfColumnTask = this.LeftColumnViewModel.Inishalize();// grab all the data we need from the server and put it into the view model
					Task RightColumnTask = this.RightColumnViewModel.Inishalize();// grab all the data we need from the server and put it into the view model

					// wait for the above taks to complete.
					Task.WhenAll(new Task[] { LetfColumnTask, RightColumnTask }).ContinueWith((t) => 
					{
						// tell the UI changes have been made so we can update the UI
						this.Parent.Parent.CallUiNeedsUpdatingEvent();
					});
					
					
				}
				
			}
		}
		
		
		#endregion

		

		

	}
}
