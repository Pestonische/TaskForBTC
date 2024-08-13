using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TaskForBTC.Repositories.Items;
using TaskForBTC.Services.Models;

namespace TaskForBTC.Services.Mappers
{
    //Класс, реализующий преобразование событий из модели в объект и наоборот.
    public class EventMapper
    {
        /// <summary>
        /// Из модели в объект.
        /// </summary>
        /// <param name="eventModel"> Модель события. </param>
        /// <param name="userItems"> Id пользователей. </param>
        /// <returns> Объект события. </returns>
        public static EventItem Map(EventModel eventModel, List<int> userItems)
        {

            return eventModel == null || userItems.Count == 0 ? null : new EventItem()
            {
                Id = eventModel.EventId,
                Fields = JsonConvert.SerializeObject(eventModel.EventFields),
                Name = eventModel.EventName,
                EventDate = eventModel.EventDate,
                MaxNumberOfUsers = eventModel.MaxNumberOfUsers,
                NumberOfUsers = eventModel.NumberOfUsers,
                Users = JsonConvert.SerializeObject(userItems)
            };
        }

        /// <summary>
        /// Из объекта в модель.
        /// </summary>
        /// <param name="eventItem"> Объект события. </param>
        /// <param name="userNames"> Имена пользователей. </param>
        /// <returns> Модель события. </returns>
        public static EventModel Map(EventItem eventItem, List<string> userNames)
        {
            return eventItem == null || userNames == null ? null : new EventModel()
            {
                EventId = eventItem.Id,
                EventName = eventItem.Name,                
                EventFields = JsonConvert.DeserializeObject<List<Field>>(eventItem.Fields),
                EventDate = eventItem.EventDate,
                MaxNumberOfUsers = eventItem.MaxNumberOfUsers,
                NumberOfUsers = eventItem.NumberOfUsers,
                UserNames = userNames
            };
        }
    }
}
