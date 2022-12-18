using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Application.ItemServices.Carts
{
    public interface ICartService
    {
        Task<ResultApi<bool>> AddToCart(string UserId, CreateCart cartCreateRequest);

        Task<ResultApi<List<ResponseOrderedItem>>> GetCart(string UserId);

        Task<ResultApi<bool>> DeleteItem(string UserId, DeleteOrderedItem orderItemDelete);
    }
}
