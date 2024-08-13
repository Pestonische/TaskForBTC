using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Text;

namespace TaskForBTC.Repositories.Items
{
    //Класс для хранения пользователей, реализующий интерфейс IdentityUser,
    //для реализации авторизации пользователей.
    [Table("Users")]
    public class UserItem : IdentityUser
    {
        //Поле для хранения собственного имени пользователя.
        [ScaffoldColumn(false)]
        [StringLength(255)]
        public string PersonalName { get; set; }
       
        //Поле для описания пользователя, наподобие "О себе".
        [ScaffoldColumn(false)]
        public string Description { get; set; }

        
    }
}
