using GameShopStoreV2.Core.Items.Categories;
using GameShopStoreV2.Core.Items.Games;

namespace GameShopStoreV2.Admin.Services
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        public CategoryApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<List<CategoryViewModel>> GetAll()
        {
            return await GetListAsync<CategoryViewModel>("/api/categories");
        }

        public async Task<CreatedGenreRequest> GetById(int id)
        {
            return await GetAsync<CreatedGenreRequest>($"/api/categories/{id}");
        }
    }
}
