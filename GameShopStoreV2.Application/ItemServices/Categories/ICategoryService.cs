using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Application.ItemServices.Categories
{
    public interface ICategoryService
    {
        Task<CategoryViewModel> GetById(int id);

        Task<List<CategoryViewModel>> GetAll();

        Task<ResultApi<bool>> CreateCategory(CategoryCreate request);

        Task<ResultApi<bool>> EditCategory(CategoryEdit request);
    }
}
