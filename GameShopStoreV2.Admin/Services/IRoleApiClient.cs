using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.System.Roles;

namespace GameShopStoreV2.Admin.Services
{
    public interface IRoleApiClient
    {
        Task<ResultApi<List<RoleViewModel>>> GetAll();
    }
}
