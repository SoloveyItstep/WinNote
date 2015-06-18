using IInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WinNote.Model.Entity.DataBase;

namespace WinNote.Model.Entity
{
    /// <summary>
    /// Класс для десериализации событий календаря
    /// </summary>
    class CalendarDeserialisation
    {
        /// <summary>
        /// Обьект этого класса
        /// </summary>
        public static CalendarDeserialisation calendarDeserialization { get; set; }

        /// <summary>
        /// Обьект класса Entity Framework
        /// </summary>
        WinEntities EntityDataBase;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="EntityDataBase">Обьект класса Entity Framework</param>
        public CalendarDeserialisation(WinEntities EntityDataBase)
        {
            this.EntityDataBase = EntityDataBase;
            calendarDeserialization = this;
        }

        /// <summary>
        /// Получение событий из базы данных (все события)
        /// </summary>
        /// <returns>"Dictionary" с набором данных (IEventInfo, IEventMainInfo): название события, время, напоминание, информация и т.д.</returns>
        public Dictionary<IEventMainInfo, IEventInfo> GetClendarEvents()
        {
            Dictionary<IEventMainInfo, IEventInfo> dictInfo = new Dictionary<IEventMainInfo, IEventInfo>();
            List<WinNote.Model.Entity.DataBase.Calendar> infoList = (from data in EntityDataBase.Calendars where data.UserId == Variables.Id select data).ToList();

            foreach (WinNote.Model.Entity.DataBase.Calendar info in infoList)
            {
                IEventMainInfo inf = GetEventMainInfo(info);
                IEventInfo mainInf = DeserializeFromXml(info.Data);
                dictInfo.Add(inf, mainInf);
            }

            return dictInfo;
        }

        /// <summary>
        /// Получение событий из базы данных (По текущей дате)
        /// </summary>
        /// <param name="date">"DateTime" - Дата для сравнения с DateFrom</param>
        /// <returns>"Dictionary" с набором данных (IEventInfo, IEventMainInfo): название события, время, напоминание, информация и т.д.</returns>
        public Dictionary<IEventMainInfo, IEventInfo> GetCalendarEvents_inCurrentDate(DateTime date)
        {
            Dictionary<IEventMainInfo, IEventInfo> dictInfo = new Dictionary<IEventMainInfo, IEventInfo>();
            List<WinNote.Model.Entity.DataBase.Calendar> infoList = (from data in EntityDataBase.Calendars where data.UserId == Variables.Id select data).ToList();

            foreach (WinNote.Model.Entity.DataBase.Calendar info in infoList)
            {
                IEventMainInfo inf = GetEventMainInfo(info);
                IEventInfo mainInf = DeserializeFromXml(info.Data);
                if (inf.dateFrom.Year == date.Year && inf.dateFrom.Month == date.Month && inf.dateFrom.Day == date.Day)
                    dictInfo.Add(inf, mainInf);
            }
            return dictInfo;
        }

        /// <summary>
        /// Получение событий из базы данных (По текущей неделе)
        /// </summary>
        /// <param name="dateFrom">"DateTime" - Дата для сравнения с DateFrom: dateFrom.Day - дата понедельника</param>
        /// <returns>"Dictionary" с набором данных (IEventInfo, IEventMainInfo): название события, время, напоминание, информация и т.д.</returns>
        public Dictionary<IEventMainInfo, IEventInfo> GetCalendarEvents_inCurrentWeek(DateTime dateFrom)
        {
            Dictionary<IEventMainInfo, IEventInfo> dictInfo = new Dictionary<IEventMainInfo, IEventInfo>();
            List<WinNote.Model.Entity.DataBase.Calendar> infoList = (from data in EntityDataBase.Calendars where data.UserId == Variables.Id select data).ToList();

            foreach (WinNote.Model.Entity.DataBase.Calendar info in infoList)
            {
                IEventMainInfo inf = GetEventMainInfo(info);
                IEventInfo mainInf = DeserializeFromXml(info.Data);

                DateTime tmp = dateFrom.AddDays(6);
                DateTime DateTo = new DateTime(tmp.Year, tmp.Month, tmp.Day, 23, 59, 59);
                DateTime dateFr = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);

                if (inf.dateFrom >= dateFr && inf.dateTo <= DateTo)
                    dictInfo.Add(inf, mainInf);

            }
            return dictInfo;
        }

