using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.Checkouts;
using GameShopStoreV2.Core.Items.Games;
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

namespace GameShopStoreV2.Application.ItemServices.Checkouts
{
    public class CheckoutService : ICheckoutService
    {
        private readonly GameShopStoreVTwoDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public CheckoutService(GameShopStoreVTwoDBContext context, UserManager<AppUser> useManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = useManager;
            _signInManager = signInManager;
        }

        public async Task<ResultApi<int>> CheckoutGame(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            decimal total = 0;
            var getCart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId.ToString() == UserId && x.Status.Equals((Status)1));
            if (getCart == null)
            {
                return new ErrorResultApi<int>("An error occured with the game checkout.");
            }
            else
            {
                var gamelist = new List<Game>();
                try
                {
                    gamelist = await _context.OrderedGames.Where(x => x.CartId == getCart.CartId).Select(y => y.Game).ToListAsync();
                }
                catch (Exception ex)
                {
                }
                foreach (var item in gamelist)
                {
                    total = total + (item.Price - (item.Price * item.Discount / 100));
                }
                Checkout newCheckout = new Checkout()
                {
                    Cart = getCart,
                    DatePurchased = DateTime.Now,
                    TotalPrice = total,
                    Username = user.UserName,
                };

                foreach (var item in gamelist)
                {
                    var image = _context.GameImages.Where(x => x.GameId == item.GameId).Select(x => x.ImagePath).FirstOrDefault();
                    var soldgame = new SoldGame()
                    {
                        GameId = item.GameId,
                        GameName = item.GameName,
                        Discount = item.Discount,
                        Price = item.Price,
                        ImagePath = image = null!,
                        Checkout = newCheckout,
                        GameFile = item.FilePath,
                        DatePurchased = newCheckout.DatePurchased,
                    };
                    await _context.SoldGames.AddAsync(soldgame);
                }

                var listgame = await _context.OrderedGames.Where(x => x.CartId == getCart.CartId).ToListAsync();
                foreach (var game in listgame)
                {
                    var wishedgames = await _context.WishesGames.FirstOrDefaultAsync(x => x.GameId == game.GameId && x.Wishlist.UserId.ToString() == UserId);
                    if (wishedgames != null)
                    {
                        _context.WishesGames.Remove(wishedgames);
                    }
                }
                getCart.Status = 0;
                _context.Carts.Update(getCart);

                await _context.SaveChangesAsync();

                return new SuccessResultApi<int>(newCheckout.Id);
            }
        }

        public async Task<ResultApi<List<CheckoutViewModel>>> GetAllBill()
        {
            var listbill = new List<CheckoutViewModel>();
            var newbill = new CheckoutViewModel();
            var alluser = await _context.Users.ToListAsync();
            foreach (var user in alluser)
            {
                var checkouts = _context.Checkouts.Where(x => x.Cart.UserId == user.Id).ToList();

                foreach (var checkout in checkouts)
                {
                    var bill = await _context.Checkouts.Include(x => x.SoldGames).FirstOrDefaultAsync(x => x.Id == checkout.Id);
                    if (bill != null)
                    {
                        newbill = new CheckoutViewModel()
                        {
                            CartId = bill.CartId,
                            TotalPrice = bill.TotalPrice,
                            DatePurchased = bill.DatePurchased,
                            Username = bill.Username,
                            Listgame = new List<GameViewModel>()
                        };
                        
                        foreach (var game in bill.SoldGames)
                        {
                            var soldgame = new GameViewModel()
                            {
                                GameId = game.GameId,
                                Name = game.GameName,
                                Price = game.Price,
                                Discount = game.Discount,
                                DateCreated = game.DatePurchased
                            };
                            soldgame.ListImage.Add(game.ImagePath);
                            newbill.Listgame.Add(soldgame);
                        }

                    }
                    else
                    {
                        return new ErrorResultApi<List<CheckoutViewModel>>("An error occured with the bill.");
                    }

                    listbill.Add(newbill);
                }
            }
            return new SuccessResultApi<List<CheckoutViewModel>>(listbill);
        }

        public async Task<ResultApi<CheckoutViewModel>> GetBill(int checkoutId)
        {
            var bill = await _context.Checkouts.Include(x => x.SoldGames).FirstOrDefaultAsync(x => x.Id == checkoutId);
            if (bill != null)
            {
                var newbill = new CheckoutViewModel()
                {
                    CartId = bill.CartId,
                    TotalPrice = bill.TotalPrice,
                    DatePurchased = bill.DatePurchased,
                    Username = bill.Username,
                    Listgame = new List<GameViewModel>()
                };

                foreach (var game in bill.SoldGames)
                {
                    var soldgame = new GameViewModel()
                    {
                        GameId = game.GameId,
                        Name = game.GameName,
                        Price = game.Price,
                        Discount = game.Discount,
                        DateCreated = game.DatePurchased
                    };
                    soldgame.ListImage.Add(game.ImagePath);
                    newbill.Listgame.Add(soldgame);
                }

                return new SuccessResultApi<CheckoutViewModel>(newbill);
            }
            else
            {
                return new ErrorResultApi<CheckoutViewModel>("The bill is not found.");
            }
        }

        public async Task<PagedResult<GameViewModel>> GetPurchasedGames(string UserId, ManageGamePagingRequest request)
        {
            var query = _context.Checkouts
                .Where(x => x.Cart.UserId.ToString() == UserId).SelectMany(x => x.SoldGames).AsQueryable();
            if (!query.Any())
            {
                return new PagedResult<GameViewModel>()
                {
                    TotalRecords = 0,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = null!
                };
            }
            else
            {
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(x => x.GameName.Contains(request.Keyword));
                }

                int totalrow = await query.CountAsync();
                var data = await query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new GameViewModel()
                    {
                        GameId = x.GameId,
                        Name = x.GameName,
                        Price = x.Price,
                        Discount = x.Discount,
                        ListImage = new List<string>() { x.ImagePath },
                        FileName = x.GameFile,
                        DateCreated = x.DatePurchased
                    }).ToListAsync();
                
                var pagedResult = new PagedResult<GameViewModel>()
                {
                    TotalRecords = totalrow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data
                };
                return pagedResult;
            }
        }
    }
}
