using System;
using System.Collections.Generic;
using System.Text;
using TaskForBTC.Repositories.Items;
using TaskForBTC.Services.Models;

namespace TaskForBTC.Services.IServices
{
    //Интерфейс, реализующий взаимодействие модели события с обьектом события.
    public interface IEventService
    {
        ICollection<EventModel> GetEvents();
        EventModel GetEventsById(int? eventId);
        EventModel GetEventsByName(string eventName);
        bool EditEvent(EventModel eventModel);
        bool CreatEvent(EventModel eventModel);
        bool DeleteEvent(int eventId);
    }
}
