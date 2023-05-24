using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Server.ControllersLogic.User.UserDetails;
using InvoiceAssistantV2.Server.ControllersLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using InvoiceAssistantV2.Shared.Models.Database.User;
using InvoiceAssistantV2.Server.ControllersLogic.User.UserAddress;

namespace InvoiceAssistantV2.Server.Controllers.User
{
	[ApiController]
	[Route("User/[controller]")]
	public class UserAddressController : Controller
	{
		public UserAddressController(IOptions<AppSettings> appSettings)
		{
			this.appSettings = appSettings.Value;
		}

		public AppSettings appSettings { get; set; }

		[HttpPost]
		[Route("UpdateUsersAddress")]
		[Produces("application/json")]
		public ControllerLogicReturnValue UpdateUsersAddress([FromForm] UserAddress usersAddress)
		{
			UpdateUsersAddressControllerLogic controllerLogic = new UpdateUsersAddressControllerLogic(this.Response);

			return controllerLogic.Process(usersAddress);
		}
	}
}
