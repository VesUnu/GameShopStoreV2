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
        Task<ResultApi<bool>> AddToCart(string UserID, CreateCart cartCreateRequest);

        Task<ResultApi<List<ResponseOrderedItem>>> GetCart(string UserID);

        Task<ResultApi<bool>> DeleteItem(string UserID, DeleteOrderedItem orderItemDelete);
    }
}
