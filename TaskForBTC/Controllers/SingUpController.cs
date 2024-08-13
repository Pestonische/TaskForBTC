using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using TaskForBTC.Services.Services;
using TaskForBTC.Repositories.Items;
using TaskForBTC.Services.Models;
using TaskForBTC.DAL.Repositories;
using TaskForBTC.Services.IServices;
using Org.BouncyCastle.Tls;
using System.Xml.Linq;
using System;
using System.Diagnostics;
using TaskForBTC.Models;

namespace EmailApp.Controllers
{
    public class SingUpController : Controller
    {
        private readonly SignInManager<UserItem> _signInManager;
        private readonly UserManager<UserItem> _userManager;

        private readonly IEmailService _emailService;

        public SingUpController(IEmailService emailService, UserManager<UserItem> userManager, SignInManager<UserItem> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        #region--Регистрация--
        [HttpGet]
        public IActionResult Register()
        {            
            return View();
        }

        /// <summary>
        /// Регистрация пользователя.
        /// </summary>
        /// <param name="model"> Модель пользователя. </param>
        /// <returns> Страница входа в систему. </returns>
        [HttpPost]
        public async Task<IActionResult> Register(UserModel model)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                UserItem user = new UserItem() { PersonalName = model.PersonalName, UserName = model.UserName, 
                            Email = model.UserEmail, Description = model.UserDescription };
                
                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "user");
                    // генерация токена для пользователя
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "SingUp",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    await _emailService.SendEmailAsync(model.UserEmail, "Confirm your account",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");
                    message = "Для завершения регистрации проверьте электронную почту "+ model.UserEmail + " и перейдите по ссылке, указанной в письме";
                    return RedirectToAction("Login", "SingUp", new { message = message });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
            
        }
        #endregion

        #region--Вход в систему--
        // <summary>
        /// Запуск добавления входа в систему.
        /// </summary>
        /// <returns> Открытие формы. </returns>
        [HttpGet]
        public IActionResult Login(string returnUrl = null, string message = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Events", "Events");
            }
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
            }
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// Вход в систему.
        /// </summary>
        /// <param name="model"> Модель входа. </param>
        /// <returns> Страницу с событиями. </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "Вы не подтвердили свой почтовый адрес");
                        return View(model);
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Events", "Events");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }
        #endregion

        #region--Подтверждение почты--
        /// <summary>
        /// Подтверждение почты.
        /// </summary>
        /// <param name="userId"> Id пользователя. </param>
        /// <param name="code"> Токен пользователя. </param>
        /// <returns> Подтверждение. </returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Error", "SingUp");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Error", "SingUp");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("LogIn", "SingUp");
            else
                return RedirectToAction("Error", "SingUp");
        }
        #endregion

        /// <summary>
        /// Выход из системы.
        /// </summary>
        /// <returns> Страница входа в систему. </returns>
        [HttpGet]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "SingUp");
        }
        /// <summary>
        /// Вызывает ошибку.
        /// </summary>
        /// <returns> Окно с ошибкой. </returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}