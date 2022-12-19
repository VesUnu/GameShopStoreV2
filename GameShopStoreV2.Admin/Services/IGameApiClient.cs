using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.GameImages;
using GameShopStoreV2.Core.Items.Games;

namespace GameShopStoreV2.Admin.Services
{
    public interface IGameApiClient
    {
        Task<PagedResult<GameViewModel>> GetGamePagings(ManageGamePagingRequest request);

        Task<bool> CreateGame(CreatedGameRequest request);

        Task<ResultApi<bool>> CategoryAssign(int id, AssignCategory request);

        Task<GameViewModel> GetById(int id);

        Task<bool> DeleteGame(int id);

        Task<bool> UpdateGame(EditGameRequest request);

        Task<bool> AddImage(int GameID, CreateGameImageRequest request);
    }
}
