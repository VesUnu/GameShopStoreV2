using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.Categories;
using GameShopStoreV2.Data.EF;
using GameShopStoreV2.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Application.ItemServices.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly GameShopStoreVTwoDBContext _context;
        public CategoryService(GameShopStoreVTwoDBContext context)
        {
            _context = context;
        }

        public async Task<ResultApi<bool>> CreateCategory(CategoryCreate request)
        {
            var check = await _context.Genres.FirstOrDefaultAsync(x => x.GenreName.Contains(request.GenreName.Trim()));
            if (check != null)
            {
                return new ErrorResultApi<bool>("An error occured with creating a category.");
            }
            else
            {
                var newGenre = new Genre()
                {
                    GenreName = request.GenreName
                };
                await _context.Genres.AddAsync(newGenre);
                await _context.SaveChangesAsync();
                return new SuccessResultApi<bool>();
            }
        }

        public async Task<ResultApi<bool>> EditCategory(CategoryEdit request)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.GenreId == request.GenreId);
            if (genre == null)
            {
                return new ErrorResultApi<bool>("An error occured with editing a category.");
            }
            else
            {
                genre.GenreName = request.GenreName.Trim();
                _context.Genres.Update(genre);
                await _context.SaveChangesAsync();
                return new SuccessResultApi<bool>();
            }
        }

        public async Task<List<CategoryViewModel>> GetAll()
        {
            var query = await _context.Genres.Select(x => new CategoryViewModel()
            {
                Id = x.GenreId,
                Name = x.GenreName,
            }).ToListAsync();
            return query;
        }

        public async Task<CategoryViewModel> GetById(int id)
        {
            var query = await _context.Genres.Where(x => x.GenreId == id).FirstOrDefaultAsync();
            if (query != null)
            {
                CategoryViewModel genre = new CategoryViewModel()
                {
                    Name = query.GenreName,
                    Id = query.GenreId
                };
                return genre;
            }
            else
            {
                return null!;
            }
        }
    }
}
