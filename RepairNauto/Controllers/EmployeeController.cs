using AutoRepair.Data;
using AutoRepair.Data.Entities;
using AutoRepair.Helpers;
using AutoRepair.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AutoRepair.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IConverterUserHelper _converterUserHelper;
        private readonly IBlobHelper _blobHelper;


        public EmployeeController(
            IUserHelper userHelper,
            IMailHelper mailHelper,
            IConfiguration configuration,
            IUserRepository userRepository,
            IConverterUserHelper converterUserHelper,
            IBlobHelper blobHelper)

        {
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _configuration = configuration;
            _userRepository = userRepository;
            _converterUserHelper = converterUserHelper;
            _blobHelper = blobHelper;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(_userRepository.GetAllUser());

        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(EmployeeRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {

                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                    }
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                        AgreeTerm = true,
                        IsMechanic = model.IsMechanic,
                        ImageId = imageId
                    };

                    var result = await _userHelper.AddUserAsync(user, "123456");
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    if (user.IsMechanic == false)
                    {
                        await _userHelper.AddUserToRoleAsync(user, "Employee");
                        var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                        await _userHelper.ConfirmEmailAsync(user, token);
                    }
                    else
                    {
                        await _userHelper.AddUserToRoleAsync(user, "Mechanic");
                        var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                        await _userHelper.ConfirmEmailAsync(user, token);
                    }
                }
                if (user.IsMechanic == false)
                {
                    var isInRole = await _userHelper.IsUserInRoleAsync(user, "Employee");
                    if (!isInRole)
                    {
                        await _userHelper.AddUserToRoleAsync(user, "Employee");
                    }
                    else
                    {
                        string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                        string tokenLink = Url.Action("Login", "Account", new
                        {

                        }, protocol: HttpContext.Request.Scheme);

                        Response response = _mailHelper.SendEmail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                                                  $"To allow the user, " +
                            $"<br /><br /><br /> <h1>------- Account Details---------------------------------------------------------------</h1>" +
                            $"<br /><br />" +

                            $"Gmail: {user.Email}" + $"<br />" +
                            $"Password: 123456" +

                            $"<br /><br />IMPORTANT:" +
                            $"<br />Change password!!" +
                            $"<br /><br /><br />Please click in this link:</br></br><a href = \"{tokenLink}\">Go to Login Email</a>");


                        if (response.IsSuccess)
                        {
                            ModelState.AddModelError(string.Empty, "Employee created successfully!" +
                                "\n\nAn email has been sent to employee confirming the account.");

                            await Logout();

                            return View(model);
                        }

                        ModelState.AddModelError(string.Empty, "The user couldn't be logget.");
                    }
                }
                else
                {
                    var isInRole = await _userHelper.IsUserInRoleAsync(user, "Mechanic");
                    if (!isInRole)
                    {
                        await _userHelper.AddUserToRoleAsync(user, "Mechanic");
                    }
                    else
                    {
                        string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                        string tokenLink = Url.Action("Login", "Account", new
                        {

                        }, protocol: HttpContext.Request.Scheme);

                        Response response = _mailHelper.SendEmail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                                                  $"To allow the user, " +
                            $"<br /><br /><br /> <h1>------- Account Details---------------------------------------------------------------</h1>" +
                            $"<br /><br />" +

                            $"Gmail: {user.Email}" + $"<br />" +
                            $"Password: 123456" +

                            $"<br /><br />IMPORTANT:" +
                            $"<br />Change password!!" +
                            $"<br /><br /><br />Please click in this link:</br></br><a href = \"{tokenLink}\">Go to Login Email</a>");


                        if (response.IsSuccess)
                        {
                            ModelState.AddModelError(string.Empty, "Employee created successfully!" +
                                "\n\nAn email has been sent to employee confirming the account.");

                            await Logout();

                            return View(model);
                        }

                        ModelState.AddModelError(string.Empty, "The user couldn't be logget.");
                    }
                }

            }
            ModelState.AddModelError(string.Empty, "The user couldn't be created.");
            return View(model);
        }


        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new EmployeeRegisterViewModel();
            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Username = user.UserName;

            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, user.PasswordHash, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return this.View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);

                    }
                }
            }

            return BadRequest();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {

            }

            return View();

        }


        public IActionResult RecoverPassword()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendEmail(model.Email, "Shop Password Reset", $"<h1>Shop Password Reset</h1>" +
                $"To reset the password click in this link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");

                if (response.IsSuccess)
                {
                    this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
                }

                return this.View();

            }

            return this.View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.UserName);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successful.";
                    return View();
                }

                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found.";
            return View(model);
        }



        public IActionResult NotAuthorized()
        {
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("CarNotFound");
            }

            var user = await _userRepository.GetByIdAsync(id);


            if (user == null)
            {
                return new NotFoundViewResult("CarNotFound");
            }
            foreach (var item in _userRepository.GetAllUser())
            {
                if (id == item.UserName)
                {
                    var model = new EmployeeRegisterViewModel();

                    model.FirstName = item.FirstName;
                    model.LastName = item.LastName;
                    model.Username = item.UserName;
                    model.IsMechanic = item.IsMechanic;

                    return View(model);
                }
            }


            return new NotFoundViewResult("CarNotFound");
        }


        //// POST: Products/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id,UsersViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var user = _converterUserHelper.ToUser(model);


                    //TODO: Modificar para o user que tiver logado
                    //product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _userRepository.UpdateAsync(user);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _userRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        //[HttpPost]
        //[Route("Account/GetCitiesAsync")]
        //public async Task<JsonResult> GetCitiesAsync(int countryId)
        //{
        //    var country = await _countryRepository.GetCountryWithCitiesAsync(countryId);
        //    return Json(country.Cities.OrderBy(c => c.Name));
        //}
    }

}