        /// <summary>
        /// Получение событий из базы данных (По текущему месяцу)
        /// </summary>
        /// <param name="date">"DateTime" - Дата для сравнения с DateFrom</param>
        /// <returns>"Dictionary" с набором данных (IEventInfo, IEventMainInfo): название события, время, напоминание, информация и т.д.</returns>
        public Dictionary<IEventMainInfo, IEventInfo> GetCalendarEvents_inCurrentMonth(DateTime date)
        {
            Dictionary<IEventMainInfo, IEventInfo> dictInfo = new Dictionary<IEventMainInfo, IEventInfo>();
            List<WinNote.Model.Entity.DataBase.Calendar> infoList = (from data in EntityDataBase.Calendars where data.UserId == Variables.Id select data).ToList();

            foreach (WinNote.Model.Entity.DataBase.Calendar info in infoList)
            {
                IEventMainInfo inf = GetEventMainInfo(info);
                IEventInfo mainInf = DeserializeFromXml(info.Data);
                if (inf.dateFrom.Year == date.Year && inf.dateFrom.Month == date.Month)
                    dictInfo.Add(inf, mainInf);
            }
            return dictInfo;
        }

        /// <summary>
        /// Получение событий из базы данных (По текущему году)
        /// </summary>
        /// <param name="date">"DateTime" - Дата для сравнения с DateFrom</param>
        /// <returns>"Dictionary" с набором данных (IEventInfo, IEventMainInfo): название события, время, напоминание, информация и т.д.</returns>
        public Dictionary<IEventMainInfo, IEventInfo> GetCalendarEvents_inCurrentYear(DateTime date)
        {
            Dictionary<IEventMainInfo, IEventInfo> dictInfo = new Dictionary<IEventMainInfo, IEventInfo>();
            List<WinNote.Model.Entity.DataBase.Calendar> infoList = (from data in EntityDataBase.Calendars where data.UserId == Variables.Id select data).ToList();

            foreach (WinNote.Model.Entity.DataBase.Calendar info in infoList)
            {
                IEventMainInfo inf = GetEventMainInfo(info);
                IEventInfo mainInf = DeserializeFromXml(info.Data);
                if (inf.dateFrom.Year == date.Year)
                    dictInfo.Add(inf, mainInf);
            }
            return dictInfo;
        }

        /// <summary>
        /// Получение класса типа IEventMainInfo
        /// </summary>
        /// <param name="info">Класс базы данных Entity Framework</param>
        /// <returns></returns>
        IEventMainInfo GetEventMainInfo(WinNote.Model.Entity.DataBase.Calendar info)
        {
            IEventMainInfo inf = new CalendarEvent();
            inf.dateFrom = DateTime.Parse(info.DateFrom);
            inf.dateTo = DateTime.Parse(info.DateTo);
            inf.EventName = info.EventName;
            inf.ServerId = info.ServerId;
            inf.ShareKey = info.ShareKey;
            inf.created = DateTime.Parse(info.Created);
            return inf;
        }

        /// <summary>
        /// Получение класса данных с информацией события
        /// </summary>
        /// <param name="info">info - строка содержащая сериализованный класс</param>
        /// <returns>возвращает десериализованный класс IEventInfo</returns>
        public IEventInfo DeserializeFromXml(String info)
        {
            IEventInfo eventInfo = new EventInfo();
            StringReader stringReader = new StringReader(info);
            XmlSerializer serializer = new XmlSerializer(eventInfo.GetType());
            eventInfo = serializer.Deserialize(stringReader) as IEventInfo;
            return eventInfo;
        }

    }
}
