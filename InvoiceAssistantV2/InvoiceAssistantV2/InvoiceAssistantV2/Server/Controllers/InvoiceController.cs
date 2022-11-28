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
		[Route("RemovePayment")]
		[Produces("application/json")]
		public ControllerLogicReturnValue RemovePayment([FromForm] int InvoicePaymentId)
		{
            RemovePaymentControllerLogic ControllerLogic = new RemovePaymentControllerLogic();
            return ControllerLogic.Process(InvoicePaymentId);
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
