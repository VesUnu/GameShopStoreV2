using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.System.Users;

namespace GameShopStoreV2.Admin.Services
{
    public interface IUserApiClient
    {
        Task<ResultApi<LoginResponse>> Authenticate(LoginRequest request);

        Task<ResultApi<PagedResult<UserViewModel>>> GetUsersPaging(UserPagingRequestGet request);

        Task<ResultApi<bool>> RegisterUser(RegisterRequest request);

        Task<ResultApi<bool>> UpdateUser(UpdateUserRequest request);

        Task<ResultApi<UserViewModel>> GetById(Guid id);

        Task<ResultApi<bool>> Delete(Guid id);

        Task<ResultApi<bool>> RoleAssign(Guid id, RoleAssignRequest request);
    }
}