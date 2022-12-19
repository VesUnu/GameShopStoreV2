using GameShopStoreV2.Admin.Services;
using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.GameImages;
using GameShopStoreV2.Core.Items.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace GameShopStoreV2.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class GameController : Controller
    {
        private readonly IGameApiClient _gameApiClient;
        private readonly ICategoryApiClient _categoryApiClient;

        public GameController(IGameApiClient gameApiClient, ICategoryApiClient categoryApiClient)
        {
            _gameApiClient = gameApiClient;
            _categoryApiClient = categoryApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string keyword, int? GenreId, int pageIndex = 1, int pageSize = 5)
        {
            var request = new ManageGamePagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                GenreId = GenreId,
            };

            var games = await _gameApiClient.GetGamePagings(request);
            ViewBag.Keyword = keyword;

            var categories = await _categoryApiClient.GetAll();
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = GenreId.HasValue && GenreId.Value == x.Id
            });

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(games);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryApiClient.GetAll();

            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreatedGameRequest request, int GenreId)
        {
            if (!ModelState.IsValid)
                return View(request);

            request.Genre = GenreId;
            var result = await _gameApiClient.CreateGame(request);
            if (result)
            {
                TempData["result"] = "New product was successfully added.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to add a product");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryAssign(int id)
        {
            var roleAssignRequest = await GetCategoryAssignRequest(id);
            return View(roleAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryAssign(AssignCategory request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _gameApiClient.CategoryAssign(request.Id, request);

            if (result.IsSuccess)
            {
                TempData["result"] = "Successfully added a category";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await GetCategoryAssignRequest(request.Id);

            return View(roleAssignRequest);
        }

        private async Task<AssignCategory> GetCategoryAssignRequest(int id)
        {
            var productObj = await _gameApiClient.GetById(id);
            var categories = await _categoryApiClient.GetAll();
            var categoryAssignRequest = new AssignCategory();
            foreach (var role in categories)
            {
                categoryAssignRequest.Categories.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = productObj.GenreName.Contains(role.Name)
                });
            }
            return categoryAssignRequest;
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new DeletedGameRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeletedGameRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _gameApiClient.DeleteGame(request.Id);
            if (result)
            {
                TempData["result"] = "Product was deleted successfully";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to delete a product");
            return View(request);
        }

        [HttpGet]
        public IActionResult AddImage(int id)
        {
            return View(new CreateGameImageRequest()
            {
                GameId = id
            });
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddImage(int GameID, [FromForm] CreateGameImageRequest request)
        {
            var result = await _gameApiClient.AddImage(GameID, request);
            if (result)
            {
                TempData["result"] = "An image was successfully added.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to add an image.");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var game = await _gameApiClient.GetById(id);
            var editVm = new EditGameRequest()
            {
                GameId = game.GameId,
                Description = game.Description,
                Name = game.Name,
                Price = game.Price,
                Discount = game.Discount,
                Gameplay = game.Gameplay,
                SystemRequireMin = game.SystemRequireMin,
                SystemRequiredRecommend = game.SystemRequiredRecommend,
            };
            return View(editVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] EditGameRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _gameApiClient.UpdateGame(request);
            if (result)
            {
                TempData["result"] = "The product was successfully updated";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to update the product");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _gameApiClient.GetById(id);
            return View(result);
        }
    }
}
