using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.Wishlists;
using GameShopStoreV2.Data.EF;
using GameShopStoreV2.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Application.ItemServices.Wishlists
{
    public class WishlistService : IWishlistService
    {
        private readonly GameShopStoreVTwoDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public WishlistService(GameShopStoreVTwoDBContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ResultApi<bool>> AddWishlist(string UserId, AddWishlistRequest addWishlistRequest)
        {
            var getCart = await _context.Wishlists.FirstOrDefaultAsync(x => x.UserId.ToString() == UserId);
            if (getCart != null)
            {
                var orderedgames = await _context.OrderedGames.ToListAsync();
                var bills = await _context.Checkouts.Where(x => x.Cart.UserId.ToString() == UserId).Select(y => y.CartId).ToListAsync();
                if (bills.Count > 0)
                {
                    foreach (var item in bills)
                    {
                        var checkbuy = orderedgames.FirstOrDefault(x => x.CartId == item && x.GameId == addWishlistRequest.GameId);
                        if (checkbuy != null)
                        {
                            return new ErrorResultApi<bool>("This game is already been bought");
                        }
                    }
                }
                var check = await _context.WishesGames.FirstOrDefaultAsync(x => x.Id == getCart.Id && x.GameId == addWishlistRequest.GameId);
                if (check != null)
                {
                    return new ErrorResultApi<bool>("This game is already been bought");
                }
                else
                {
                    WishesGame newgame = new WishesGame()
                    {
                        GameId = addWishlistRequest.GameId,
                        WishId = getCart.Id,
                        DateAdded = DateTime.Now
                    };
                    _context.WishesGames.Add(newgame);
                    await _context.SaveChangesAsync();
                    return new SuccessResultApi<bool>();
                }
            }
            else
            {
                var orderedgames = await _context.OrderedGames.ToListAsync();
                var bills = await _context.Checkouts.Where(x => x.Cart.UserId.ToString() == UserId).Select(y => y.CartId).ToListAsync();
                if (bills.Count > 0)
                {
                    foreach (var item in bills)
                    {
                        var checkbuy = orderedgames.FirstOrDefault(x => x.CartId == item && x.GameId == addWishlistRequest.GameId);
                        if (checkbuy != null)
                        {
                            return new ErrorResultApi<bool>("This game is already been bought");
                        }
                    }
                }
                Wishlist newcart = new Wishlist()
                {
                    UserId = new Guid(UserId),
                };
                WishesGame newgame = new WishesGame()
                {
                    GameId = addWishlistRequest.GameId,
                    Wishlist = newcart,
                    DateAdded = DateTime.Now
                };
                _context.WishesGames.Add(newgame);
                await _context.SaveChangesAsync();
                return new SuccessResultApi<bool>();
            }
        }

        public async Task<ResultApi<bool>> DeleteItem(string UserId, DeleteItemRequest orderItemDelete)
        {
            var orderitem = await _context.WishesGames
                .FirstOrDefaultAsync(x => x.Wishlist.UserId.ToString() == UserId && x.GameId == orderItemDelete.GameId);
            if (orderitem == null)
            {
                return new ErrorResultApi<bool>("This game is not found");
            }
            else
            {
                _context.WishesGames.Remove(orderitem);
                await _context.SaveChangesAsync();
                return new ErrorResultApi<bool>();
            }
        }

        public async Task<ResultApi<List<WishlistViewModel>>> GetWishlist(string UserId)
        {
            var getCart = await _context.WishesGames.Where(x => x.Wishlist.UserId.ToString() == UserId)
                 .Select(x => new WishlistViewModel()
                 {
                     GameId = x.GameId,
                     Name = x.Game.GameName,
                     Price = x.Game.Price,
                     Discount = x.Game.Discount,
                     ImageList = new List<string>(),
                     DateAdded = x.DateAdded,
                     GenreName = new List<string>(),
                     GenreIds = x.Game.GameGenres.Select(y => y.GenreId).ToList(),
                 }).ToListAsync();
            var genres = _context.Genres.AsQueryable();

            foreach (var item in getCart)
            {
                foreach (var genre in item.GenreIds)
                {
                    var name = genres.Where(x => x.GenreId == genre).Select(y => y.GenreName).FirstOrDefault();
                    item.GenreName.Add(name = null!);
                }
            }
            var thumbnailimage = _context.GameImages.AsQueryable();
            foreach (var item in getCart)
            {
                var listgame = thumbnailimage.Where(x => x.GameId == item.GameId).Select(y => y.ImagePath).ToList();
                item.ImageList = listgame;
            }

            if (getCart == null)
            {
                return new ErrorResultApi<List<WishlistViewModel>>("The wishlist is not found");
            }

            return new SuccessResultApi<List<WishlistViewModel>>(getCart);
        }
    }
}
