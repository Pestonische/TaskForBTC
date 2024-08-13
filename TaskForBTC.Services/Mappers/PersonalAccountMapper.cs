using System;
using System.Collections.Generic;
using System.Text;
using TaskForBTC.Repositories.Items;
using TaskForBTC.Services.Models;

namespace TaskForBTC.Services.Mappers
{
    //Класс, реализующий преобразование пользователей из модели в объект и наоборот.
    public class PersonalAccountMapper
    {
        /// <summary>
        /// Из модели в объект.
        /// </summary>
        /// <param name="personalAccountModel"> Модель личного кабинета. </param>
        /// <returns> Объект пользователь. </returns>
        public static UserItem Map(PersonalAccountModel personalAccountModel)
        {

            return personalAccountModel == null ? null : new UserItem()
            {                
                PersonalName = personalAccountModel.PersonalName,
                Email = personalAccountModel.Email,
                Description = personalAccountModel.Description
            };
        }

        /// <summary>
        /// Из объекта в модель.
        /// </summary>
        /// <param name="userItem"> Объект пользователя. </param>
        /// <returns> Модель личного кабинета. </returns>
        public static PersonalAccountModel Map(UserItem userItem)
        {
            return userItem == null ? null : new PersonalAccountModel()
            {
                PersonalName = userItem.PersonalName,
                Email = userItem.Email,
                Description = userItem.Description
            };
        }
    }
}
