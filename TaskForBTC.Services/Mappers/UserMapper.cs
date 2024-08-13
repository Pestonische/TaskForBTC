using System;
using System.Collections.Generic;
using System.Text;
using TaskForBTC.Repositories.Items;
using TaskForBTC.Services.Models;

namespace TaskForBTC.Services.Mappers
{
    //Класс, реализующий преобразование пользователей из модели в объект и наоборот.
    public class UserMapper
    {
        /// <summary>
        /// Из модели в объект.
        /// </summary>
        /// <param name="userModel"> Модель пользователя. </param>
        /// <returns> Объект пользователя. </returns>
        public static UserItem Map(UserModel userModel)
        {

            return userModel == null ? null : new UserItem()
            {
                Id = userModel.UserId,
                PersonalName = userModel.PersonalName,
                UserName = userModel.UserName,
                Email = userModel.UserEmail,
                Description = userModel.UserDescription
            };
        }

        /// <summary>
        /// Из объекта в модель.
        /// </summary>
        /// <param name="userItem"> Объект пользователя. </param>
        /// <returns> Модель пользователя. </returns>
        public static UserModel Map(UserItem userItem)
        {
            return userItem == null ? null : new UserModel()
            {
                UserId = userItem.Id,
                PersonalName = userItem.PersonalName,
                UserName = userItem.UserName,
                UserEmail = userItem.Email,
                UserDescription = userItem.Description
            };
        }
    }
}
