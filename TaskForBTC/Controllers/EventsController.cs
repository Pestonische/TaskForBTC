using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System;
using TaskForBTC.Services.IServices;
using TaskForBTC.Services.Models;
using Newtonsoft.Json;
using TaskForBTC.Services.Services;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Threading.Tasks;
using Org.BouncyCastle.Utilities;
using TaskForBTC.Repositories.Items;
using TaskForBTC.Services.Mappers;
using TaskForBTC.DAL.Interfaces;

namespace TaskForBTC.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;
        private readonly UserManager<UserItem> _userManager;
        private readonly IEmailService _emailService;
        private readonly IUsersRepository _usersRepository;
        public EventsController(IEventService eventService, IEmailService emailService, IUsersRepository usersRepository, UserManager<UserItem> userManager)
        {
            _eventService = eventService;
            _emailService = emailService;   
            _userManager = userManager;
            _usersRepository = usersRepository;
        }
        public IActionResult Events()
        {
            return View();
        }

        /// <summary>
        /// Открывает событие для ознакомления.
        /// </summary>
        /// <param name="id"> Id события. </param>
        /// <returns> Событие. </returns>
        [HttpGet]
        public IActionResult OpenEvent(int id)
        {

            return View(_eventService.GetEventsById(id));
        }

        #region--Заполнение таблиц--
        /// <summary>
        /// Заполняет таблицу событий.
        /// </summary>
        /// <returns> События. </returns>
        [HttpGet]
        public JsonResult GetEvents()
        {
            return Json(new { data = _eventService.GetEvents() });
        }

        /// <summary>
        /// Заполняет таблицу пользователей, входящих в состав события.
        /// </summary>
        /// <param name="id"> Id события. </param>
        /// <returns> Пользователей. </returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers(int id)
        {
            List<UserItem> users = new List<UserItem>();
            foreach (var userName in _eventService.GetEventsById(id).UserNames)
            {
                users.Add(await _userManager.FindByNameAsync(userName));
            }
            return Json(new { data = users });
        }

        /// <summary>
        /// Заполняет таблицу дополнительных палей у события.
        /// </summary>
        /// <param name="id"> Id события. </param>
        /// <returns> Дополнительные поля события. </returns>
        [HttpGet]
        public JsonResult GetFields(int id)
        {
            return Json(new { data = _eventService.GetEventsById(id).EventFields });
        }
        #endregion        

        #region--Регистрация пользователя на событе--
        /// <summary>
        /// Отправляет письмо для регистрации пользователя на событие.
        /// </summary>
        /// <param name="id"> Id события. </param>
        /// <returns> Подтверждения. </returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Join(int id)
        {
            EventModel eventModel = _eventService.GetEventsById(id);
            UserItem user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (eventModel.UserNames.Contains(user.UserName))
            {
                return Json(new { success = true, message = "Вы уже зарегистрированы" });
            }
            else
            {
                if (eventModel.NumberOfUsers < eventModel.MaxNumberOfUsers)
                {
                    //Генерация ссылки.
                    var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Events",
                            new { Token = id, UserId = user.Id, Email = user.Email },
                            protocol: HttpContext.Request.Scheme);
                    //Отправка ссылки.
                    await _emailService.SendEmailAsync(user.Email, "Confirm your account",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                    return Json(new { success = true, message = "Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме" });
                }
                return Json(new { success = true, message = "Событие уже заполнено" });
            }
        }

        /// <summary>
        /// Получает подтверждения регистрации пользователя на событие.
        /// </summary>
        /// <param name="Token"> Токен-Id события. </param>
        /// <param name="userId"> Id пользователя. </param>
        /// <param name="Email"> Почтовый адрес пользователя. </param>
        /// <returns> Переход к таблице событий. </returns>
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(int Token, string userId, string Email)
        {
            EventModel eventModel = _eventService.GetEventsById(Token);
            if (eventModel != null && eventModel.NumberOfUsers < eventModel.MaxNumberOfUsers)
            {
                eventModel.NumberOfUsers++;
                eventModel.UserNames.Add((await _userManager.FindByIdAsync(userId)).UserName);
                _eventService.EditEvent(eventModel);
            }

            return RedirectToAction("Events", "Events");            
        }
        #endregion
    }
}
