using CalendarAddMenu_UserControl;
using EventLibrsries;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using WinNote.Control.Entity;
using WinNote.Model.Entity.DataBase;
using WinNote.Model.SQL;

namespace WinNote.Model.Entity
{
    /// <summary>
    /// Класс для сохранения данных в таблицу
    /// </summary>
    public class SaveCalendarEventToAction_Delete_Add_Change
    {
        private DispatcherTimer timer;
        WorkWithSQLData SQLData;
        EventToDoActions_Event sendDoAction;
        
        private static SaveCalendarEventToAction_Delete_Add_Change ActionEvent { get; set; }
        private SaveCalendarEventToAction_Delete_Add_Change() 
        {
            sendDoAction = new EventToDoActions_Event();
            SQLData = WorkWithSQLData.GetInstance;
            SQLData.SelectEvents();
            SetTimer();
            SendBackEventAfterDoAction_Event.GetEvent += SendBackEventAfterDoAction_Event_GetEvent;
            CheckActions();
        }

        void CheckActions()
        {
		        if (Variables.Id == 0)
                {
                    var logUser = (from data in EntityData.EntityDataBase.LogedInUsers select data).ToList();
                    foreach (LogedInUser user in logUser)
                        Variables.Id = user.UserId;
                }
                var v = (from data in EntityData.EntityDataBase.CalendarEventsToLoadOnServers where data.UserId == Variables.Id select data).ToList();
                if (v.Any())
                {
                    Connect();
                   timer.Start();
                }
        }

        void SendBackEventAfterDoAction_Event_GetEvent(object sender, SendBackEventAfterDoAction e)
        {
            String dateCreated = String.Format("{0}.{1}.{2} {3}:{4}:{5}",e.mainInfo.created.Year,e.mainInfo.created.Month,
                        e.mainInfo.created.Day,e.mainInfo.created.Hour,e.mainInfo.created.Minute,e.mainInfo.created.Second);
            List<CalendarEventsToLoadOnServer> lstToDelete =
                (from data in EntityData.EntityDataBase.CalendarEventsToLoadOnServers
                 where data.Created == dateCreated && 
                               data.EventName == e.mainInfo.EventName select data).ToList();

            foreach(var v in lstToDelete)
            {
                EntityData.EntityDataBase.CalendarEventsToLoadOnServers.Remove(v);
            }
            EntityData.EntityDataBase.SaveChanges();
        }
        
        public static SaveCalendarEventToAction_Delete_Add_Change GetInstance
        {
            get
            {
                if (ActionEvent == null)
                    ActionEvent = new SaveCalendarEventToAction_Delete_Add_Change();
                return ActionEvent;
            }
            set { ActionEvent = value; }
        }

        /// <summary>
        /// Таймер для обработки незавершенных на сервере событий (Update, Delete, Insert)
        /// </summary>
        void SetTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0,5,0);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(Connect));
            thread.Start();
        }

        /// <summary>
        /// Если соединение открыто и в наличии есть незавершенные задачи - производится их завершение
        /// </summary>
        void Connect()
        {
            if (SQLData.Connect())
            {
                SQLData.key = true;
                SendDataToSQLServer();
                timer.Stop();
                SQLData.key = false;
            }
            var v = (from data in EntityData.EntityDataBase.CalendarEventsToLoadOnServers where data.UserId == Variables.Id select data).ToList();
             if (v.Count == 0)
                 timer.Stop();
        }

        
        void SendDataToSQLServer()
        {
            List<CalendarEventsToLoadOnServer> ListWithData =
                (from data in EntityData.EntityDataBase.CalendarEventsToLoadOnServers select data).ToList();
            foreach(var v in ListWithData)
            {
                IEventMainInfo mainInfo = new EventMainInfo()
                {
                    created = DateTime.Parse(v.Created),
                    Data = v.Data,
                    dateFrom = DateTime.Parse(v.DateFrom),
                    dateTo = DateTime.Parse(v.DateTo),
                    EventName = v.EventName,
                    ServerId = v.ServerId,
                    ShareKey = v.ShareKey
                };
                IEventInfo info = CalendarDeserialisation.calendarDeserialization.DeserializeFromXml(v.Data);
                sendDoAction.SendEvent(mainInfo, info, v.Value);
            }
        }

        /// <summary>
        /// Занесение в базу данных событий которые не завершины из-за отсутствия соединения с интернетом
        /// Событие предназначено для Delete
        /// </summary>
        public void Save(IEventMainInfo mainInfo, IEventInfo info, String Value)
        {
            String data = EntityData.GetInstance.SerializeToXml(info);
            var ev = new CalendarEventsToLoadOnServer()
            {
                Created = String.Format("{0}.{1}.{2} {3}:{4}:{5}", mainInfo.created.Year, mainInfo.created.Month, mainInfo.created.Day,
                                                                 mainInfo.created.Hour, mainInfo.created.Minute, mainInfo.created.Second),
                DateFrom = String.Format("{0}.{1}.{2} {3}:{4}:{5}", mainInfo.dateFrom.Year, mainInfo.dateFrom.Month, mainInfo.dateFrom.Day,
                                                                 mainInfo.dateFrom.Hour, mainInfo.dateFrom.Minute, mainInfo.dateFrom.Second),
                DateTo = String.Format("{0}.{1}.{2} {3}:{4}:{5}", mainInfo.dateTo.Year, mainInfo.dateTo.Month, mainInfo.dateTo.Day,
                                                                 mainInfo.dateTo.Hour, mainInfo.dateTo.Minute, mainInfo.dateTo.Second),
                EventName = mainInfo.EventName,
                ServerId = mainInfo.ServerId,
                ShareKey = mainInfo.ShareKey,
                Data = data,
                UserId = Variables.Id,
                Value = Value
            };
            EntityData.EntityDataBase.CalendarEventsToLoadOnServers.Add(ev);
            EntityData.EntityDataBase.SaveChanges();
        }

        /// <summary>
        /// Занесение в базу данных событий которые не завершины из-за отсутствия соединения с интернетом
        /// Событие предназначено для Update и Insert
        /// </summary>
        public void Save(IEventMainInfo mainInfo, String val)
        {


            var ev = new CalendarEventsToLoadOnServer()
            {
                Created = String.Format("{0}.{1}.{2} {3}:{4}:{5}", mainInfo.created.Year, mainInfo.created.Month, mainInfo.created.Day,
                                                                 mainInfo.created.Hour, mainInfo.created.Minute, mainInfo.created.Second),
                DateFrom = String.Format("{0}.{1}.{2} {3}:{4}:{5}", mainInfo.dateFrom.Year, mainInfo.dateFrom.Month, mainInfo.dateFrom.Day,
                                                                 mainInfo.dateFrom.Hour, mainInfo.dateFrom.Minute, mainInfo.dateFrom.Second),
                DateTo = String.Format("{0}.{1}.{2} {3}:{4}:{5}", mainInfo.dateTo.Year, mainInfo.dateTo.Month, mainInfo.dateTo.Day,
                                                                 mainInfo.dateTo.Hour, mainInfo.dateTo.Minute, mainInfo.dateTo.Second),
                EventName = mainInfo.EventName,
                ServerId = mainInfo.ServerId,
                ShareKey = mainInfo.ShareKey,
                Data = mainInfo.Data,
                UserId = Variables.Id,
                Value = val
            };
            EntityData.EntityDataBase.CalendarEventsToLoadOnServers.Add(ev);
            EntityData.EntityDataBase.SaveChanges();

        }
    }
}
