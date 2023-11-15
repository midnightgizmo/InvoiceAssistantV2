using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Server.ControllersLogic;
using InvoiceAssistantV2.Server.ControllersLogic.Invoice;
using InvoiceAssistantV2.Shared.Models;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Runtime.ExceptionServices;

namespace InvoiceAssistantV2.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
	{

        public InvoiceController(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        public AppSettings appSettings { get; set; }

		[HttpPost]
		[Route("InsertNewInvoice")]
		[Produces("application/json")]
		public ControllerLogicReturnValue InsertNewInvoice([FromForm]DateTime DateOfInvoice, [FromForm] string ReferenceNumber,
															[FromForm]string Description, [FromForm] int AddressToMakeInvoiceOutToId, 
                                                            [FromForm] PlacesVisitedForInvoiceBase[]? PlacesVisited)
        {
            InsertNewInvoiceControllerLogic ControllerLogic = new InsertNewInvoiceControllerLogic();

            return ControllerLogic.Process(DateOfInvoice, ReferenceNumber, Description,
													AddressToMakeInvoiceOutToId, PlacesVisited);
        }
        [HttpPost]
        [Route("UpdateMainDetails")]
        [Produces("application/json")]
        public ControllerLogicReturnValue UpdateInvoiceMainDetails([FromForm]int Id, [FromForm]DateTime DateOfInvoice, 
                                                                   [FromForm]string ReferenceNumber, [FromForm]string Description,
                                                                   [FromForm]int PaymentTypeID, [FromForm]int AddressToMakeInvoiceOutToId,
                                                                   [FromForm]DateTime? DateRecievedMoney)
        {
			UpdateInvoiceMainDetailsControllerLogic ControllerLogic = new UpdateInvoiceMainDetailsControllerLogic();
            return ControllerLogic.Process(Id, DateOfInvoice, ReferenceNumber,
                                           Description, PaymentTypeID,
                                           AddressToMakeInvoiceOutToId, DateRecievedMoney,
                                           this.Response);

		}
        [HttpPost]
        [Route("UpdateAddressInvoiceMadeOutTo")]
        [Produces("application/json")]
        public ControllerLogicReturnValue UpdateAddressInvoiceMadeOutTo([FromForm]int InvoiceId, [FromForm]int? AddressToMakeInvoiceOutToId)
        {
            UpdateAddressInvoiceMadeOutToControllerLogic ControllerLogic = new UpdateAddressInvoiceMadeOutToControllerLogic();
            return ControllerLogic.Process(InvoiceId, AddressToMakeInvoiceOutToId, this.Response);

		}


		[HttpPost]
		[Route("Delete")]
		[Produces("application/json")]
		public ControllerLogicReturnValue DeleteInvoice([FromForm]int InvoiceId)
        {
			DeleteInvoiceControllerLogic ControllerLogic = new DeleteInvoiceControllerLogic();
            return ControllerLogic.Process(InvoiceId);

		}

        [HttpPost]
        [Route("Search")]
        [Produces("application/json")]
        public ControllerLogicReturnValue Search([FromForm]InvoiceSearchParameters invoiceSearchParameters)
		{
            SearchForInvoicesControllerLogic ControllerLogic = new SearchForInvoicesControllerLogic();
            return ControllerLogic.Process(invoiceSearchParameters);
		}

        [HttpGet]
        [Route("Payment")]
        [Produces("application/json")]
        public ControllerLogicReturnValue GetAllPaymentTypes()
        {
            GetAllPaypmentTypesControllerLogic ControllerLogic = new GetAllPaypmentTypesControllerLogic();
            return ControllerLogic.Process();
        }

		[HttpPost]
		[Route("AddPayment")]
		[Produces("application/json")]
		public ControllerLogicReturnValue AddPayment([FromForm]int InvoiceId, [FromForm]string Description, [FromForm]decimal Ammount)
        {
			AddPaymentControllerLogiccs ControllerLogic = new AddPaymentControllerLogiccs();
            return ControllerLogic.Process(InvoiceId, Description, Ammount);

		}

        [HttpPost]
        [Route("EditPayment")]
        [Produces("application/json")]
        public ControllerLogicReturnValue EditPayment([FromForm]int InvoicePaymentId, [FromForm]string Description, [FromForm]decimal Ammount)
        {
            EditPaymentRowControllerLogic ControllerLogic = new EditPaymentRowControllerLogic();
            return ControllerLogic.Process(InvoicePaymentId, Description, Ammount);
        }

		[HttpPost]
		[Route("RemovePayment")]
		[Produces("application/json")]
		public ControllerLogicReturnValue RemovePayment([FromForm] int InvoicePaymentId)
		{
            RemovePaymentControllerLogic ControllerLogic = new RemovePaymentControllerLogic();
            return ControllerLogic.Process(InvoicePaymentId);
		}


		[HttpPost]
		[Route("ListAllPaymentsForInvoice")]
		[Produces("application/json")]
		public ControllerLogicReturnValue ListAllPaymentsForInvoice([FromForm]int InvoiceId)
        {
            ListAllPaymentsForInvoiceControllerLogic ControllerLogic = new ListAllPaymentsForInvoiceControllerLogic();
            return ControllerLogic.Process(InvoiceId);
        }

        [HttpPost]
        [Route("GetInvoiceTotalPaymentAmount")]
        [Produces("application/json")]
        public ControllerLogicReturnValue GetInvoiceTotalPaymentAmount([FromForm]int InvoiceId) 
        {
            GetInvoiceTotalPaymentAmountControllerLogic ControllerLogic = new GetInvoiceTotalPaymentAmountControllerLogic();
            return ControllerLogic.Process(InvoiceId);
        }

		[HttpPost]
        [Route("ListPlacesVisited")]
        [Produces("application/json")]
        public ControllerLogicReturnValue GetAddressessVisited([FromForm]int InvoiceId)
        {
            GetAddressessVisitedCotrollerLogic ControllerLogic = new GetAddressessVisitedCotrollerLogic();
            return ControllerLogic.Process(InvoiceId);
        }

        [HttpPost]
        [Route("AddPlaceVisited")]
        [Produces("application/json")]
        public ControllerLogicReturnValue AddAddressVisited([FromForm]int InvoiceId, [FromForm]int AddressId, [FromForm]int NoTimesVisited)
        {
            AddAddressVisitedControllerLogic ControllerLogic = new AddAddressVisitedControllerLogic(this.Response);
            return ControllerLogic.Process(InvoiceId, AddressId, NoTimesVisited);
        }

        /// <summary>
        /// Removes all visited addresses for the passed in invoice id
        /// </summary>
        /// <param name="InvoiceId"></param>
        /// <returns></returns>
		[HttpPost]
		[Route("RemoveAllPlacesVisited")]
		[Produces("application/json")]
		public ControllerLogicReturnValue RemoveAllPlacesVisited([FromForm]int InvoiceId) 
        {
            RemoveAllPlacesVisitedControllerLogic ControllerLogic = new RemoveAllPlacesVisitedControllerLogic(this.Response);
            return ControllerLogic.Process(InvoiceId);

		}

        [HttpPost]
        [Route("RemoveVisitedAddressFromInvoice")]
        [Produces("application/json")]
        public ControllerLogicReturnValue RemoveVisitedAddressFromInvoice([FromForm]int InvoiceId, [FromForm]int AddressId)
        {
            RemoveVisitedAddressFromInvoiceControllerLogic ControllerLogic = new RemoveVisitedAddressFromInvoiceControllerLogic(this.Response);
            return ControllerLogic.Process(InvoiceId, AddressId);

		}

		[HttpPost]
		[Route("GenerateReferenceNumber")]
		[Produces("application/json")]
		public ControllerLogicReturnValue GenerateUniqueReferenceNumber([FromForm]DateTime InvoiceDate)
        {
            GenerateUniqueReferenceNumberControllerLogic ControllerLogic = new GenerateUniqueReferenceNumberControllerLogic();
            return ControllerLogic.Process(InvoiceDate);

		}

	}
}
