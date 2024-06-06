using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string whSecret = "whsec_cfba1ce98d64f0733d6d06c3cc6d7a4ceb2f4274846a769d05c108116a8d13bc";
        public PaymentsController(IPaymentService paymentService,
            ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [Authorize]
        [ProducesResponseType(typeof( CustomerBasket),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketid}")] //Get : BaseUrl/api/payment/{basketid}
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null) return BadRequest(new ApiResponse(400, "An Error in your Basket"));

            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], whSecret);

            Order? order;

            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, true);
                    _logger.LogInformation($"Order is succeeded {order?.PaymentIntentId}");
                    _logger.LogInformation($"Unhandled event type: {stripeEvent.Type}");

                    break;

                case Events.PaymentIntentPaymentFailed:
                    order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, false);
                    _logger.LogInformation($"Order is failed {order?.PaymentIntentId}");

                    break;

            }

            return Ok();
        }
    }
}
