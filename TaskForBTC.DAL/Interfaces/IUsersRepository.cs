using System;
using System.Collections.Generic;
using TaskForBTC.Repositories.Items;
using System.Text;
using System.ComponentModel;

namespace TaskForBTC.DAL.Interfaces
{
    //Интерфейс, описывающий взаимодействие с датасетом пользователей.
    public interface IUsersRepository 
    {
        UserItem GetUserById(string userId);
        UserItem GetUserByUserName(string personalName);
        bool SetDescription(UserItem user, string userDescription);
        bool SetPersonalName(UserItem user, string personalName);
        bool Save();
    }
}
