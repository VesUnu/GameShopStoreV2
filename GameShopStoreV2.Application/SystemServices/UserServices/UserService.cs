using GameShopStoreV2.Application.CommonServices;
using GameShopStoreV2.Core.Common;
using GameShopStoreV2.Core.Items.UserImages;
using GameShopStoreV2.Core.System.Users;
using GameShopStoreV2.Data.EF;
using GameShopStoreV2.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Application.SystemServices.UserServices
{
    public class UserService : IUserService
    {
        private readonly GameShopStoreVTwoDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;

        public UserService(GameShopStoreVTwoDBContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration config, IStorageService storageService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _storageService = storageService;
        }

        public async Task<ResultApi<string>> AddAvatar(string UserId, CreateUserImageRequest request)
        {
            if (request.ImageFile != null)
            {
                var getAvatar = await _context.AvatarUsers.Where(x => x.UserId.ToString() == UserId).FirstOrDefaultAsync();
                if (getAvatar == null)
                {
                    var newAvatar = new AvatarUser()
                    {
                        UserId = new Guid(UserId),
                        DateUpdate = DateTime.Now,
                        ImagePath = await this.Savefile(request.ImageFile)
                    };
                    _context.AvatarUsers.Add(newAvatar);
                    await _context.SaveChangesAsync();
                    return new SuccessResultApi<string>(newAvatar.ImagePath);
                }
                else
                {
                    getAvatar.DateUpdate = DateTime.Now;
                    getAvatar.ImagePath = await this.Savefile(request.ImageFile);
                    _context.AvatarUsers.Update(getAvatar);
                    await _context.SaveChangesAsync();
                    return new SuccessResultApi<string>(getAvatar.ImagePath);
                }
            }
            else
            {
                return new ErrorResultApi<string>("There are no pictures");
            }
        }

        public async Task<string> Savefile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var filename = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), filename);
            return filename;
        }

        public async Task<ResultApi<string>> AddThumbnail(string UserId, CreateUserImageRequest request)
        {
            if (request.ImageFile != null)
            {
                var getThumbnail = await _context.ThumbnailUsers.Where(x => x.UserId.ToString() == UserId).FirstOrDefaultAsync();
                if (getThumbnail == null)
                {
                    var newThumbnail = new ThumbnailUser()
                    {
                        UserId = new Guid(UserId),
                        DateUpdate = DateTime.Now,
                        ImagePath = await this.Savefile(request.ImageFile)
                    };
                    _context.ThumbnailUsers.Add(newThumbnail);
                    await _context.SaveChangesAsync();
                    return new SuccessResultApi<string>(newThumbnail.ImagePath);
                }
                else
                {
                    getThumbnail.DateUpdate = DateTime.Now;
                    getThumbnail.ImagePath = await this.Savefile(request.ImageFile);
                    _context.ThumbnailUsers.Update(getThumbnail);
                    await _context.SaveChangesAsync();
                    return new SuccessResultApi<string>(getThumbnail.ImagePath);
                }
            }
            else
            {
                return new ErrorResultApi<string>("There are no pictures");
            }
        }

        public async Task<ResultApi<bool>> AdminRegister(RegisterRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                return new ErrorResultApi<bool>("This account is already existing");
            }
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return new ErrorResultApi<bool>("This email is already existing");
            }
            var useravatar = new AvatarUser()
            {
                ImagePath = "imgnotfound.jpg",
            };
            var userthumbnail = new ThumbnailUser()
            {
                ImagePath = "imgnotfound.jpg"
            };
            Random r = new Random();
            int randNum = r.Next(1000000);
            string sixDigitNumber = randNum.ToString("D6");
            user = new AppUser()
            {
                UserName = request.UserName,
                Email = request.Email,
                AvatarUser = useravatar,
                ThumbnailUser = userthumbnail,
                IsConfirmed = true,
                CodeConfirm = sixDigitNumber,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return new SuccessResultApi<bool>();
            }
            else
            {
                return new ErrorResultApi<bool>("The registration process has failed");
            }
        }

        public async Task<ResultApi<LoginResponse>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new ErrorResultApi<LoginResponse>("This account doesn't exist");
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return new ErrorResultApi<LoginResponse>("Incorrect login");
            }
            else
            {
                if (user.IsConfirmed == false)
                {
                    await _signInManager.SignOutAsync();
                    return new ErrorResultApi<LoginResponse>("This account doesn't exist");
                }
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {   new Claim("NameIdentifier",user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Role,String.Join(";",roles))
             };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            string _issuer = _config.GetValue<string>("Tokens:Issuer");
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _issuer,
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            LoginResponse response = new LoginResponse()
            {
                UserId = user.Id.ToString(),
                IsConfirmed = user.IsConfirmed,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
            return new SuccessResultApi<LoginResponse>(response);
        }

        public async Task<ResultApi<bool>> ChangePassword(UpdatePasswordRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new ErrorResultApi<bool>("Incorrect login");
            }
            var hasher = new PasswordHasher<AppUser>();
            //var haspassword = hasher.HashPassword(null, request.Password);
            var check = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (check.Equals((PasswordVerificationResult)0))
            {
                return new ErrorResultApi<bool>("Incorrect password");
            }
            else
            {
                var newpassword = hasher.HashPassword(null!, request.NewPassword);
                user.PasswordHash = newpassword;
                await _userManager.UpdateAsync(user);
                return new SuccessResultApi<bool>();
            }
        }

        public async Task<ResultApi<bool>> ConfirmAccount(AccountConfirmRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new ErrorResultApi<bool>("Incorrect input information. Sorry about that");
            }
            else
            {
                var hasher = new PasswordHasher<AppUser>();
                var check = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
                if (check.Equals((PasswordVerificationResult)0))
                {
                    return new ErrorResultApi<bool>("Incorrect input information. Sorry about that");
                }
                else
                {
                    if (user.CodeConfirm == request.CodeConfirm)
                    {
                        if (user.IsConfirmed == false)
                        {
                            user.IsConfirmed = true;

                            var result = await _userManager.UpdateAsync(user);
                            if (result.Succeeded)
                            {
                                return new SuccessResultApi<bool>();
                            }
                            else
                            {
                                return new ErrorResultApi<bool>("An error has occured during the account confirmation. Try again, later");
                            }
                        }
                        else
                        {
                            return new ErrorResultApi<bool>("Success, your account is activated");
                        }
                    }
                    else
                    {
                        return new ErrorResultApi<bool>("Incorrect input information. Sorry about that");
                    }
                }
            }
        }

        public async Task<ResultApi<bool>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ErrorResultApi<bool>("This username doesn't exist");
            }
            var reult = await _userManager.DeleteAsync(user);
            if (reult.Succeeded)
                return new SuccessResultApi<bool>();

            return new ErrorResultApi<bool>("Deletion process has failed");
        }

        public async Task<ResultApi<bool>> ForgotPassword(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new ErrorResultApi<bool>("This account doesn't exist");
            }
            if (!user.Email.Equals(request.Email))
            {
                return new ErrorResultApi<bool>("This email doesn't exist");
            }
            if (!user.CodeConfirm.Equals(request.CodeConfirm))
            {
                return new ErrorResultApi<bool>("Incorrect code");
            }
            else
            {
                var hasher = new PasswordHasher<AppUser>();
                var newpassword = hasher.HashPassword(null!, request.NewPassword);
                user.PasswordHash = newpassword;
                await _userManager.UpdateAsync(user);
                return new SuccessResultApi<bool>();
            }
        }

        public async Task<ResultApi<UserViewModel>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ErrorResultApi<UserViewModel>("This username doesn't exist");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userAvatar = await _context.AvatarUsers.FirstOrDefaultAsync(x => x.UserId == id);
            var userThumbnail = await _context.ThumbnailUsers.FirstOrDefaultAsync(x => x.UserId == id);
            var userVm = new UserViewModel()
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                BirthDate = user.BirthDate,
                Id = user.Id,
                LastName = user.LastName,
                UserName = user.UserName,
                Roles = roles,
                AvatarPath = userAvatar.ImagePath,
                ThumbnailPath = userThumbnail.ImagePath
            };

            return new SuccessResultApi<UserViewModel>(userVm);
        }

        public async Task<ResultApi<PagedResult<UserViewModel>>> GetUsersPaging(UserPagingRequestGet request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword) || x.PhoneNumber.Contains(request.Keyword));
            }
            int totalrow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                 .Take(request.PageSize)
                 .Select(x => new UserViewModel()
                 {
                     Email = x.Email,
                     PhoneNumber = x.PhoneNumber,
                     UserName = x.UserName,
                     FirstName = x.FirstName,
                     Id = x.Id,
                     LastName = x.LastName
                 }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<UserViewModel>()
            {
                TotalRecords = totalrow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new SuccessResultApi<PagedResult<UserViewModel>>(pagedResult);
        }

        public async Task<ResultApi<bool>> Register(RegisterRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                return new ErrorResultApi<bool>("The account is already existing.");
            }
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return new ErrorResultApi<bool>("The email is already existing");
            }
            var useravatar = new AvatarUser()
            {
                ImagePath = "imgnotfound.jpg",
            };
            var userthumbnail = new ThumbnailUser()
            {
                ImagePath = "imgnotfound.jpg"
            };
            Random r = new Random();
            int randNum = r.Next(1000000);
            string sixDigitNumber = randNum.ToString("D6");
            user = new AppUser()
            {
                UserName = request.UserName,
                Email = request.Email,
                AvatarUser = useravatar,
                ThumbnailUser = userthumbnail,
                IsConfirmed = false,
                CodeConfirm = sixDigitNumber
            };

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("gameshop1901@gmail.com");
                mail.To.Add(user.Email);
                mail.Subject = "Confirm Account";
                mail.Body = $@"<html>
                      <body>
                      <p>Dear {user.UserName},</p>
                      <p>Thank you for joining us,here is your confirmation code {user.CodeConfirm}</p>
                      <p>Sincerely,<br>-STEM</br></p>
                      </body>
                      </html>
                     ";
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("gameshop1901@gmail.com", "yfvcjmebvgggeult");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return new SuccessResultApi<bool>();
            }
            else
            {
                return new ErrorResultApi<bool>("The registration process has failed.");
            }
        }

        public async Task<ResultApi<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ErrorResultApi<bool>("This account doesn't exist");
            }
            var removedRoles = request.Roles.Where(x => x.Selected == false).Select(x => x.Name).ToList();
            foreach (var roleName in removedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == true)
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName);
                }
            }
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            var addedRoles = request.Roles.Where(x => x.Selected).Select(x => x.Name).ToList();
            foreach (var roleName in addedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == false)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            return new SuccessResultApi<bool>();
        }

        public async Task<ResultApi<bool>> SendEmail(SendEmailRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("gameshop1901@gmail.com");
                    mail.To.Add(user.Email);
                    mail.Subject = "Confirm Account";
                    mail.Body = $@"<html>
                      <body>
                      <p>Dear {user.UserName},</p>
                      <p>Looking for a confirmation code? Here it is {user.CodeConfirm}</p>
                      <p>Sincerely,<br>-STEM</br></p>
                      </body>
                      </html>
                     ";
                    mail.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("gameshop1901@gmail.com", "yfvcjmebvgggeult");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                return new SuccessResultApi<bool>();
            }
            else
            {
                return new ErrorResultApi<bool>("This account doesn't exist");
            }
        }

        public async Task<ResultApi<bool>> UpdateUser(UpdateUserRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != request.Id))
            {
                return new ErrorResultApi<bool>("The email is already existing");
            }
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            user.BirthDate = request.BirthDate;
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new SuccessResultApi<bool>();
            }
            else
            {
                return new ErrorResultApi<bool>("The update process has failed");
            }
        }
    }
}
