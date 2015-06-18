using CalendarAddMenu_UserControl;
using EventLibrsries;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using WinNote.Control.Entity;
using WinNote.Model.Entity.DataBase;
using WinNote.Model.SQL;

namespace WinNote.Model.Entity.CheckNewNotes
{
    public class CheckNewNotes
    {
        SendEventAboutChanges_Event sendEventAboutChanges;
        DispatcherTimer timer;
        public CheckNewNotes()
        {
            sendEventAboutChanges = new SendEventAboutChanges_Event();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0,10,0);
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            FindCalendarEvents();
        }

        public void FindCalendarEvents()
        {
            timer.Stop();
            List<IEventMainInfo> mainInfoList = new List<IEventMainInfo>();
            if (Variables.Id == 0)
            {
                if (EntityData.entityData != null)
                    EntityData.entityData.GetServerUserId();
                else
                    return;
                if (Variables.Id == 0)
                    return;
            }
                Int32 serverId = (from data in EntityData.EntityDataBase.Users where data.Id == Variables.Id select data).ToList()[0].ServerId;
                String str = "Select * From Calendar where `UserId` = '" + serverId + "'";

                DataSet dataSet = WorkWithSQLData.GetInstance.GetRequest(str);

                if (dataSet == null)
                {
                    timer.Start();
                    return;
                }

                foreach (DataTable table in dataSet.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        IEventMainInfo mainInfo = new EventMainInfo();
                        mainInfo.ServerId = (Int32)row[1];
                        mainInfo.EventName = row[2].ToString();
                        mainInfo.dateFrom = DateTime.Parse(row[3].ToString());
                        mainInfo.dateTo = DateTime.Parse(row[4].ToString());
                        mainInfo.Data = row[5].ToString();
                        mainInfo.ShareKey = row[6].ToString();
                        mainInfo.created = DateTime.Parse(row[7].ToString());

                        mainInfoList.Add(mainInfo);
                    }
                }

                if (mainInfoList.Count > 0)
                    CheckCalendarEvents(mainInfoList);

            
        }

        /// <summary>
        /// проверка на разницу событий в календаре: если событие отсутствует и небыло удалено из-за отсутствия соединения
        /// с сервером, тогда событие добавляется в локальную базу данных
        /// </summary>
        /// <param name="list">Лист с событиями</param>
        void CheckCalendarEvents(List<IEventMainInfo> list)
        {
            Boolean key = false;
            List<Calendar> calendarList = (from data in EntityData.EntityDataBase.Calendars 
                                           where 
                                                data.UserId == Variables.Id 
                                           select data).ToList();
            List<CalendarEventsToLoadOnServer> deletedEvents = (from data in EntityData.EntityDataBase.CalendarEventsToLoadOnServers
                                                                where
                                                                    data.UserId == Variables.Id && data.Value == "Delete"
                                                                select data).ToList();

            Int32 addEventCount = 0;


            if(calendarList != null)
            {
                foreach(IEventMainInfo mainInfo in list)
                {
                    foreach(Calendar calend in calendarList)
                    {
                        if(DateTime.Parse(calend.Created) == mainInfo.created)
                        {
                            key = true;
                            if (deletedEvents != null && deletedEvents.Count > 0)
                            {
                                foreach (CalendarEventsToLoadOnServer ev in deletedEvents)
                                {
                                    if (DateTime.Parse(ev.Created) == mainInfo.created)
                                        key = false;
                                }
                            }
                            break;
                        }
                    }
                    if(!key)
                    {
                        EntityData.EntityDataBase.Calendars.Add(new Calendar() 
                        {
                            Created = String.Format("{0}.{1}.{2} {3}:{4}:{5}", mainInfo.created.Year, mainInfo.created.Month,
                                                    mainInfo.created.Day, mainInfo.created.Hour, mainInfo.created.Minute, mainInfo.created.Second),
                            Data = mainInfo.Data,
                            DateFrom = String.Format("{0}.{1}.{2} {3}:{4}:{5}", mainInfo.dateFrom.Year, mainInfo.dateFrom.Month,
                                                    mainInfo.dateFrom.Day, mainInfo.dateFrom.Hour, mainInfo.dateFrom.Minute, mainInfo.dateFrom.Second),
                            DateTo = String.Format("{0}.{1}.{2} {3}:{4}:{5}", mainInfo.dateTo.Year, mainInfo.dateTo.Month,
                                                    mainInfo.dateTo.Day, mainInfo.dateTo.Hour, mainInfo.dateTo.Minute, mainInfo.dateTo.Second),
                            EventName = mainInfo.EventName,
                            ServerId = mainInfo.ServerId,
                            ShareKey = mainInfo.ShareKey,
                            UserId = Variables.Id
                        });
                        addEventCount++;
                        EntityData.EntityDataBase.SaveChanges();
                    }
                    key = false;
                }
            }
            else
            {
                foreach (IEventMainInfo mainInfo in list)
                {
                    EntityData.EntityDataBase.Calendars.Add(new Calendar()
                    {
                        Created = String.Format("{0}.{1}.{2} {3}:{4}:{5}", mainInfo.created.Year, mainInfo.created.Month,
                                                mainInfo.created.Day, mainInfo.created.Hour, mainInfo.created.Minute, mainInfo.created.Second),
                        Data = mainInfo.Data,
                        DateFrom = String.Format("{0}.{1}.{2} {3}:{4}:{5}", mainInfo.dateFrom.Year, mainInfo.dateFrom.Month,
                                                mainInfo.dateFrom.Day, mainInfo.dateFrom.Hour, mainInfo.dateFrom.Minute, mainInfo.dateFrom.Second),
                        DateTo = String.Format("{0}.{1}.{2} {3}:{4}:{5}", mainInfo.dateTo.Year, mainInfo.dateTo.Month,
                                                mainInfo.dateTo.Day, mainInfo.dateTo.Hour, mainInfo.dateTo.Minute, mainInfo.dateTo.Second),
                        EventName = mainInfo.EventName,
                        ServerId = mainInfo.ServerId,
                        ShareKey = mainInfo.ShareKey,
                        UserId = Variables.Id
                    });
                    addEventCount++;
                    EntityData.EntityDataBase.SaveChanges();
                }
            }

            if (addEventCount > 0)
                sendEventAboutChanges.SendEvent(true);
        }

    }
}
