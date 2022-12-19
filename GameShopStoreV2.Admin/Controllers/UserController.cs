using GameShopStoreV2.Admin.Services;
using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.System.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GameShopStoreV2.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IRoleApiClient _roleApiClient;

        public UserController(IUserApiClient userApiClient, IConfiguration configuration, IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _roleApiClient = roleApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var request = new UserPagingRequestGet()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            var data = await _userApiClient.GetUsersPaging(request);
            return View(data.ResultObj);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.RegisterUser(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "You successfully added an user!";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            return View(new DeleteUserRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteUserRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.Delete(request.Id);
            if (result.IsSuccess)
            {
                TempData["result"] = "You successfully deleted an user";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            if (result.IsSuccess)
            {
                var user = result.ResultObj;
                var updateRequest = new UpdateUserRequest()
                {
                    BirthDate = user.BirthDate,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Id = id
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.UpdateUser(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "You successfully updated the user!";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            return View(result.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var rolesAssignRequest = await GetRoleAssignRequest(id);

            return View(rolesAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.RoleAssign(request.Id, request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Successfully assigned a role!";
                return RedirectToAction("Index");
            }
            var rolesAssignRequest = GetRoleAssignRequest(request.Id);
            ModelState.AddModelError("", result.Message);
            return View(rolesAssignRequest);
        }

        private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var userObj = await _userApiClient.GetById(id);
            var roleObj = await _roleApiClient.GetAll();
            var rolesAssignRequest = new RoleAssignRequest();
            foreach (var role in roleObj.ResultObj)
            {
                rolesAssignRequest.Roles.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = userObj.ResultObj.Roles.Contains(role.Name)
                });
            }

            return rolesAssignRequest;
        }
    }
}
