using GameShopStoreV2.Application.ItemServices.Carts;
using GameShopStoreV2.Core.Items.Carts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameShopStoreV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("UserId")]
        public async Task<IActionResult> AddToCart(string UserID, [FromBody] CreateCart cartCreateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _cartService.AddToCart(UserID, cartCreateRequest);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("UserId")]
        public async Task<IActionResult> GetCart(string UserID)
        {
            var result = await _cartService.GetCart(UserID);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpDelete("UserId")]
        public async Task<IActionResult> DeleteItem(string UserID, [FromBody] DeleteOrderedItem orderItemDelete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _cartService.DeleteItem(UserID, orderItemDelete);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
