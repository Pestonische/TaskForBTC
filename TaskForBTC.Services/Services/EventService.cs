using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TaskForBTC.DAL.Interfaces;
using TaskForBTC.DAL.Repositories;
using TaskForBTC.Repositories.Items;
using TaskForBTC.Services.IServices;
using TaskForBTC.Services.Mappers;
using TaskForBTC.Services.Models;

namespace TaskForBTC.Services.Services
{
    //Класс, реализующий интерфейс IEventService.
    public class EventService : IEventService
    {
        private readonly IEventsRepository _eventsRepository;

        private readonly IUsersRepository _usersRepository;
        private readonly UserManager<UserItem> _userManager;

        public EventService(IEventsRepository eventsRepository, IUsersRepository usersRepository, UserManager<UserItem> userManager) 
        {
            _eventsRepository = eventsRepository; 
            _usersRepository = usersRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Создает и добавляет новое событие в бд.
        /// </summary>
        /// <param name="eventModel"> Модель события. </param>
        /// <returns> True, если добавление прошло успешно, и false в осталных случаях.</returns>
        public bool CreatEvent(EventModel eventModel)
        {
            List<string> users = new List<string>();
            foreach (var user in eventModel.UserNames)
            {
                users.Add(_usersRepository.GetUserById(user).Id);
            }
            return _eventsRepository.CreatEvent(eventModel.EventName, eventModel.EventFields, 
                        users, eventModel.EventDate, eventModel.MaxNumberOfUsers);
        }

        /// <summary>
        /// Удаления события по его id.
        /// </summary>
        /// <param name="eventId"> Id события. </param>
        /// <returns> True, если удаление прошло успешно, и false в осталных случаях. </returns>
        public bool DeleteEvent(int eventId)
        {
            return _eventsRepository.DeleteEvent(eventId);
        }
        /// <summary>
        /// Изменяет событие по параметрам.
        /// </summary>
        /// <param name="eventModel"> Модель события. </param>
        /// <returns>True, если изменения прошли успешно, и false в осталных случаях.</returns>
        public bool EditEvent(EventModel eventModel)
        {
            List<string> users = new List<string>();
            foreach (var user in eventModel.UserNames)
            {
                users.Add(_usersRepository.GetUserByUserName(user).Id);
            }
            return _eventsRepository.EditEvent(eventModel.EventId, eventModel.EventName, eventModel.EventFields,
                        users, eventModel.EventDate, eventModel.NumberOfUsers, eventModel.MaxNumberOfUsers);
        }

        /// <summary>
        /// Возвращает все события отсортированные по id.
        /// </summary>
        /// <returns> События. </returns>
        public ICollection<EventModel> GetEvents()
        {
            List<EventModel> eventModels = new List<EventModel>();
            List<EventItem> eventItems = _eventsRepository.GetEvents() as List<EventItem>;

            foreach (var eventItem in eventItems)
            {
                List<string> users = new List<string>();
                foreach (var user in JsonConvert.DeserializeObject<List<string>>(eventItem.Users))
                {
                    users.Add(_usersRepository.GetUserById(user).UserName);
                }
                
                eventModels.Add(EventMapper.Map(eventItem, users));
            }
            return eventModels;
        }

        /// <summary>
        /// Получение события по id.
        /// </summary>
        /// <param name="eventId"> Id события. </param>
        /// <returns> Модель события. </returns>
        public EventModel GetEventsById(int? eventId)
        {
            EventItem eventItem = _eventsRepository.GetEventsById(eventId);
            List<string> users = new List<string>();
            foreach (var user in JsonConvert.DeserializeObject<List<string>>(eventItem.Users))
            {
                users.Add(_usersRepository.GetUserById(user).UserName);
            }
            return EventMapper.Map(eventItem, users);
        }

        /// <summary>
        /// Получения события по названию.
        /// </summary>
        /// <param name="eventName"> Название события. </param>
        /// <returns> Модкль события. </returns>
        public EventModel GetEventsByName(string eventName)
        {
            EventItem eventItem = _eventsRepository.GetEventsByName(eventName);
            List<string> users = new List<string>();
            foreach (var user in JsonConvert.DeserializeObject<List<string>>(eventItem.Users))
            {
                users.Add(_usersRepository.GetUserById(user).UserName);
            }
            return EventMapper.Map(eventItem, users);
        }
    }
}
