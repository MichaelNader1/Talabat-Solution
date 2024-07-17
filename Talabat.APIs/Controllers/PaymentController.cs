using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.ServiceInterfaces;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class PaymentController: BaseApiController
    {
        const string endpointSecret = "whsec_086da796cd6a41875e55ad1b36b310e0bb6d6344677830762ab3c94dceff8d78";
        private readonly IpaymentService _ipaymentService;
        public PaymentController(IpaymentService IpaymentService)
        {
            _ipaymentService = IpaymentService;
        }
        [ProducesResponseType(typeof(CustomerBasket),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var Basket = await _ipaymentService.CreateOrUpdatePaymentIntent(basketId);
            if (Basket == null) return BadRequest(new ApiResponse(400,"There is a problem in your payment!"));
            return Ok(Basket);
        }



       [HttpPost("webhook")]
       [AllowAnonymous]
      public async Task<IActionResult> Index()
      {
          var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
          try
          {
              var stripeEvent = EventUtility.ConstructEvent(json,
                  Request.Headers["Stripe-Signature"], endpointSecret);
                var PaymentIntent = stripeEvent.Data.Object as PaymentIntent;

              // Handle the event
              if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
              {
                    await _ipaymentService.UpdatePaymentIntentToSuccessOrFail(PaymentIntent.Id, false);
              }
              else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
              {
                    await _ipaymentService.UpdatePaymentIntentToSuccessOrFail(PaymentIntent.Id, true);

                }
                // ... handle other event types
                else
              {
                  Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
              }

              return Ok();
          }
          catch (StripeException e)
          {
              return BadRequest();
          }
      }

        }
}
