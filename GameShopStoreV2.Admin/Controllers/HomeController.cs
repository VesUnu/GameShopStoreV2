using GameShopStoreV2.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace GameShopStoreV2.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContactApiClient _contactApiClient;

        public HomeController(ILogger<HomeController> logger, IContactApiClient contactApiClient)
        {
            _logger = logger;
            _contactApiClient = contactApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _contactApiClient.GetContacts();

            return View(result.ResultObj);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}