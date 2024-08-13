using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TaskForBTC.Repositories.Items;

namespace TaskForBTC.Services.Models
{
    //Короткая запись события для возврата после редактирования.
    public class ShortEventModel
    {
        public int EventId { get; set; }
        [Display(Name = "Название")]
        public string EventName { get; set; }
        
        [Display(Name = "Дата")]
        public DateTime EventDate { get; set; }
        
        [Display(Name = "Максимальное количество")]
        public int MaxNumberOfUsers { get; set; }
    }
}
