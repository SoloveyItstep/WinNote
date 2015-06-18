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

namespace WinNote.Model.Entity
{
    class CheckEventsClass
    {
        EntityData entityData;
        BalloonInfo_Event balloonInfo;
        DispatcherTimer timer;
        List<Calendar> calendar;
        List<Calendar> ToRemind;
        
        public CheckEventsClass()
        {
            balloonInfo = new BalloonInfo_Event();
            calendar = new List<Calendar>();
            ToRemind = new List<Calendar>();
        }
        public void SetTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 1, 0);
            timer.Tick += timer_Tick;
            timer.Start();
            timer_Tick(null, null);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Task task = new Task(new Action(() =>
            {
                calendar = (from data in EntityData.EntityDataBase.Calendars where data.UserId == Variables.Id select data).ToList();
                DateTime tmp = new DateTime(1, 1, 1, 1, 1, 1);
                
                foreach (Calendar calend in calendar)
                {
                    IEventInfo info = CalendarDeserialisation.calendarDeserialization.DeserializeFromXml(calend.Data);
                    IEventMainInfo mainInfo = new EventMainInfo()
                    {
                        created = DateTime.Parse(calend.Created),
                        Data = calend.Data,
                        dateFrom = DateTime.Parse(calend.DateFrom),
                        dateTo = DateTime.Parse(calend.DateTo),
                        EventName = calend.EventName,
                        ServerId = calend.ServerId,
                        ShareKey = calend.ShareKey
                    };
                    if(info.Remind == 1 && info.Reminded == 1)
                    {
                        DateTime date = DateTime.Parse(calend.DateFrom);
                        if(date <= DateTime.Now)
                        {
                            ToRemind.Add(calend);
                            //String text = String.Format("с {0}\nдо {1}\nРасположение {2}\nОписание {3}",
                            //            calend.DateFrom,calend.DateTo,info.location,info.description);

                            balloonInfo.SendEvent(mainInfo, info);
                        }
                        else
                        {
                            DateTime now = DateTime.Now;
                            DateTime ev = DateTime.Now;
                            Boolean key = false;
                            if(info.reminderDate == Calendarenums.ReminderDate.fiveMinutes)
                                ev = date.AddMinutes(-5);
                            else if (info.reminderDate == Calendarenums.ReminderDate.Hour)
                                ev = date.AddHours(-1);
                            else if (info.reminderDate == Calendarenums.ReminderDate.Day)
                                ev = date.AddDays(-1);
                            
                            if (ev.Year == now.Year && ev.Month == now.Month && ev.Minute == now.Minute)
                            {
                                ToRemind.Add(calend);
                                key = true;
                            }
                            if (key)
                            {
                                //String text = String.Format("с {0}\nдо {1}\nРасположение {2}\nОписание {3}",
                                //            calend.DateFrom, calend.DateTo, info.location, info.description);
                                balloonInfo.SendEvent(mainInfo, info);
                                key = false;
                            }
                        }
                    }
                    
                }
                
            }));
            task.Start();
        }

    }
}
