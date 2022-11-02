using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Server.ControllersLogic.Invoice;
using InvoiceAssistantV2.Server.ControllersLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using InvoiceAssistantV2.Server.ControllersLogic.Company;
using InvoiceAssistantV2.Shared.Models.Database.Company;

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
        [Route("Find")]
        [Produces("application/json")]
        public ControllerLogicReturnValue FindCompany([FromForm]int CompanyId)
        {
            FindCompanyControllerLogic ControllerLogic = new FindCompanyControllerLogic();
            return ControllerLogic.Process(CompanyId);

        }

        [HttpPost]
        [Route("Edit")]
        [Produces("application/json")]
        public ControllerLogicReturnValue EditCompanyDetails([FromForm]CompanyDetails companyDetails)
        {
            EditCompanyDetailsControllerLogic ControllerLogic = new EditCompanyDetailsControllerLogic();
            return ControllerLogic.Process(companyDetails);
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
