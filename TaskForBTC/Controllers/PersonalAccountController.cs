using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using TaskForBTC.DAL.Interfaces;
using TaskForBTC.Repositories.Items;
using TaskForBTC.Services.IServices;
using TaskForBTC.Services.Mappers;
using TaskForBTC.Services.Models;

namespace TaskForBTC.Controllers
{
    public class PersonalAccountController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly UserManager<UserItem> _userManager;
        private readonly SignInManager<UserItem> _signInManager;
        public PersonalAccountController(IUsersRepository usersRepository, SignInManager<UserItem> signInManager, UserManager<UserItem> userManager)
        {
            _usersRepository = usersRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Открывает личный кабинет, отправляя в него текущего пользователя.
        /// </summary>
        /// <returns> Текущего пользователя. </returns>
        [HttpGet]
        public async Task<IActionResult> PersonalAccount()
        {
            return View(PersonalAccountMapper.Map(await _userManager.FindByNameAsync(User.Identity.Name)));
        }

        /// <summary>
        /// Редактирует пользователя.
        /// </summary>
        /// <param name="model"> Модель личного кабинета. </param>
        /// <returns> Обновление страницы. </returns>
        [HttpPost]
        public async Task<IActionResult> PersonalAccount(PersonalAccountModel model)
        {
            if (ModelState.IsValid)
            {
                UserItem user = await _userManager.FindByNameAsync(User.Identity.Name);
                _usersRepository.SetDescription(user, model.Description);
                _usersRepository.SetPersonalName(user, model.PersonalName);
                var result = await _userManager.ChangeEmailAsync(user, model.Email, await _userManager.GenerateChangeEmailTokenAsync(user, model.Email));
                if (result.Succeeded)
                {
                    return RedirectToAction("PersonalAccount", "PersonalAccount");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return RedirectToAction("PersonalAccount", "PersonalAccount");
        }

        /// <summary>
        /// Изменение пароля.
        /// </summary>
        /// <param name="newPassword"> Новый пароль. </param>
        /// <param name="currentPasword"> Текущий пароль. </param>
        /// <returns> Подтверждение. </returns>
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string newPassword, string currentPasword)
        {
            var result = await _userManager.ChangePasswordAsync(await _userManager.FindByNameAsync(User.Identity.Name), currentPasword, newPassword);
            if (result.Succeeded) 
            {
                return Json(new { success = true, message = "Пароль успешно изменен" });
            }
            else 
            {
                string messege = string.Empty;
                foreach (var error in result.Errors)
                {
                    messege += error.Description+"\n";
                }
                return Json(new { success = false, message = messege });
            }
        }

    }
}
