using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskForBTC.DAL.Interfaces;
using TaskForBTC.Repositories.Items;

namespace TaskForBTC.DAL.Repositories
{
    //Класс, реализующий интерфейс IEventsRepository
    public class EventsRepository : IEventsRepository
    {
        private readonly DatabaseContext _context;
        public EventsRepository(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Создает и добавляет новое событие в бд.
        /// </summary>
        /// <param name="eventName"> Название события. </param>
        /// <param name="eventFields"> Дополнительные поля события. </param>
        /// <param name="eventUsers"> Пользователи участвующие в событии. </param>
        /// <param name="eventDate"> Дата проведения события. </param>
        /// <param name="maxNumberOfUsers"> Максимальное количество пользователей. </param>
        /// <returns> True, если добавление прошло успешно, и false в осталных случаях. </returns>
        public bool CreatEvent(string eventName, List<Field> eventFields, List<string> eventUsers, 
                    DateTime eventDate, int maxNumberOfUsers)
        {            
            EventItem eventItem = new EventItem 
            {
                Name = eventName,
                Fields = JsonConvert.SerializeObject(eventFields),
                Users = JsonConvert.SerializeObject(eventUsers),
                EventDate = eventDate,
                MaxNumberOfUsers = maxNumberOfUsers
            };
            _context.Add(eventItem);
            return Save();
        }

        /// <summary>
        /// Удаления события по его id.
        /// </summary>
        /// <param name="eventId"> Id события. </param>
        /// <returns> True, если удаление прошло успешно, и false в осталных случаях. </returns>
        public bool DeleteEvent(int eventId)
        {
            EventItem eventItem = _context.EventItems.AsQueryable().Where(e => e.Id == eventId).FirstOrDefault();
            _context.Remove(eventItem);
            return Save();
        }

        /// <summary>
        /// Изменяет событие по параметрам.
        /// </summary>
        /// <param name="eventId"> Id события. </param>
        /// <param name="eventName"> Название события. </param>
        /// <param name="eventFields"> Дополнительные поля события. </param>
        /// <param name="eventUsers"> Пользователи участвующие в событии. </param>
        /// <param name="eventDate"> Дата проведения события. </param>
        /// <param name="numberOfUsers"> Количество пользователей. </param>
        /// <param name="maxNumberOfUsers"> Максимальное количество пользователей. </param>
        /// <returns> True, если изменения прошли успешно, и false в осталных случаях. </returns>
        public bool EditEvent(int eventId, string eventName, List<Field> eventFields, 
                    List<string> eventUsers, DateTime eventDate, int numberOfUsers, int maxNumberOfUsers)
        {
            EventItem eventItem = _context.EventItems.AsQueryable().Where(e => e.Id == eventId).FirstOrDefault();
            eventItem.Name = eventName;
            eventItem.Fields = JsonConvert.SerializeObject(eventFields);
            eventItem.Users = JsonConvert.SerializeObject(eventUsers);
            eventItem.EventDate = eventDate;
            eventItem.NumberOfUsers = numberOfUsers;
            eventItem.MaxNumberOfUsers = maxNumberOfUsers;
            _context.Update(eventItem);
            return Save();
        }

        /// <summary>
        /// Возвращает все события отсортированные по id.
        /// </summary>
        /// <returns> События. </returns>
        public ICollection<EventItem> GetEvents()
        {
            ICollection<EventItem> eventItems = _context.EventItems.AsQueryable().OrderBy(e => e.Id).ToList();
            return eventItems;
        }

        /// <summary>
        /// Возвращает событие по id.
        /// </summary>
        /// <param name="eventId"> Id события. </param>
        /// <returns> Событие. </returns>
        public EventItem GetEventsById(int? eventId)
        {
            EventItem eventItem = _context.EventItems.AsQueryable().Where(e => e.Id == eventId).FirstOrDefault();
            return eventItem;
        }

        /// <summary>
        /// Возвращает событие по названию.
        /// </summary>
        /// <param name="eventName"> Название события. </param>
        /// <returns> Событие. </returns>
        public EventItem GetEventsByName(string eventName)
        {
            EventItem eventItem = _context.EventItems.AsQueryable().Where(e => e.Name == eventName).FirstOrDefault();
            return eventItem;
        }

        /// <summary>
        /// Сохранение изменений в бд.
        /// </summary>
        /// <returns>Возвращает true, при успешном сохранении, и false в остальных случиях.</returns>
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
