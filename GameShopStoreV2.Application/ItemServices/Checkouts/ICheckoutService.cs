using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.Checkouts;
using GameShopStoreV2.Core.Items.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Application.ItemServices.Checkouts
{
    public interface ICheckoutService
    {
        Task<ResultApi<int>> CheckoutGame(string UserId);

        Task<PagedResult<GameViewModel>> GetPurchasedGames(string UserId, ManageGamePagingRequest request);

        Task<ResultApi<CheckoutViewModel>> GetBill(int checkoutId);

        Task<ResultApi<List<CheckoutViewModel>>> GetAllBill();
    }
}
