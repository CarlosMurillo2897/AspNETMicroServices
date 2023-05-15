using Basket.API.Entities;
using Basket.API.GRPCServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGRPCService _discountGRPCService;
        public BasketController(IBasketRepository repository, DiscountGRPCService discountGRPCService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _discountGRPCService = discountGRPCService ?? throw new ArgumentNullException(nameof(discountGRPCService));
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> Get(string userName)
        {
            var basket = await _repository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            // TODO: Communicate with Discount.GRPC
            // and Calculate latest prices of a Product into Shopping Cart.
            foreach (var item in basket.Items)
            {
                var coupon = await _discountGRPCService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }
            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        public async Task<IActionResult> DeleteBasket(string userName) {
            await _repository.DeleteBasket(userName);
            return Ok();
        }
    }
}
