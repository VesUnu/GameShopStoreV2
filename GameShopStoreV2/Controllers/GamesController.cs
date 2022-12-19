using GameShopStoreV2.Application.ItemServices.Games;
using GameShopStoreV2.Core.Items.GameImages;
using GameShopStoreV2.Core.Items.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GameShopStoreV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService, IWebHostEnvironment webHostEnvironment)
        {
            _gameService = gameService;
        }

        //https:://localhost:port/game
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ManageGamePagingRequest request)
        {
            var games = await _gameService.GetAll(request);
            return Ok(games);
        }

        /* https:://localhost:port/game/paging */

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] ManageGamePagingRequest request)
        {
            var games = await _gameService.GetAllPaging(request);
            if (games == null)
            {
                return NotFound();
            }
            return Ok(games);
        }

        [HttpGet("bestseller")]
        public async Task<IActionResult> GetBestSeller([FromQuery] ManageGamePagingRequest request)
        {
            var games = await _gameService.GetBestSeller(request);
            if (games == null)
            {
                return NotFound();
            }
            return Ok(games);
        }

        [HttpGet("getgamesale")]
        public async Task<IActionResult> GetGameSell([FromQuery] ManageGamePagingRequest request)
        {
            var games = await _gameService.GetSaleGames(request);
            if (games == null)
            {
                return NotFound();
            }
            return Ok(games);
        }

        /* https:://localhost:port/game/1 */

        [HttpGet("{GameId}")]
        public async Task<IActionResult> GetById(int GameID)
        {
            var games = await _gameService.GetById(GameID);
            if (games == null)
            {
                return NotFound();
            }
            return Ok(games);
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetAll([FromQuery] ManageGamePagingRequest request)
        {
            var games = await _gameService.GetAll(request);
            if (games == null)
            {
                return NotFound();
            }
            return Ok(games);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromForm] CreatedGameRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            var gameID = await _gameService.Create(request);
            if (gameID == 0)
            {
                return BadRequest();
            }
            var game = await _gameService.GetById(gameID);
            return Created(nameof(GetById), game);
        }

        [HttpPut("{GameId}")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([FromRoute] int GameID, [FromForm] EditGameRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var affedtedResult = await _gameService.Update(GameID, request);
            if (affedtedResult == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete("{GameId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int GameID)
        {
            var affedtedResult = await _gameService.Delete(GameID);
            if (affedtedResult == 0)
            {
                return BadRequest();
            }

            return Ok();
        }

        //For Price update
        [HttpPatch("price/{GameId}/{newPrice}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdatePrice([FromRoute] int GameID, decimal newPrice)
        {
            var isSuccess = await _gameService.UpdatePrice(GameID, newPrice);
            if (isSuccess == false)
            {
                return BadRequest();
            }

            return Ok();
        }

        //For Image
        [HttpPost("{GameId}/Images")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateImage([FromRoute] int GameID, [FromForm] CreateGameImageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var imageid = await _gameService.AddImage(GameID, request);
                if (imageid == 0)
                {
                    return BadRequest();
                }
                var image = await _gameService.GetImageById(imageid);
                return CreatedAtAction(nameof(GetImageByID), new { ImageID = imageid, GameID = GameID }, image);
            }
        }

        [HttpGet("{GameId}/Images/{ImageId}")]
        public async Task<IActionResult> GetImageByID(int GameID, int ImageID)
        {
            var image = await _gameService.GetImageById(ImageID);
            if (image == null)
            {
                return BadRequest("Could not find this image");
            }
            return Ok(image);
        }

        [HttpGet("{GameID}/images")]
        public async Task<IActionResult> GetListImages(int GameID)
        {
            var ListImage = await _gameService.GetListImages(GameID);
            if (ListImage == null)
            {
                return BadRequest("Could not find this image");
            }
            return Ok(ListImage);
        }

        //Updating the image
        [HttpPut("{GameId}/Images/{ImageId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateImage(int ImageID, [FromForm] UpdateGameImageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameService.UpdateImage(ImageID, request);
            if (result == 0)
                return BadRequest();
            return Ok();
        }

        //Deleteing an image
        [HttpDelete("{GameId}/Images/{ImageId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteImage(int ImageID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameService.RemoveImage(ImageID);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        //Assigning the categories
        [HttpPut("{id}/genres")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CategoryAssign(int id, [FromBody] AssignCategory request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _gameService.CategoryAssign(id, request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
