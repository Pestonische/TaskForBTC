using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskForBTC.DAL.Interfaces;
using TaskForBTC.Repositories.Items;

namespace TaskForBTC.DAL.Repositories
{
    //Класс, реализующий интерфейс IUsersRepository.
    public class UsersRepository : IUsersRepository
    {
        private readonly DatabaseContext _context;

        public UsersRepository(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Возвращает пользователя по его id.
        /// </summary>
        /// <param name="userId"> Id пользователя. </param>
        /// <returns> Пользователь. </returns>
        public UserItem GetUserById(string userId)
        {
            UserItem user = _context.UserItems.AsQueryable().Where(u => u.Id == userId).FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Возвращает пользователя по его имени.
        /// </summary>
        /// <param name="userName"> Имя пользователя. </param>
        /// <returns> Пользователь. </returns>
        public UserItem GetUserByUserName(string userName)
        {
            UserItem user = _context.UserItems.AsQueryable().Where(u => u.UserName == userName).FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Изменяет описание пользователя.
        /// </summary>
        /// <param name="user"> Пользователь </param>
        /// <param name="userDescription"> Описание пользователя. </param>
        /// <returns> True, если изменения прошли успешно, и false в осталных случаях. </returns>
        public bool SetDescription(UserItem user, string userDescription)
        {
            user.Description = userDescription;
            _context.Update(user);
            return Save();
        }

        /// <summary>
        /// Изменияет собственное имя пользователя.
        /// </summary>
        /// <param name="user"> Пользователь. </param>
        /// <param name="personalName"> Собственное имя пользователя. </param>
        /// <returns> True, если изменения прошли успешно, и false в осталных случаях. </returns>
        public bool SetPersonalName(UserItem user, string personalName)
        {
            user.PersonalName = personalName;
            _context.Update(user);
            return Save();
        }

        /// <summary>
        /// Сохранение изменений в бд.
        /// </summary>
        /// <returns>Возвращает true, при успешном сохранении, и false в остальных случиях.</returns>
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }


    }
}
