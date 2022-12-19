using GameShopStoreV2.Application.ItemServices.Checkouts;
using GameShopStoreV2.Core.Items.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameShopStoreV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutsController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("UserId")]
        public async Task<IActionResult> Checkout(string UserID)
        {
            var result = await _checkoutService.CheckoutGame(UserID);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("CheckoutId")]
        public async Task<IActionResult> GetBill(int CheckoutID)
        {
            var result = await _checkoutService.GetBill(CheckoutID);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("AllBill")]
        public async Task<IActionResult> GetAllBill()
        {
            var result = await _checkoutService.GetAllBill();
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("paging/{UserId}")]
        public async Task<IActionResult> GetAllPaging(string UserID, [FromQuery] ManageGamePagingRequest request)
        {
            var games = await _checkoutService.GetPurchasedGames(UserID, request);
            if (games == null)
            {
                return NotFound();
            }
            return Ok(games);
        }
    }
}
