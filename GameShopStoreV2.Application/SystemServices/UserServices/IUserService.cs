using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.UserImages;
using GameShopStoreV2.Core.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Application.SystemServices.UserServices
{
    public interface IUserService
    {
        Task<ResultApi<LoginResponse>> Authenticate(LoginRequest request);

        Task<ResultApi<bool>> Register(RegisterRequest request);

        Task<ResultApi<bool>> AdminRegister(RegisterRequest request);

        Task<ResultApi<bool>> ConfirmAccount(AccountConfirmRequest request);

        Task<ResultApi<bool>> UpdateUser(UpdateUserRequest request);

        Task<ResultApi<PagedResult<UserViewModel>>> GetUsersPaging(UserPagingRequestGet request);

        Task<ResultApi<UserViewModel>> GetById(Guid id);

        Task<ResultApi<bool>> Delete(Guid id);

        Task<ResultApi<bool>> RoleAssign(Guid id, RoleAssignRequest request);

        Task<ResultApi<bool>> ChangePassword(UpdatePasswordRequest request);

        Task<ResultApi<bool>> ForgotPassword(ForgotPasswordRequest request);

        Task<ResultApi<string>> AddAvatar(string UserId, CreateUserImageRequest request);

        Task<ResultApi<string>> AddThumbnail(string UserId, CreateUserImageRequest request);

        Task<ResultApi<bool>> SendEmail(SendEmailRequest request);
    }
}
