﻿using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Server.ControllersLogic;
using InvoiceAssistantV2.Server.ControllersLogic.User.UserDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InvoiceAssistantV2.Server.Controllers.User
{
	[ApiController]
	[Route("User/[controller]")]
	public class UserDetailsController : ControllerBase
	{
		public UserDetailsController(IOptions<AppSettings> appSettings)
		{
			this.appSettings = appSettings.Value;
		}

		public AppSettings appSettings { get; set; }

		[HttpPost]
		[Route("GetUsersDetails")]
		[Produces("application/json")]
		public ControllerLogicReturnValue GetUsersDetails([FromForm]bool includePaymentDetails, [FromForm]bool includeAddressDetails)
		{
			GetUserDetailsControllerLogic controllerLogic = new GetUserDetailsControllerLogic();
			
			return controllerLogic.Process(includePaymentDetails, includeAddressDetails, this.Response);
		}


		[HttpPost]
		[Route("UpdateUsersDetails")]
		[Produces("application/json")]
		public ControllerLogicReturnValue UpdateUsersDetails([FromForm]int Id, [FromForm]string UsersName) 
		{
			UpdateUserDetailsControllerLogic controllerLogic = new UpdateUserDetailsControllerLogic();

			return controllerLogic.Process(Id, UsersName, this.Response);
		}
	}
}
