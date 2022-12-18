using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.Carts;
using GameShopStoreV2.Data.Configs;
using GameShopStoreV2.Data.EF;
using GameShopStoreV2.Data.Entities;
using GameShopStoreV2.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Application.ItemServices.Carts
{
    public class CartService : ICartService
    {
        private readonly GameShopStoreVTwoDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public CartService(GameShopStoreVTwoDBContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ResultApi<bool>> AddToCart(string UserId, CreateCart cartCreateRequest)
        {
            var getCart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId.ToString() == UserId && x.Status.Equals((Status)1));
            if (getCart != null)
            {
                var check = await _context.OrderedGames.FirstOrDefaultAsync(x => x.CartId == getCart.CartId && x.GameId == cartCreateRequest.GameId);
                if (check != null)
                {
                    return new ErrorResultApi<bool>("Oops, something went wrong here.");
                }
                else
                {
                    OrderedGame newgame = new OrderedGame()
                    {
                        GameId = cartCreateRequest.GameId,
                        CartId = getCart.CartId,
                        DateAdded = DateTime.Now,
                    };
                    _context.OrderedGames.Add(newgame);
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
                        var check = orderedgames.FirstOrDefault(x => x.CartId == item && x.GameId == cartCreateRequest.GameId);
                        if (check != null)
                        {
                            return new ErrorResultApi<bool>("Oops, something went wrong here");
                        }
                    }
                    Cart cart = new Cart()
                    {
                        UserId = new Guid(UserId),
                        Status = (Status)1,
                        DateCreated = DateTime.Now,
                        DateUpdated = DateTime.Now,
                    };
                    OrderedGame game = new OrderedGame()
                    {
                        GameId = cartCreateRequest.GameId,
                        Cart = cart,
                        DateAdded = DateTime.Now
                    };
                    _context.OrderedGames.Add(game);
                    await _context.SaveChangesAsync();
                    return new SuccessResultApi<bool>();
                }

                Cart newCart = new Cart()
                {
                    UserId = new Guid(UserId),
                    Status = (Status)1,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                };
                OrderedGame newgame = new OrderedGame()
                {
                    GameId = cartCreateRequest.GameId,
                    Cart = newCart,
                    DateAdded = DateTime.Now
                };
                _context.OrderedGames.Add(newgame);
                await _context.SaveChangesAsync();
                return new SuccessResultApi<bool>();
            }
        }

        public async Task<ResultApi<bool>> DeleteItem(string UserId, DeleteOrderedItem orderItemDelete)
        {
            var orderitem = await _context.OrderedGames.FirstOrDefaultAsync(x => x.Cart.UserId.ToString() == UserId && x.GameId == orderItemDelete.GameId);
            if (orderitem == null)
            {
                return new ErrorResultApi<bool>("An error occured with deleting a game.");
            }
            else
            {
                _context.OrderedGames.Remove(orderitem);
                await _context.SaveChangesAsync();
                return new SuccessResultApi<bool>();
            }
        }

        public async Task<ResultApi<List<ResponseOrderedItem>>> GetCart(string UserId)
        {
            var getCart = await _context.OrderedGames.Where(x => x.Cart.UserId.ToString() == UserId && x.Cart.Status.Equals((Status)1))
                .Select(x => new ResponseOrderedItem()
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
                return new ErrorResultApi<List<ResponseOrderedItem>>("There's an error regarding getting an item.");
            }

            return new SuccessResultApi<List<ResponseOrderedItem>>(getCart);
        }
    }
}
