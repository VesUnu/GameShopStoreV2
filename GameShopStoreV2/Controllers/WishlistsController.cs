using GameShopStoreV2.Application.ItemServices.Wishlists;
using GameShopStoreV2.Core.Items.Wishlists;
using Microsoft.AspNetCore.Mvc;

namespace GameShopStoreV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistsController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistsController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpPost("UserId")]
        public async Task<IActionResult> AddWishlist(string UserID, AddWishlistRequest addWishlistRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _wishlistService.AddWishlist(UserID, addWishlistRequest);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("UserId")]
        public async Task<IActionResult> GetWishlist(string UserID)
        {
            var result = await _wishlistService.GetWishlist(UserID);
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
        public async Task<IActionResult> DeleteItem(string UserID, [FromBody] DeleteItemRequest deleteItemRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _wishlistService.DeleteItem(UserID, deleteItemRequest);
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
