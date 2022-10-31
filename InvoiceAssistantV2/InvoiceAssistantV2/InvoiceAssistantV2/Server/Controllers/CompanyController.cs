using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Server.ControllersLogic.Invoice;
using InvoiceAssistantV2.Server.ControllersLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using InvoiceAssistantV2.Server.ControllersLogic.Company;

namespace InvoiceAssistantV2.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        public CompanyController(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        public AppSettings appSettings { get; set; }


        [HttpGet]
        [Route("ListOfCompanys")]
        [Produces("application/json")]
        public ControllerLogicReturnValue GetAllPaymentTypes()
        {
            ListOfCompanysControllerLogic ControllerLogic = new ListOfCompanysControllerLogic();
            return ControllerLogic.Process();
        }

        [HttpPost]
        [Route("Remove")]
        [Produces("application/json")]
        public ControllerLogicReturnValue RemoveCompany([FromForm]int CompanyId)
        {
            RemoveCompanyControllerLogic ControllerLogic = new RemoveCompanyControllerLogic();
            return ControllerLogic.Process(CompanyId);
        }
    }
}
