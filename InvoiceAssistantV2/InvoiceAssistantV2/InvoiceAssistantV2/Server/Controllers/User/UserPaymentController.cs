using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Server.ControllersLogic;
using InvoiceAssistantV2.Server.ControllersLogic.User.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InvoiceAssistantV2.Server.Controllers.User
{
	[ApiController]
	[Route("User/UserDetails/Payment")]
	public class UserPaymentController : ControllerBase
	{
		public UserPaymentController(IOptions<AppSettings> appSettings)
		{
			this.appSettings = appSettings.Value;
		}

		public AppSettings appSettings { get; set; }

		[HttpPost]
		[Route("GetPaymentMethod")]
		[Produces("application/json")]
		public ControllerLogicReturnValue GetPaymentMethod([FromForm]int PaymentMethodId)
		{
			GetPaymentMethodControllerLogic ControllerLogic = new GetPaymentMethodControllerLogic(this.Response);

			return ControllerLogic.Process(PaymentMethodId);
		}

		[HttpPost]
		[Route("AddPaymentMethod")]
		[Produces("application/json")]
		public ControllerLogicReturnValue AddNewPaymentMethod([FromForm]int UsersDetailsId, [FromForm]string PaymentMethodName)
		{
			AddNewPaymentMethodControllerLogic ControllerLogic = new AddNewPaymentMethodControllerLogic(this.Response);
			
			return ControllerLogic.Process(UsersDetailsId, PaymentMethodName);
		}

		[HttpPost]
		[Route("UpdatePaymentMethodName")]
		[Produces("application/json")]
		public ControllerLogicReturnValue UpdatePaymentMethodName([FromForm]int PaymentMethodId, [FromForm]string PaymentMethodName)
		{
			UpdatePaymentMethodNameControllerLogic ControllerLogic = new UpdatePaymentMethodNameControllerLogic(this.Response);

			return ControllerLogic.Process(PaymentMethodId,PaymentMethodName);
		}






		[HttpPost]
		[Route("GetPaymentDetails")]
		[Produces("application/json")]
		public ControllerLogicReturnValue GetPaymentDetails([FromForm]int paymentMethodInfoId)
		{
			GetPaymentDetailsControllerLogic ControllerLogic = new GetPaymentDetailsControllerLogic(this.Response);
			
			return ControllerLogic.Process(paymentMethodInfoId);
		}


		[HttpPost]
		[Route("AddPaymentDetail")]
		[Produces("application/json")]
		public ControllerLogicReturnValue AddPaymentDetail([FromForm]int PaymentMethodId, [FromForm]string PaymentDetailKey, [FromForm]string? PaymentDetailValue)
		{
			AddPaymentDetailControllerLogic ControllerLogic = new AddPaymentDetailControllerLogic(this.Response);

			return ControllerLogic.Process(PaymentMethodId,PaymentDetailKey,PaymentDetailValue);
		}

		[HttpPost]
		[Route("UpdatePaymentDetails")]
		[Produces("application/json")]
		public ControllerLogicReturnValue UpdatePaymentDetails([FromForm] int PaymentDetailsId, [FromForm] string PaymentDetailKey, [FromForm] string? PaymentDetailValue)
		{
			UpdatePaymentDetailsControllerLogic ControllerLogic = new UpdatePaymentDetailsControllerLogic(this.Response);

			return ControllerLogic.Process(PaymentDetailsId, PaymentDetailKey, PaymentDetailValue);
		}

		[HttpPost]
		[Route("DeletePaymentDetail")]
		[Produces("application/json")]
		public ControllerLogicReturnValue DeletePaymentDetail([FromForm]int PaymentDetailsId)
		{
			DeletePaymentDetailControllerLogic ControllerLogic = new DeletePaymentDetailControllerLogic(this.Response);
			
			return ControllerLogic.Process(PaymentDetailsId);
		}
	}
}
