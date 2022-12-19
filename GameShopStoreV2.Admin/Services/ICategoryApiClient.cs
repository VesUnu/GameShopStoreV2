using GameShopStoreV2.Core.Items.Categories;
using GameShopStoreV2.Core.Items.Games;

namespace GameShopStoreV2.Admin.Services
{
    public interface ICategoryApiClient
    {
        Task<CreatedGenreRequest> GetById(int id);
        Task<List<CategoryViewModel>> GetAll();
    }
}