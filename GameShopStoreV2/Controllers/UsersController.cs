using GameShopStoreV2.Application.SystemServices.UserServices;
using GameShopStoreV2.Core.Items.UserImages;
using GameShopStoreV2.Core.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GameShopStoreV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        //For Authentication
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Authenticate(request);
            if (result.ResultObj == null)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
        //For changing the password
        [HttpPost("changepassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.ChangePassword(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        //For forgetting a password
        [HttpPost("forgotpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.ForgotPassword(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        //For Registering an account
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Register(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //For Admin registration
        [HttpPost("adminregister")]
        [AllowAnonymous]
        public async Task<IActionResult> AdminRegister([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.AdminRegister(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //For account update
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.UpdateUser(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //For role assigning
        [HttpPut("{id}/roles")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RoleAssign(Guid Id, [FromBody] RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.RoleAssign(Id, request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] UserPagingRequestGet request)
        {
            var games = await _userService.GetUsersPaging(request);
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var games = await _userService.GetById(id);
            return Ok(games);
        }
        //For deleting an account
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.Delete(id);
            return Ok(result);
        }
        //For adding an avatar
        [Authorize]
        [HttpPost("Avatar/{UserID}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddAvatar(string UserID, [FromForm] CreateUserImageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.AddAvatar(UserID, request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //For adding a thumbnail
        [Authorize]
        [HttpPost("Thumbnail/{UserID}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddThumbnail(string UserID, [FromForm] CreateUserImageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.AddThumbnail(UserID, request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //For confirming an account
        [AllowAnonymous]
        [HttpPost("Confirm")]
        public async Task<IActionResult> ConfirmAccount(AccountConfirmRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.ConfirmAccount(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //For sending an emial request
        [AllowAnonymous]
        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail(SendEmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.SendEmail(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
