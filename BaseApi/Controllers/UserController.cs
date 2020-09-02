using System;
using System.Threading.Tasks;
using BaseApi.Models.JsonApi;
using BaseApi.Models.User;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.InterFace;

namespace BaseApi.Controllers
{

    public class UserController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _uow;

        public UserController(SignInManager<ApplicationUser> signInManager,
            IUnitOfWork uow,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _uow = uow;
            _userManager = userManager;
        }


        public async Task<IActionResult> UserLoginForBotManage(string UserName, string UserType, string Password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UserName) ||
                    string.IsNullOrWhiteSpace(UserType) ||
                    string.IsNullOrWhiteSpace(Password))
                    return BadRequest(new JsonResultContent(false, JsonStatusCode.Warning));

                var result = await _signInManager.PasswordSignInAsync(UserName, Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = _uow.UserRepo.GetUserByName(UserName);

                    var roles = await _userManager.GetRolesAsync(user);
                    bool hasAccess = false;
                    foreach (var role in roles)
                    {
                        if (UserType.Contains(role))
                        {
                            hasAccess = true;
                            break;
                        }
                    }
                    if (!hasAccess)
                    {
                        return Unauthorized(new JsonResultContent(false, JsonStatusCode.Forbidden));
                    }
                    if (!user.IsActive)
                    {
                        return Unauthorized(new JsonResultContent(false, JsonStatusCode.Warning, "User is not active"));
                    }
                    // if login has successfully 
                    return Ok(new JsonResultContent<ApplicationUser>(true, JsonStatusCode.Success, user));

                }
                else
                {
                    return NotFound(new JsonResultContent(false, JsonStatusCode.Warning, "Invalid username or password"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResultContent(false, JsonStatusCode.Warning, "Bad request" + ex.Message));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginDto model)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = _uow.UserRepo.GetUserByName(model.UserName);

                    var roles = await _userManager.GetRolesAsync(user);
                    bool hasAccess = false;
                    foreach (var role in roles)
                    {
                        if (role == "Admin" || role == "Affiliate" || role == "Buyer")
                        {
                            hasAccess = true;
                            break;
                        }
                    }
                    if (!hasAccess)
                    {
                        return Unauthorized(new JsonResultContent(false, JsonStatusCode.Forbidden));
                    }
                    if (!user.IsActive)
                    {
                        return Unauthorized(new JsonResultContent(false, JsonStatusCode.Warning, "User is not active"));
                    }

                    var userDto = new UserDto
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        Roles = roles
                    };

                    // if login has successfully 
                    return Ok(new JsonResultContent<UserDto>(true, JsonStatusCode.Success, userDto));

                }
                else
                {
                    return NotFound(new JsonResultContent(false, JsonStatusCode.Warning, "Invalid username or password"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResultContent(false, JsonStatusCode.Warning, "Bad request" + ex.Message));
            }
        }
    }
}
