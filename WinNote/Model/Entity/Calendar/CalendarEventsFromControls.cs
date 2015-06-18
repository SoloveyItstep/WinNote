using EventLibrsries;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinNote.Model.Entity
{
    public class CalendarEventsFromControls
    {
        CalendarDeserialisation calendarDeserialization;
        LoadEventInfo_Event loadEventInfo;
        public CalendarEventsFromControls()
        {
            calendarDeserialization = CalendarDeserialisation.calendarDeserialization;
        }

        public void Events()
        {
            loadEventInfo = new LoadEventInfo_Event();
            SendEventToModel_Event.GetEvent += SendEventToModel_Event_GetEvent;
        }

//TODO: Event - Sending to panels IEventInfo...
        /// <summary>
        /// Событие из панели с датой и именем для сбора событий и отпраски обратно в пенель (день, неделя, месяц и т.д.)
        /// </summary>
        /// <param name="e">Класс с перечислением имени панели и датой</param>
        void SendEventToModel_Event_GetEvent(object sender, SendEventToModel e)
        {
            Dictionary<IEventMainInfo, IEventInfo> dict = new Dictionary<IEventMainInfo,IEventInfo>();
            LoadEventInfo_Event.CalendarEnum en = LoadEventInfo_Event.CalendarEnum.Day;

            if(e.controlName == SendEventToModel_Event.ControlNameEnum.Day)
            {
                dict = calendarDeserialization.GetCalendarEvents_inCurrentDate(e.date);
                en = LoadEventInfo_Event.CalendarEnum.Day;
            }
            else if(e.controlName == SendEventToModel_Event.ControlNameEnum.Week)
            {
                dict = calendarDeserialization.GetCalendarEvents_inCurrentWeek(e.date);
                en = LoadEventInfo_Event.CalendarEnum.Week;
            }
            else if (e.controlName == SendEventToModel_Event.ControlNameEnum.Month)
            {
                dict = calendarDeserialization.GetCalendarEvents_inCurrentMonth(e.date);
                en = LoadEventInfo_Event.CalendarEnum.Month;
            }
            else if (e.controlName == SendEventToModel_Event.ControlNameEnum.Timetable)
            {
                dict = calendarDeserialization.GetCalendarEvents_inCurrentMonth(e.date);
                en = LoadEventInfo_Event.CalendarEnum.Timetable;
            }
            loadEventInfo.SetEvent(dict, en);
        }
    }
}
