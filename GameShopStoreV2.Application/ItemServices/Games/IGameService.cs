using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.GameImages;
using GameShopStoreV2.Core.Items.Games;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Application.ItemServices.Games
{
    public interface IGameService
    {
        Task<int> Create(CreatedGameRequest request);

        Task<int> Update(int GameId, EditGameRequest request);

        Task<int> Delete(int GameId);

        Task<bool> UpdatePrice(int GameId, decimal newPrice);

        Task<string> Savefile(IFormFile file);

        Task<PagedResult<GameViewModel>> GetAll(ManageGamePagingRequest request);

        Task<PagedResult<GameViewModel>> GetSaleGames(ManageGamePagingRequest request);

        Task<PagedResult<GameViewModel>> GetAllPaging(ManageGamePagingRequest request);

        Task<int> AddImage(int GameId, CreateGameImageRequest newimage);

        Task<int> RemoveImage(int ImageId);

        Task<int> UpdateImage(int ImageId, UpdateGameImageRequest Image);

        Task<List<GameImageViewModel>> GetListImages(int GameId);

        Task<GameViewModel> GetById(int GameId);

        Task<GameImageViewModel> GetImageById(int ImageId);

        Task<ResultApi<bool>> CategoryAssign(int id, AssignCategory request);

        Task<PagedResult<BestSellerGame>> GetBestSeller(ManageGamePagingRequest request);
    }
}
