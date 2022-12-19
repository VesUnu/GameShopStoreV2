using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.Contacts;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace GameShopStoreV2.Admin.Services
{
    public class ContactApiClient : IContactApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContactApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)

        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResultApi<List<ContactViewModel>>> GetContacts()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration.GetValue<string>("BaseAddress"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync($"api/contact");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<SuccessResultApi<List<ContactViewModel>>>(body);
            }
            return JsonConvert.DeserializeObject<ErrorResultApi<List<ContactViewModel>>>(body);
        }
    }
}
