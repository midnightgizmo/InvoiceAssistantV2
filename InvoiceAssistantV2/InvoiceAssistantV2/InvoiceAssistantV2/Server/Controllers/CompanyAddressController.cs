using InvoiceAssistantV2.Server.ControllersLogic.Company;
using InvoiceAssistantV2.Server.ControllersLogic;
using Microsoft.AspNetCore.Mvc;
using InvoiceAssistantV2.Server.ControllersLogic.CompanyAddress;
using InvoiceAssistantV2.Shared.Models.Database.Company;

namespace InvoiceAssistantV2.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyAddressController : Controller
    {
        [HttpPost]
        [Route("AddressesForCopmany")]
        [Produces("application/json")]
        public ControllerLogicReturnValue GetAllAddressesForCompany([FromForm] int CompanyId)
        {
            GetAllAddressesForCompanyControllerLogic ControllerLogic = new GetAllAddressesForCompanyControllerLogic();
            return ControllerLogic.Process(CompanyId);
        }

        [HttpPost]
        [Route("Find")]
        [Produces("application/json")]
        public ControllerLogicReturnValue FindCompanyAddress([FromForm] int CompanyAddressId)
        {
            FindCompanyAddressControllerLogic ControllerLogic = new FindCompanyAddressControllerLogic();
            return ControllerLogic.Process(CompanyAddressId);
        }

        [HttpPost]
        [Route("Insert")]
        [Produces("application/json")]
        public ControllerLogicReturnValue InsertCompanyAddress([FromForm] string FriendlyName,
                                                                [FromForm] int CompanyDetailsID, [FromForm] int DrivingDistanceToAddress,
                                                                [FromForm] string? AddressLine1, [FromForm] string? AddressLine2,
                                                                [FromForm] string? AddressLine3, [FromForm] string? AddressLine4,
                                                                [FromForm] string? AddressLine5, [FromForm] string? PostCode
                                                             )
        {

            InsertCompanyAddressControllerLogic ControllerLogic = new InsertCompanyAddressControllerLogic();
            return ControllerLogic.Process( new CompanyAddress()
            {
                FriendlyName = FriendlyName,
                CompanyDetailsID = CompanyDetailsID,
                DrivingDistanceToAddress = DrivingDistanceToAddress,
                AddressLine1 = AddressLine1,
                AddressLine2 = AddressLine2,
                AddressLine3 = AddressLine3,
                AddressLine4 = AddressLine4,
                AddressLine5 = AddressLine5,
                PostCode = PostCode
            });
        }


            [HttpPost]
        [Route("Edit")]
        [Produces("application/json")]
        public ControllerLogicReturnValue EditCompanyAddress([FromForm]int Id, [FromForm]string FriendlyName, 
                                                             [FromForm]int CompanyDetailsID,[FromForm] int DrivingDistanceToAddress,
                                                             [FromForm]string? AddressLine1, [FromForm]string? AddressLine2, 
                                                             [FromForm]string? AddressLine3, [FromForm]string? AddressLine4, 
                                                             [FromForm]string? AddressLine5, [FromForm]string? PostCode
                                                             )
        {
            EditCompanyAddressControllerLogic ControllerLogic = new EditCompanyAddressControllerLogic();
            return ControllerLogic.Process(new CompanyAddress()
            {
                Id = Id,
                FriendlyName = FriendlyName,
                CompanyDetailsID = CompanyDetailsID,
                DrivingDistanceToAddress = DrivingDistanceToAddress,
                AddressLine1 = AddressLine1,
                AddressLine2 = AddressLine2,
                AddressLine3 = AddressLine3,
                AddressLine4 = AddressLine4,
                AddressLine5 = AddressLine5,
                PostCode = PostCode

            });

        }

        [HttpPost]
        [Route("Remove")]
        [Produces("application/json")]
        public ControllerLogicReturnValue RemoveCompanyAddress([FromForm] int CompanyAddressId)
        {
            RemoveCompanyAddressControllerLogic ControllerLogic = new RemoveCompanyAddressControllerLogic();
            return ControllerLogic.Process(CompanyAddressId);
        }
    }
}
