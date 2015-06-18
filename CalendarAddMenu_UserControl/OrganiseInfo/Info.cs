using CalendarAddMenu_UserControl.Model;
using EventLibrsries;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalendarAddMenu_UserControl.OrganiseInfo
{
    public class Info
    {
        CalendarAddMenu_UserControl control;
        IEventInfo eventInfo;
        IEventMainInfo eventMainInfo;
        CalendarEventInfo calendarEventInfo;
        HideCalendarEventAddMenu_Event hideMenuEvent;
        VarEventInfo varEvent;
        SendEventInfoToChange_Event sendChangedEvent;

        public Info(CalendarAddMenu_UserControl control) 
        {
            this.control = control;
            calendarEventInfo = new CalendarEventInfo();
            hideMenuEvent = new HideCalendarEventAddMenu_Event();
            varEvent = VarEventInfo.GetInstance;
            sendChangedEvent = new SendEventInfoToChange_Event();
        }

        /// <summary>
        /// Спрятать контрол
        /// </summary>
        public void HideMenu()
        {
            hideMenuEvent.SetHideMenuEvent(true);
        }

        /// <summary>
        /// Получение данных из контролла и изминение в базе данных
        /// </summary>
        public void GetAndChange()
        {
            eventInfo = new EventInfo();
            eventMainInfo = new EventMainInfo();

            //Event Name
            if (control.TextBox_eventName.Text == "Название события")
                eventMainInfo.EventName = "Без названия";
            else
                eventMainInfo.EventName = control.TextBox_eventName.Text;

            //Date from
            DateTime date = DateTime.Parse(control.datePicker_DateFrom.Text);
            String time = control.TextBox_timeFrom.Text;
            Int32 length = time.Length;
            Int32 hour = 0, min = 0;
            if (Char.IsDigit(time[0]) && Char.IsDigit(time[1]))
            {
                hour = Int32.Parse(time.Substring(0, 2));
                min = Int32.Parse(time.Substring(3));
            }
            else if (Char.IsDigit(time[0]) && !Char.IsDigit(time[1]))
            {
                hour = Int32.Parse(time.Substring(0, 1));
                min = Int32.Parse(time.Substring(2));
            }
            DateTime dateFrom = new DateTime(date.Year, date.Month, date.Day, hour, min, 0);

            eventMainInfo.dateFrom = dateFrom;

            //Date To
            date = DateTime.Parse(control.datePicker_DateTo.Text);

            time = control.TextBox_timeTo.Text;
            length = time.Length;

            Regex regex = new Regex(@"^(\d{2}:(\d{1,2}))");
            if (regex.IsMatch(time))
            {
                hour = Int32.Parse(time.Substring(0, 2));
                min = Int32.Parse(time.Substring(3));
            }
            else if(new Regex(@"^(\d{1}:(\d{1,2}))").IsMatch(time))
            {
                hour = Int32.Parse(time[0].ToString());
                min = Int32.Parse(time.Substring(2));
            }
            DateTime dateTo = new DateTime(date.Year, date.Month, date.Day, hour, min, 0);

            eventMainInfo.dateTo = dateTo;

            //All day
            if (control.CheckBox_AllDay.IsChecked == true)
                eventInfo.AllDay = 1;
            else
                eventInfo.AllDay = 0;

            //Repeat Event

            if (control.CheckBox_RepeatEvent.IsChecked == true)
                eventInfo.Repeat = 1;
            else
                eventInfo.Repeat = 0;

            //Repeat Event Info

            if (eventInfo.Repeat == 1)
                RepeatEventInfo();

            //Repeat Event - weekday
            if (eventInfo.Repeat == 1 && eventInfo.enumRepeat == Calendarenums.EnumRepeat.Week)
                RepeatEventWeekInfo();

            //Remind Event

            if (control.CheckBox_Reminder.IsChecked == true)
            {
                eventInfo.Remind = 1;
                RemindEventInfo();
            }

            //Location
            if (control.TextBox_Locate.Text == "Место проведения")
                eventInfo.location = "Не указано";
            else
                eventInfo.location = control.TextBox_Locate.Text;

            //Description
            if (control.TextBox_Description.Text == "Описание")
                eventInfo.description = "Без описания";
            else
                eventInfo.description = control.TextBox_Description.Text;

            //EventInfo data setting clear string (not null)
            eventMainInfo.Data = "";

            //Entity key
            eventMainInfo.ShareKey = "";

            //Add SharedKey, ServerId & Created date
            eventMainInfo.ServerId = varEvent.eventMainInfo.ServerId;
            eventMainInfo.ShareKey = varEvent.eventMainInfo.ShareKey;
            eventMainInfo.created = varEvent.eventMainInfo.created;
            //Sending Data
            sendChangedEvent.SendEvent(eventMainInfo, eventInfo);

            //Hide menu
            hideMenuEvent.SetHideMenuEvent(true);
        }

        /// <summary>
        /// Creates Info from control and sending to "CalendarEventInfo"
        /// </summary>
        public void GetAndSend()
        {
            eventInfo = new EventInfo();
            eventMainInfo = new EventMainInfo();

    //Event Name
            if (control.TextBox_eventName.Text == "Название события")
                eventMainInfo.EventName = "Без названия";
            else
                eventMainInfo.EventName = control.TextBox_eventName.Text;

    //Date from
            DateTime date = DateTime.Parse(control.datePicker_DateFrom.Text);
            String time = control.TextBox_timeFrom.Text;
            Int32 length = time.Length;
            Int32 hour = Int32.Parse(time.Substring(0, length - 3));
            Int32 min = Int32.Parse(time.Substring(length - 2, 2));

            DateTime dateFrom = new DateTime(date.Year, date.Month, date.Day, hour, min, 0);

            eventMainInfo.dateFrom = dateFrom;

    //Date To
            date = DateTime.Parse(control.datePicker_DateTo.Text);

            time = control.TextBox_timeTo.Text;
            length = time.Length;
            hour = Int32.Parse(time.Substring(0, length - 3));
            min = Int32.Parse(time.Substring(length - 2, 2));
            if(hour == 24)
            {
                hour = 23;
                min = 59;
            }

            DateTime dateTo = new DateTime(date.Year, date.Month, date.Day, hour, min, 0);

            eventMainInfo.dateTo = dateTo;

    //All day
            if (control.CheckBox_AllDay.IsChecked == true)
                eventInfo.AllDay = 1;
            else
                eventInfo.AllDay = 0;

    //Repeat Event

            if (control.CheckBox_RepeatEvent.IsChecked == true)
                eventInfo.Repeat = 1;
            else
                eventInfo.Repeat = 0;

    //Repeat Event Info

            if(eventInfo.Repeat == 1)
                RepeatEventInfo();
            
    //Repeat Event - weekday
            if (eventInfo.Repeat == 1 && eventInfo.enumRepeat == Calendarenums.EnumRepeat.Week)
                RepeatEventWeekInfo();

    //Remind Event

            if(control.CheckBox_Reminder.IsChecked == true)
            {
                eventInfo.Remind = 1;
                RemindEventInfo();
            }

    //Location
            if (control.TextBox_Locate.Text == "Место проведения")
                eventInfo.location = "Не указано";
            else
                eventInfo.location = control.TextBox_Locate.Text;

    //Description
            if (control.TextBox_Description.Text == "Описание")
                eventInfo.description = "Без описания";
            else
                eventInfo.description = control.TextBox_Description.Text;

    //EventInfo data setting clear string (not null)
            eventMainInfo.Data = "";
    
    //Creation date
            eventMainInfo.created = DateTime.Now;
    //Entity key
            eventMainInfo.ShareKey = "";

    //Sending Data
            calendarEventInfo.SendEvent(eventInfo, eventMainInfo);

    //Hide menu
            hideMenuEvent.SetHideMenuEvent(true);
        }

        public void RemindEventInfo()
        {
            if (control.ComboBox_enumRemindEvent.Text == "За пять минут")
                eventInfo.reminderDate = Calendarenums.ReminderDate.fiveMinutes;
            else if (control.ComboBox_enumRemindEvent.Text == "За час")
                eventInfo.reminderDate = Calendarenums.ReminderDate.Hour;
            else if (control.ComboBox_enumRemindEvent.Text == "За день")
                eventInfo.reminderDate = Calendarenums.ReminderDate.Day;
            eventInfo.Reminded = 1;
        }

        public void RepeatEventInfo()
        {
            String item = control.ComboBox_enumRepeatEvent.Text;
            if(item == "Каждый день")
            {
                eventInfo.enumRepeat = Calendarenums.EnumRepeat.Day;
            }
            else if (item == "Каждый будний день")
            {
                eventInfo.enumRepeat = Calendarenums.EnumRepeat.WorkDay;
            }
            else if (item == "Каждую неделю")
            {
                eventInfo.enumRepeat = Calendarenums.EnumRepeat.Week;
            }
            else if (item == "Каждый месяц")
            {
                eventInfo.enumRepeat = Calendarenums.EnumRepeat.Month;
            }
            else if (item == "Каждый год")
            {
                eventInfo.enumRepeat = Calendarenums.EnumRepeat.Yaer;
            }
        }

        public void RepeatEventWeekInfo()
        {
            eventInfo.DayOfWeek = new List<Calendarenums.DayOfWeekEnum>();
            if (control.checkBox_Monday.IsChecked == true)
                eventInfo.DayOfWeek.Add(Calendarenums.DayOfWeekEnum.Monday);
            if (control.checkBox_Tuesday.IsChecked == true)
                eventInfo.DayOfWeek.Add(Calendarenums.DayOfWeekEnum.Tuesday);
            if (control.checkBox_Wednesday.IsChecked == true)
                eventInfo.DayOfWeek.Add(Calendarenums.DayOfWeekEnum.Wednesday);
            if (control.checkBox_Thirsday.IsChecked == true)
                eventInfo.DayOfWeek.Add(Calendarenums.DayOfWeekEnum.Thirsday);
            if (control.checkBox_Friday.IsChecked == true)
                eventInfo.DayOfWeek.Add(Calendarenums.DayOfWeekEnum.Friday);
            if (control.checkBox_Saturday.IsChecked == true)
                eventInfo.DayOfWeek.Add(Calendarenums.DayOfWeekEnum.Saturday);
            if (control.checkBox_Sunday.IsChecked == true)
                eventInfo.DayOfWeek.Add(Calendarenums.DayOfWeekEnum.Sunday);
        }
    }
}
