using GameShopStoreV2.Application.CommonServices;
using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.GameImages;
using GameShopStoreV2.Core.Items.Games;
using GameShopStoreV2.Data.EF;
using GameShopStoreV2.Data.Entities;
using GameShopStoreV2.Data.Enums;
using GameShopStoreV2.Utilities.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GameShopStoreV2.Application.ItemServices.Games
{
    public class GameService : IGameService
    {
        private readonly GameShopStoreVTwoDBContext _context;
        private readonly IStorageService _storageService;

        public GameService(GameShopStoreVTwoDBContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> AddImage(int GameId, CreateGameImageRequest newimage)
        {
            var gameimage = new GameImage()
            {
                Caption = newimage.Caption,
                CreatedDate = DateTime.Now,
                isDefault = newimage.isDefault,
                GameId = GameId,
                SortOrder = newimage.SortOrder
            };
            if (newimage.ImageFile != null)
            {
                gameimage.ImagePath = await this.Savefile(newimage.ImageFile);
                gameimage.Filesize = newimage.ImageFile.Length;
            }
            _context.GameImages.Add(gameimage);
            await _context.SaveChangesAsync();
            return gameimage.ImageId;
        }

        public async Task<ResultApi<bool>> CategoryAssign(int id, AssignCategory request)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return new ErrorResultApi<bool>($"Product with that kind of id {id} doesn't exist.");
            }
            foreach (var genre in request.Categories)
            {
                var gameInGenre = await _context.GameGenres
                    .FirstOrDefaultAsync(x => x.GenreId == int.Parse(genre.Id)
                    && x.GameId == id);
                if (gameInGenre != null && genre.Selected == false)
                {
                    _context.GameGenres.Remove(gameInGenre);
                }
                else if (gameInGenre == null && genre.Selected)
                {
                    await _context.GameGenres.AddAsync(new GameGenre()
                    {
                        GenreId = int.Parse(genre.Id),
                        GameId = id
                    });
                }
            }
            await _context.SaveChangesAsync();
            return new SuccessResultApi<bool>();
        }

        public async Task<int> Create(CreatedGameRequest request)
        {
            var game = new Game()
            {
                GameName = request.GameName,
                Price = request.Price,
                Description = request.Description,
                Discount = request.Discount,
                Gameplay = request.Gameplay,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Publisher = request.Publisher,
                Status = (Status)request.Status
            };
            var genrelist = from g in _context.Genres select g;

            var genre = genrelist.FirstOrDefault(x => x.GenreId == request.Genre);

            var newGameGenre = new GameGenre()
            {
                Game = game,
                Genre = genre = null!
            };
            _context.GameGenres.Add(newGameGenre);

            if (request.ThumbnailImage != null)
            {
                game.GameImages = new List<GameImage>()
                {
                     new GameImage()
                     {
                         Caption="Thumbnail Image",
                         CreatedDate = DateTime.Now,
                         Filesize = request.ThumbnailImage.Length,
                         ImagePath=  await this.Savefile(request.ThumbnailImage),
                         isDefault = true,
                         SortOrder = 1,
                     }
                };
            }
            if (request.FileGame != null)
            {
                game.FilePath = await this.Savefile(request.FileGame);
            }
            if (request.SystemRequiredRecommend != null)
            {
                game.RecommendSystemRequirement = new RecommendSystemRequirement()
                {
                    OpSystem = request.SystemRequiredRecommend.OS,
                    Processor = request.SystemRequiredRecommend.Processor,
                    Memory = request.SystemRequiredRecommend.Memory,
                    Graphics = request.SystemRequiredRecommend.Graphics,
                    Storage = request.SystemRequiredRecommend.Storage,
                    AdditionalNotes = request.SystemRequiredRecommend.AdditionalNotes,
                    SoundCard = request.SystemRequiredRecommend.Soundcard
                };
            }
            if (request.SystemRequireMin != null)
            {
                game.MinSystemRequirement = new MinSystemRequirement()
                {
                    OpSystem = request.SystemRequireMin.OS,
                    Processor = request.SystemRequireMin.Processor,
                    Memory = request.SystemRequireMin.Memory,
                    Graphics = request.SystemRequireMin.Graphics,
                    Storage = request.SystemRequireMin.Storage,
                    AdditionalNotes = request.SystemRequireMin.AdditionalNotes,
                    SoundCard = request.SystemRequireMin.Soundcard
                };
            }

            _context.Games.Add(game);

            await _context.SaveChangesAsync();

            return game.GameId;
        }

        public async Task<int> Delete(int GameId)
        {
            var game = await _context.Games.FindAsync(GameId);
            if (game == null)
            {
                throw new ExceptionMessage($"The game cannot be found");
            }
            else
            {
                var thumbnailImages = _context.GameImages.Where(i => i.GameId == GameId);
                foreach (var item in thumbnailImages)
                {
                    await _storageService.DeleteFileAsync(item.ImagePath);
                    _context.GameImages.Remove(item);
                }

                _context.Games.Remove(game);
                return await _context.SaveChangesAsync();
            }
        }

        public async Task<PagedResult<GameViewModel>> GetAll(ManageGamePagingRequest request)
        {
            var query = _context.Games.AsQueryable();

            // filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.GameName.Contains(request.Keyword));
            }

            if (request.GenreId != null)
            {
                query = query.Where(x => x.GameGenres.Any(x => x.GenreId == request.GenreId));
            }
            //paging

            int totalrow = await query.CountAsync();
            var data = await query.Select(x => new GameViewModel()
            {
                DateCreated = x.DateCreated,
                GameId = x.GameId,
                Name = x.GameName,
                Description = x.Description,
                DateUpdated = x.DateUpdated,
                Gameplay = x.Gameplay,
                Discount = x.Discount,
                GenreName = new List<string>(),
                GenreIds = x.GameGenres.Select(y => y.GenreId).ToList(),
                Status = x.Status.ToString(),
                Price = x.Price,
                Publisher = x.Publisher,
                ListImage = new List<string>(),
                SystemRequireMin = new SystemRequireMin()
                {
                    OS = x.MinSystemRequirement.OpSystem,
                    Processor = x.MinSystemRequirement.Processor,
                    Memory = x.MinSystemRequirement.Memory,
                    Graphics = x.MinSystemRequirement.Graphics,
                    Storage = x.MinSystemRequirement.Storage,
                    AdditionalNotes = x.MinSystemRequirement.Storage,
                    Soundcard = x.MinSystemRequirement.SoundCard
                },

                SystemRequiredRecommend = new SystemRequiredRecommend()
                {
                    OS = x.RecommendSystemRequirement.OpSystem,
                    Processor = x.RecommendSystemRequirement.Processor,
                    Memory = x.RecommendSystemRequirement.Memory,
                    Graphics = x.RecommendSystemRequirement.Graphics,
                    Storage = x.RecommendSystemRequirement.Storage,
                    AdditionalNotes = x.RecommendSystemRequirement.Storage,
                    Soundcard = x.RecommendSystemRequirement.SoundCard
                }
            })
                .ToListAsync();
            var genres = _context.Genres.AsQueryable();
            foreach (var item in data)
            {
                foreach (var genre in item.GenreIds)
                {
                    var name = genres.Where(x => x.GenreId == genre).Select(y => y.GenreName).FirstOrDefault();
                    item.GenreName.Add(name = null!);
                }
            }
            var thumbnailimage = _context.GameImages.AsQueryable();
            foreach (var item in data)
            {
                var listgame = thumbnailimage.Where(x => x.GameId == item.GameId).Select(y => y.ImagePath).ToList();
                item.ListImage = listgame;
            }
            var newdata = data.OrderByDescending(x => x.DateCreated).ToList();
            newdata = newdata.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList();

            var pagedResult = new PagedResult<GameViewModel>()
            {
                TotalRecords = totalrow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = newdata
            };
            return pagedResult;
        }

        public async Task<PagedResult<GameViewModel>> GetAllPaging(ManageGamePagingRequest request)
        {
            var query = _context.Games.AsQueryable();

            // filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.GameName.Contains(request.Keyword));
            }

            if (request.GenreId != null)
            {
                query = query.Where(x => x.GameGenres.Any(x => x.GenreId == request.GenreId));
            }
            //paging

            int totalrow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).Select(x => new GameViewModel()
                {
                    DateCreated = x.DateCreated,
                    GameId = x.GameId,
                    Name = x.GameName,
                    Description = x.Description,
                    DateUpdated = x.DateUpdated,
                    Gameplay = x.Gameplay,
                    Discount = x.Discount,
                    Publisher = x.Publisher,
                    GenreName = new List<string>(),
                    GenreIds = x.GameGenres.Select(y => y.GenreId).ToList(),
                    Status = x.Status.ToString(),
                    Price = x.Price,
                    ListImage = new List<string>(),
                    SystemRequireMin = new SystemRequireMin()
                    {
                        OS = x.MinSystemRequirement.OpSystem,
                        Processor = x.MinSystemRequirement.Processor,
                        Memory = x.MinSystemRequirement.Memory,
                        Graphics = x.MinSystemRequirement.Graphics,
                        Storage = x.MinSystemRequirement.Storage,
                        AdditionalNotes = x.MinSystemRequirement.Storage,
                        Soundcard = x.MinSystemRequirement.SoundCard
                    },

                    SystemRequiredRecommend = new SystemRequiredRecommend()
                    {
                        OS = x.RecommendSystemRequirement.OpSystem,
                        Processor = x.RecommendSystemRequirement.Processor,
                        Memory = x.RecommendSystemRequirement.Memory,
                        Graphics = x.RecommendSystemRequirement.Graphics,
                        Storage = x.RecommendSystemRequirement.Storage,
                        AdditionalNotes = x.RecommendSystemRequirement.Storage,
                        Soundcard = x.RecommendSystemRequirement.SoundCard
                    }
                })
                .ToListAsync();
            var genres = _context.Genres.AsQueryable();
            foreach (var item in data)
            {
                foreach (var genre in item.GenreIds)
                {
                    var name = genres.Where(x => x.GenreId == genre).Select(y => y.GenreName).FirstOrDefault();
                    item.GenreName.Add(name = null!);
                }
            }
            var thumbnailimage = _context.GameImages.AsQueryable();
            foreach (var item in data)
            {
                var listgame = thumbnailimage.Where(x => x.GameId == item.GameId).Select(y => y.ImagePath).ToList();
                item.ListImage = listgame;
            }

            var pagedResult = new PagedResult<GameViewModel>()
            {
                TotalRecords = totalrow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<PagedResult<BestSellerGame>> GetBestSeller(ManageGamePagingRequest request)
        {
            var games = await _context.Games.Select(x => new BestSellerGame()
            {
                DateCreated = x.DateCreated,
                GameID = x.GameId,
                Name = x.GameName,
                Description = x.Description,
                DateUpdated = x.DateUpdated,
                Gameplay = x.Gameplay,
                Discount = x.Discount,
                GenreName = new List<string>(),
                GenreIds = x.GameGenres.Select(y => y.GenreId).ToList(),
                Status = x.Status.ToString(),
                Price = x.Price,
                BuyCount = 0,
                Publisher = x.Publisher,
                ListImage = new List<string>(),
                SystemRequireMin = new SystemRequireMin()
                {
                    OS = x.MinSystemRequirement.OpSystem,
                    Processor = x.MinSystemRequirement.Processor,
                    Memory = x.MinSystemRequirement.Memory,
                    Graphics = x.MinSystemRequirement.Graphics,
                    Storage = x.MinSystemRequirement.Storage,
                    AdditionalNotes = x.MinSystemRequirement.Storage,
                    Soundcard = x.MinSystemRequirement.SoundCard
                },

                SystemRequiredRecommend = new SystemRequiredRecommend()
                {
                    OS = x.RecommendSystemRequirement.OpSystem,
                    Processor = x.RecommendSystemRequirement.Processor,
                    Memory = x.RecommendSystemRequirement.Memory,
                    Graphics = x.RecommendSystemRequirement.Graphics,
                    Storage = x.RecommendSystemRequirement.Storage,
                    AdditionalNotes = x.RecommendSystemRequirement.Storage,
                    Soundcard = x.RecommendSystemRequirement.SoundCard
                }
            })
                .ToListAsync();

            var cartids = await _context.Checkouts.Select(x => x.CartId).ToListAsync();
            var orderedgames = await _context.OrderedGames.ToListAsync();
            foreach (var game in games)
            {
                foreach (var cartid in cartids)
                {
                    var check = orderedgames.Where(x => x.CartId == cartid && x.GameId == game.GameID).FirstOrDefault();
                    if (check != null)
                    {
                        game.BuyCount += 1;
                    }
                }
            }
            var genres = _context.Genres.AsQueryable();
            foreach (var item in games)
            {
                foreach (var genre in item.GenreIds)
                {
                    var name = genres.Where(x => x.GenreId == genre).Select(y => y.GenreName).FirstOrDefault();
                    item.GenreName.Add(name = null!);
                }
            }
            var thumbnailimage = _context.GameImages.AsQueryable();
            foreach (var item in games)
            {
                var listgame = thumbnailimage.Where(x => x.GameId == item.GameID).Select(y => y.ImagePath).ToList();
                item.ListImage = listgame;
            }
            int totalrow = games.Count();
            var data = games.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).OrderByDescending(x => x.BuyCount).ToList();
            var pagedResult = new PagedResult<BestSellerGame>()
            {
                TotalRecords = totalrow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<GameViewModel> GetById(int GameId)
        {
            var categories = await (from c in _context.Genres
                                    join pic in _context.GameGenres on c.GenreId equals pic.GenreId
                                    where pic.GameId == GameId
                                    select c.GenreName).ToListAsync();
            var gameView = await _context.Games.Where(x => x.GameId == GameId).Select(x => new GameViewModel()
            {
                GameId = x.GameId,
                Name = x.GameName,
                Gameplay = x.Gameplay,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
                GenreIds = new List<int>(),
                GenreName = categories,
                Description = x.Description,
                Discount = x.Discount,
                Price = x.Price,
                Publisher = x.Publisher,
                ListImage = new List<string>(),
                SystemRequireMin = new SystemRequireMin()
                {
                    OS = x.MinSystemRequirement.OpSystem,
                    Processor = x.MinSystemRequirement.Processor,
                    Memory = x.MinSystemRequirement.Memory,
                    Graphics = x.MinSystemRequirement.Graphics,
                    Storage = x.MinSystemRequirement.Storage,
                    AdditionalNotes = x.MinSystemRequirement.Storage,
                    Soundcard = x.MinSystemRequirement.SoundCard
                },

                SystemRequiredRecommend = new SystemRequiredRecommend()
                {
                    OS = x.RecommendSystemRequirement.OpSystem,
                    Processor = x.RecommendSystemRequirement.Processor,
                    Memory = x.RecommendSystemRequirement.Memory,
                    Graphics = x.RecommendSystemRequirement.Graphics,
                    Storage = x.RecommendSystemRequirement.Storage,
                    AdditionalNotes = x.RecommendSystemRequirement.Storage,
                    Soundcard = x.RecommendSystemRequirement.SoundCard
                }
            }).FirstOrDefaultAsync();

            var genres = await _context.GameGenres.Where(x => x.GameId == gameView.GameId).ToListAsync();

            foreach (var genre in genres)
            {
                gameView.GenreIds.Add(genre.GenreId);
            }
            var thumbnailimage = _context.GameImages.AsQueryable();

            var listgame = thumbnailimage.Where(x => x.GameId == gameView.GameId).Select(y => y.ImagePath).ToList();
            gameView.ListImage = listgame;

            return gameView;
        }

        public async Task<GameImageViewModel> GetImageById(int ImageId)
        {
            var image = await _context.GameImages.FindAsync(ImageId);
            if (image == null)
            {
                throw new ExceptionMessage($"This image could not be found");
            }
            var imageview = new GameImageViewModel()
            {
                Path = image.ImagePath,
                Caption = image.Caption,
                CreatedDate = image.CreatedDate,
                FileSize = image.Filesize,
                ImageId = image.ImageId,
                isDefault = image.isDefault,
                GameId = image.GameId,
                SortOrder = image.SortOrder,
            };
            return imageview;
        }

        public async Task<List<GameImageViewModel>> GetListImages(int GameId)
        {
            return await _context.GameImages.Where(x => x.GameId == GameId)
                .Select(i => new GameImageViewModel()
                {
                    Path = i.ImagePath,
                    Caption = i.Caption,
                    CreatedDate = i.CreatedDate,
                    FileSize = i.Filesize,
                    ImageId = i.ImageId,
                    isDefault = i.isDefault,
                    GameId = GameId,
                    SortOrder = i.SortOrder,
                }).ToListAsync();
        }

        public async Task<PagedResult<GameViewModel>> GetSaleGames(ManageGamePagingRequest request)
        {
            var query = _context.Games.Where(x => x.Discount > 0);

            int totalrow = await query.CountAsync();
            var data = await query.Select(x => new GameViewModel()
            {
                DateCreated = x.DateCreated,
                GameId = x.GameId,
                Name = x.GameName,
                Description = x.Description,
                DateUpdated = x.DateUpdated,
                Gameplay = x.Gameplay,
                Discount = x.Discount,
                GenreName = new List<string>(),
                GenreIds = x.GameGenres.Select(y => y.GenreId).ToList(),
                Status = x.Status.ToString(),
                Price = x.Price,
                Publisher = x.Publisher,
                ListImage = new List<string>(),
                SystemRequireMin = new SystemRequireMin()
                {
                    OS = x.MinSystemRequirement.OpSystem,
                    Processor = x.MinSystemRequirement.Processor,
                    Memory = x.MinSystemRequirement.Memory,
                    Graphics = x.MinSystemRequirement.Graphics,
                    Storage = x.MinSystemRequirement.Storage,
                    AdditionalNotes = x.MinSystemRequirement.Storage,
                    Soundcard = x.MinSystemRequirement.SoundCard
                },

                SystemRequiredRecommend = new SystemRequiredRecommend()
                {
                    OS = x.RecommendSystemRequirement.OpSystem,
                    Processor = x.RecommendSystemRequirement.Processor,
                    Memory = x.RecommendSystemRequirement.Memory,
                    Graphics = x.RecommendSystemRequirement.Graphics,
                    Storage = x.RecommendSystemRequirement.Storage,
                    AdditionalNotes = x.RecommendSystemRequirement.Storage,
                    Soundcard = x.RecommendSystemRequirement.SoundCard
                }
            })
                .ToListAsync();
            var genres = _context.Genres.AsQueryable();
            foreach (var item in data)
            {
                foreach (var genre in item.GenreIds)
                {
                    var name = genres.Where(x => x.GenreId == genre).Select(y => y.GenreName).FirstOrDefault();
                    item.GenreName.Add(name = null!);
                }
            }
            var thumbnailimage = _context.GameImages.AsQueryable();
            foreach (var item in data)
            {
                var listgame = thumbnailimage.Where(x => x.GameId == item.GameId).Select(y => y.ImagePath).ToList();
                item.ListImage = listgame;
            }
            var newdata = data.OrderByDescending(x => x.Discount).ToList();
            newdata = newdata.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList();

            var pagedResult = new PagedResult<GameViewModel>()
            {
                TotalRecords = totalrow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = newdata
            };
            return pagedResult;
        }

        public async Task<int> RemoveImage(int ImageId)
        {
            var gameimage = await _context.GameImages.FindAsync(ImageId);
            if (gameimage != null)
            {
                _context.GameImages.Remove(gameimage);
                return await _context.SaveChangesAsync();
            }
            else
            {
                throw new ExceptionMessage($"An image with that kind of id {ImageId} could not be found!");
            }
        }

        public async Task<string> Savefile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var filename = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), filename);
            return filename;
        }

        public async Task<int> Update(int GameId, EditGameRequest request)
        {
            var game = await _context.Games.Where(x => x.GameId == GameId)
                .Include(y => y.MinSystemRequirement)
                .Include(g => g.RecommendSystemRequirement)
                .FirstOrDefaultAsync();
            if (game == null)
            {
                throw new ExceptionMessage($"The game cannot be found!");
            }
            else
            {
                game.GameName = request.Name;

                game.Discount = request.Discount;
                game.Description = request.Description;
                game.Gameplay = request.Gameplay;
                game.DateUpdated = DateTime.Now;
                game.Status = (Status)request.Status;
                game.Publisher = request.Publisher;
                game.Price = request.Price;
                if (request.SystemRequiredRecommend != null)
                {
                    game.RecommendSystemRequirement.OpSystem = request.SystemRequiredRecommend.OS;
                    game.RecommendSystemRequirement.Memory = request.SystemRequiredRecommend.Memory;
                    game.RecommendSystemRequirement.Processor = request.SystemRequiredRecommend.Processor;
                    game.RecommendSystemRequirement.Graphics = request.SystemRequiredRecommend.Graphics;
                    game.RecommendSystemRequirement.Storage = request.SystemRequiredRecommend.Storage;
                    game.RecommendSystemRequirement.AdditionalNotes = request.SystemRequiredRecommend.AdditionalNotes;
                    game.RecommendSystemRequirement.SoundCard = request.SystemRequiredRecommend.Soundcard;
                }
                if (request.SystemRequireMin != null)
                {
                    game.MinSystemRequirement.OpSystem = request.SystemRequireMin.OS;
                    game.MinSystemRequirement.Memory = request.SystemRequireMin.Memory;
                    game.MinSystemRequirement.Processor = request.SystemRequireMin.Processor;
                    game.MinSystemRequirement.Graphics = request.SystemRequireMin.Graphics;
                    game.MinSystemRequirement.Storage = request.SystemRequireMin.Storage;
                    game.MinSystemRequirement.AdditionalNotes = request.SystemRequireMin.AdditionalNotes;
                    game.MinSystemRequirement.SoundCard = request.SystemRequireMin.Soundcard;
                }
                if (request.ThumbnailImage != null)
                {
                    var thumbnailImage = await _context.GameImages
                        .FirstOrDefaultAsync(i => i.isDefault == true && i.GameId == request.GameId);
                    if (thumbnailImage != null)
                    {
                        thumbnailImage.Filesize = request.ThumbnailImage.Length;
                        thumbnailImage.ImagePath = await this.Savefile(request.ThumbnailImage);
                        _context.GameImages.Update(thumbnailImage);
                    }
                    else
                    {
                        game.GameImages = new List<GameImage>()       {
                        new GameImage()
                         {
                         Caption="Thumbnail Image",
                         CreatedDate = DateTime.Now,
                         Filesize = request.ThumbnailImage.Length,
                         ImagePath=  await this.Savefile(request.ThumbnailImage),
                         isDefault = true,
                         SortOrder = 1,
                          }
                        };
                    }
                }
                if (request.FileGame != null)
                {
                    game.FilePath = await this.Savefile(request.FileGame);
                }
                _context.Games.Update(game);
                return await _context.SaveChangesAsync();
            }
        }

        public async Task<int> UpdateImage(int ImageId, UpdateGameImageRequest Image)
        {
            var gameimage = await _context.GameImages.FindAsync(ImageId);
            if (gameimage != null)
            {
                gameimage.SortOrder = Image.SortOrder;
                gameimage.isDefault = Image.isDefault;
                gameimage.Caption = Image.Caption;
                if (Image.ImageFile != null)
                {
                    gameimage.ImagePath = await this.Savefile(Image.ImageFile);
                    gameimage.Filesize = Image.ImageFile.Length;
                }
            }
            else
            {
                throw new ExceptionMessage($"An image with that kind of id {ImageId} could not be found!");
            }
            _context.GameImages.Update(gameimage);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int GameId, decimal newPrice)
        {
            var game = await _context.Games.FindAsync(GameId);
            if (game == null)
            {
                return false;
            }
            else
            {
                game.Price = newPrice;
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
