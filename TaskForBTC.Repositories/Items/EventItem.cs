using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace TaskForBTC.Repositories.Items
{
    //Класс для хранения событий.
    [Table("Events")]
    public class EventItem
    {        
        [Key]
        public int Id { get; set; }
        //Название события.
        [Required]
        [ScaffoldColumn(false)]
        [StringLength(255)]
        public string Name { get; set; }
        //Дополнительные поля.
        [ScaffoldColumn(false)]
        public string Fields { get; set; }
        //Пользователи участвующие.
        [ScaffoldColumn(false)]
        public string Users { get; set; }
        //Дата события.
        [Required]
        [ScaffoldColumn(false)]
        public DateTime EventDate { get; set; }
        //Количество пользователей.
        [Required]
        [ScaffoldColumn(false)]
        public int NumberOfUsers { get; set; }
        //Максимальное количество пользователей.
        [Required]
        [ScaffoldColumn(false)]
        public int MaxNumberOfUsers { get; set; }


    }
}
