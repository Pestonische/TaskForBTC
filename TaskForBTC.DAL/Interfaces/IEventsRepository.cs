using System;
using System.Collections.Generic;
using System.Text;
using TaskForBTC.Repositories.Items;

namespace TaskForBTC.DAL.Interfaces
{
    //Интерфейс, описывающий взаимодействие с датасетом событий.
    public interface IEventsRepository
    {
        ICollection<EventItem> GetEvents();
        EventItem GetEventsById(int? eventId);
        EventItem GetEventsByName(string eventName);
        bool EditEvent(int eventId, string eventName, List<Field> eventFields, List<string> eventUsers, 
                        DateTime eventDate, int numberOfUsers, int maxNumberOfUsers);
        bool CreatEvent(string eventName, List<Field> eventFields, List<string> eventUsers,
                        DateTime eventDate, int maxNumberOfUsers);
        bool DeleteEvent(int eventId);

        bool Save();
    }
}
