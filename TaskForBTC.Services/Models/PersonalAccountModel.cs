using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TaskForBTC.Services.Models
{
    //Модель личного кабинета.
    public class PersonalAccountModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string PersonalName { get; set; }
        [Required]
        [Display(Name = "Почта")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Информация")]
        public string Description { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Старый пароль")]
        public string CurrentPassword { get; set; }
    }
}
