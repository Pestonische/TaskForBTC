using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskForBTC.Repositories.Items;
using TaskForBTC.Services.IServices;
using TaskForBTC.Services.Models;
using TaskForBTC.Services.Services;

namespace TaskForBTC.Controllers
{
    public class AdminsController : Controller
    {        
        private readonly UserManager<UserItem> _userManager;

        public AdminsController( UserManager<UserItem> userManager)
        {
            _userManager = userManager;
        }
        [Authorize(Roles = "admin")]
        public IActionResult Admins()
        {
            return View();
        }

        #region--Заполнение таблиц--
        /// <summary>
        /// Заполняет таблицу администраторов.
        /// </summary>
        /// <returns> Администраторов. </returns>
        [HttpGet]
        public async Task<IActionResult> GetAdmins()
        {
            return Json(new { data = await _userManager.GetUsersInRoleAsync("admin")});
        }

        /// <summary>
        /// Заполняет таблицу пользователей.
        /// </summary>
        /// <returns> Пользователей. </returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Json(new { data = await _userManager.GetUsersInRoleAsync("user") });
        }
        #endregion

        #region--Добавление и удаление администратора--
        /// <summary>
        /// Добавляет пользователя к администраторам по id.
        /// </summary>
        /// <param name="userId"> Id пользователя. </param>
        /// <returns> Подтверждение. </returns>
        [HttpPost]
        public async Task<IActionResult> AddAdmin(string userId)
        {
            var result = await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(userId), "admin");
            if (result.Succeeded)
            {
                return Json(new { success = true, message = "" });
            }
            string messege = string.Empty;
            foreach (var error in result.Errors)
            {
                messege += error.Description + "\n";
            }
            return Json(new { success = false, message = messege });
        }

        /// <summary>
        /// Удаляет пользователя из администраторов по id.
        /// </summary>
        /// <param name="adminId"> Id пользователя. </param>
        /// <returns> Подтверждение. </returns>
        [HttpPost]
        public async Task<IActionResult> DeleteAdmin(string adminId)
        {
            var result = await _userManager.RemoveFromRoleAsync(await _userManager.FindByIdAsync(adminId), "admin");
            if (result.Succeeded)
            {
                return Json(new { success = true, message = "" });
            }
            string messege = string.Empty;
            foreach (var error in result.Errors)
            {
                messege += error.Description + "\n";
            }
            return Json(new { success = false, message = messege });
        }
        #endregion
    }
}
