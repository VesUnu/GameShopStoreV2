using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.GameImages;
using GameShopStoreV2.Core.Items.Games;
using GameShopStoreV2.Utilities.Constants;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace GameShopStoreV2.Admin.Services
{
    public class GameApiClient : BaseApiClient, IGameApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public GameApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> AddImage(int GameID, CreateGameImageRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstant.ApplicationSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstant.ApplicationSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var requestContent = new MultipartFormDataContent();

            if (request.ImageFile != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ImageFile.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ImageFile.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "imagefile", request.ImageFile.FileName);
            }
            var myContent = JsonConvert.SerializeObject(request.isDefault);
            var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");
            requestContent.Add(stringContent, "isdefault");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Caption) ? "" : request.Caption.ToString()), "caption");

            requestContent.Add(new StringContent(request.SortOrder.ToString()), "sortorder");
            requestContent.Add(new StringContent(request.GameId.ToString()), "gameid");
            var response = await client.PostAsync($"/api/games/{GameID}/images", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<ResultApi<bool>> CategoryAssign(int id, AssignCategory request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/games/{id}/genres", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<SuccessResultApi<bool>>(result);

            return JsonConvert.DeserializeObject<ErrorResultApi<bool>>(result);
        }

        public async Task<bool> CreateGame(CreatedGameRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstant.ApplicationSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstant.ApplicationSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var requestContent = new MultipartFormDataContent();

            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }
            if (request.FileGame != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.FileGame.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.FileGame.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "fileGame", request.FileGame.FileName);
            }
            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.GameName) ? "" : request.GameName.ToString()), "gamename");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Publisher) ? "" : request.Publisher.ToString()), "publisher");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? "" : request.Description.ToString()), "description");

            requestContent.Add(new StringContent(request.Discount.ToString()), "discount");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Gameplay) ? "" : request.Gameplay.ToString()), "gameplay");
            requestContent.Add(new StringContent(request.Genre.ToString()), "genre");

            if (request.SystemRequireMin != null)
            {
                var myContent = JsonConvert.SerializeObject(request.SystemRequireMin);
                var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");
                requestContent.Add(stringContent, "systemrequiremin");
            }
            else
            {
                var srmContent = new StringContent("");
                requestContent.Add(srmContent, "systemrequiremin");
            }

            if (request.SystemRequiredRecommend != null)
            {
                var myContent = JsonConvert.SerializeObject(request.SystemRequiredRecommend);
                var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");
                requestContent.Add(stringContent, "systemrequiredrecommend");
            }
            else
            {
                var srrContent = new StringContent("");
                requestContent.Add(srrContent, "systemrequiredrecommend");
            }
            requestContent.Add(new StringContent(request.Status.ToString()), "status");

            var response = await client.PostAsync($"/api/games/", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteGame(int id)
        {
            return await Delete($"/api/games/" + id);
        }

        public async Task<GameViewModel> GetById(int id)
        {
            var data = await GetAsync<GameViewModel>($"/api/games/{id}");

            return data;
        }

        public async Task<PagedResult<GameViewModel>> GetGamePagings(ManageGamePagingRequest request)
        {
            var data = await GetAsync<PagedResult<GameViewModel>>(
              $"/api/games/paging?pageIndex={request.PageIndex}" +
              $"&pageSize={request.PageSize}" +
              $"&keyword={request.Keyword}&GenreId={request.GenreId}");

            return data;
        }

        public async Task<bool> UpdateGame(EditGameRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstant.ApplicationSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstant.ApplicationSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();

            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }

            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? "" : request.Description.ToString()), "description");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Publisher) ? "" : request.Publisher.ToString()), "publisher");
            requestContent.Add(new StringContent(request.Discount.ToString()), "discount");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Gameplay) ? "" : request.Gameplay.ToString()), "gameplay");

            if (request.SystemRequireMin != null)
            {
                var myContent = JsonConvert.SerializeObject(request.SystemRequireMin);
                var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");
                requestContent.Add(stringContent, "systemrequiremin");
            }
            else
            {
                var srmContent = new StringContent("");
                requestContent.Add(srmContent, "systemrequiremin");
            }

            if (request.SystemRequiredRecommend != null)
            {
                var myContent = JsonConvert.SerializeObject(request.SystemRequiredRecommend);
                var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");
                requestContent.Add(stringContent, "systemrequiredrecommend");
            }
            else
            {
                var srrContent = new StringContent("");
                requestContent.Add(srrContent, "systemrequiredrecommend");
            }
            requestContent.Add(new StringContent(request.Status.ToString()), "status");

            var response = await client.PutAsync($"/api/games/" + request.GameId, requestContent);
            return response.IsSuccessStatusCode;
        }
    }
}
