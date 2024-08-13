using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using TaskForBTC.Repositories.Items;
using TaskForBTC.Services.IServices;
using TaskForBTC.Services.Models;
using TaskForBTC.Services.Services;

namespace TaskForBTC.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly IEventService _eventService;
        private static List<Field> _fields { get; set; }

        public AdministrationController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Administration()
        {
            return View();
        }

        /// <summary>
        /// Удаляет событие по Id.
        /// </summary>
        /// <param name="eventId"> Id события. </param>
        /// <returns> Подтверждение. </returns>
        [HttpPost]
        public IActionResult DeleteEvent(int eventId)
        {
            try
            {
                _eventService.DeleteEvent(eventId);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
            return Json(new { success = true, message = "" });
        }

        #region--Заполнение таблиц--
        /// <summary>
        /// Заполняет таблицу событий.
        /// </summary>
        /// <returns> События. </returns>
        [HttpGet]
        public JsonResult GetEvents()
        {
            return Json(new { data = _eventService.GetEvents() });
        }

        /// <summary>
        /// Заполняет таблицу дополнительных палей у события.
        /// </summary>
        /// <param name="id"> Id события. </param>
        /// <returns> Дополнительные поля события. </returns>
        [HttpGet]
        public JsonResult GetFields(int id)
        {
            return Json(new { data = _eventService.GetEventsById(id).EventFields });
        }

        /// <summary>
        /// Заполняет таблицу дополнительных палей у события.
        /// </summary>
        /// <returns>Дополнительные поля события.</returns>
        [HttpGet]
        public JsonResult GetFieldCreate()
        {
            return Json(new { data = _fields });
        }

        #endregion

        #region--Изменения--
        /// <summary>
        /// Запуск изменения события, с передачай его на форму.
        /// </summary>
        /// <param name="id"> Id события. </param>
        /// <returns> Открытие формы. </returns>
        [HttpGet]
        public IActionResult EditEvent(int id)
        {
            return View(_eventService.GetEventsById(id));
        }

        /// <summary>
        /// Получение результатов редактирования и их фиксации.
        /// </summary>
        /// <param name="eventModel"> Модель события. </param>
        /// <returns> Подтверждение. </returns>
        [HttpPost]
        public IActionResult EditEvent(ShortEventModel eventModel)
        {
            EventModel model = _eventService.GetEventsById(eventModel.EventId);
            model.EventName = eventModel.EventName;
            model.EventDate = eventModel.EventDate;
            model.MaxNumberOfUsers = eventModel.MaxNumberOfUsers;
            var result = _eventService.EditEvent(model);
            if (result)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Изменение не удалось" });
            
        }
        /// <summary>
        /// Добавление новых дополнительных палей.
        /// </summary>
        /// <param name="fieldLable"> Название поля. </param>
        /// <param name="fieldName"> Описание поля. </param>
        /// <param name="eventId"> Id события. </param>
        /// <returns> Подтверждение. </returns>
        [HttpPost]
        public IActionResult AddField(string fieldLable, string fieldName, int eventId)
        {
            try
            {
                EventModel eventModel = _eventService.GetEventsById(eventId);
                Field field = new Field()
                {
                    Lable = fieldLable,
                    Name = fieldName
                };
                eventModel.EventFields.Add(field);
                _eventService.EditEvent(eventModel);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
            return Json(new { success = true, message = "" });
        }

        /// <summary>
        /// Удаление палей.
        /// </summary>
        /// <param name="field"> Id поля. </param>
        /// <param name="eventId"> Id события. </param>
        /// <returns> Подтверждение </returns>
        [HttpPost]
        public IActionResult DeleteField(int field, int eventId)
        {
            try
            {
                EventModel eventModel = _eventService.GetEventsById(eventId);
                eventModel.EventFields.RemoveAt(field);
                _eventService.EditEvent(eventModel);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
            return Json(new { success = true, message = "" });
        }
        #endregion

        #region--Добавление--
        /// <summary>
        /// Запуск добавления события, с передачай его на форму.
        /// </summary>
        /// <returns> Открытие формы. </returns>
        [HttpGet]
        public IActionResult CreateEvent()
        {
            _fields = new List<Field>();
            EventModel eventModel = new EventModel();
            eventModel.EventDate = DateTime.Now;
            return View(eventModel);
        }

        /// <summary>
        /// Получение результатов создания и их фиксации.
        /// </summary>
        /// <param name="eventModel"> Модель события. </param>
        /// <returns> Подтверждение. </returns>
        [HttpPost]
        public IActionResult CreateEvent(EventModel eventModel)
        {
            eventModel.EventFields = _fields;
            eventModel.NumberOfUsers = 0;
            eventModel.UserNames = new List<string>();
            var result = _eventService.CreatEvent(eventModel);
            if (result)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Изменение не удалось" });
        }

        /// <summary>
        /// Удаление палей.
        /// </summary>
        /// <param name="field"> Id поля. </param>
        /// <returns> Подтверждение </returns>
        [HttpPost]
        public IActionResult DeleteFieldCreate(int field)
        {
            _fields.RemoveAt(field);
            return Json(new { success = true, message = "" });
        }

        

        /// <summary>
        /// Добавление новых дополнительных палей.
        /// </summary>
        /// <param name="fieldLable"> Название поля. </param>
        /// <param name="fieldName"> Описание поля. </param>
        /// <returns> Подтверждение. </returns>
        [HttpPost]
        public IActionResult AddFieldCreate(string fieldLable, string fieldName)
        {
            _fields.Add(new Field()
            {
                Lable = fieldLable,
                Name = fieldName
            });
            return Json(new { success = true, message = "" });
        }        
        #endregion
    }
}
