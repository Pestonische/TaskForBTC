using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskForBTC.Repositories.Items;

namespace TaskForBTC.DAL
{
    //Класс для инициализации базыданных.
    //Добавление ролей и первого администратора.
    public class Seed
    {
        /// <summary>
        /// Функция инициализации.
        /// </summary>
        /// <param name="userManager"> Датасет пользователей. </param>
        /// <param name="roleManager"> Датасет ролей. </param>
        /// <returns></returns>
        public static async Task InitializeAsync(UserManager<UserItem> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "alexeikozunov@gmail.com";
            string adminName = "admin";
            string password = "1234As!";
            string personalName = "admin";
            string description = "admin";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            //Проверка, что админестратор еще не добавлен.
            if (await userManager.FindByNameAsync(adminName) == null)
            {
                UserItem admin = new UserItem { Email = adminEmail, UserName = adminName, PersonalName = personalName, Description = description, EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
