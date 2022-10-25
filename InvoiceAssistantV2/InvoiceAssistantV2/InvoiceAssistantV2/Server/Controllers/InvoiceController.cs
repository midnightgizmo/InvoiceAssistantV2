using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Server.ControllersLogic;
using InvoiceAssistantV2.Server.ControllersLogic.Invoice;
using InvoiceAssistantV2.Shared.Models;
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
    }
}
