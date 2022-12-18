using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.Wishlists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Application.ItemServices.Wishlists
{
    public interface IWishlistService
    {
        Task<ResultApi<bool>> AddWishlist(string UserId, AddWishlistRequest addWishlistRequest);

        Task<ResultApi<List<WishlistViewModel>>> GetWishlist(string UserId);

        Task<ResultApi<bool>> DeleteItem(string UserId, DeleteItemRequest orderItemDelete);
    }
}
