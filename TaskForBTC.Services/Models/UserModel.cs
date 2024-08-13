using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TaskForBTC.Services.Models
{
    //Модель пользователя
    public class UserModel
    {
        public string UserId { get; set; }
        [Display(Name = "Имя")]
        public string PersonalName { get; set; }
        [Display(Name = "Псевдоним")]
        public string UserName { get; set; }
        [Display(Name = "Почта")]
        public string UserEmail { get; set; }
        [Display(Name = "Информация")]
        public string UserDescription { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
