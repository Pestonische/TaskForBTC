using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TaskForBTC.Repositories.Items;

namespace TaskForBTC.Services.Models
{
    //Модель события
    public class EventModel
    {
        public int EventId { get; set; }
        [Required(ErrorMessage = "Не может быть пустым")]
        [Display(Name = "Название")]
        public string EventName { get; set; }
        public List<Field> EventFields { get; set; }
        [Display(Name = "Пользователи")]
        public List<string> UserNames { get; set; }

        [Display(Name = "Дата")]
        public DateTime EventDate { get; set; }
        [Display(Name = "Количество пользователей")]
        public int NumberOfUsers { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Не может быть меньше 1")]
        [Display(Name = "Максимальное количество")]
        public int MaxNumberOfUsers { get; set; }
    }
}
