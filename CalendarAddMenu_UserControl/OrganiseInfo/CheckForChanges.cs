using CalendarAddMenu_UserControl.Model;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalendarAddMenu_UserControl.OrganiseInfo
{
    public class CheckForChanges
    {
        CalendarAddMenu_UserControl control;
        VarEventInfo eventInfo;
        public CheckForChanges(CalendarAddMenu_UserControl control)
        {
            this.control = control;
            eventInfo = VarEventInfo.GetInstance;
        }

        public Boolean CheckInfo()
        {
            if (CheckNameEquels())
                return true;
            if (CheckDatesEquels())
                return true;
            if (control.CheckBox_AllDay.IsChecked == true && eventInfo.eventInfo.AllDay == 0 || control.CheckBox_AllDay.IsChecked == false && eventInfo.eventInfo.AllDay == 1)
                return true;
            if (control.CheckBox_RepeatEvent.IsChecked == false && eventInfo.eventInfo.Repeat == 1 || control.CheckBox_RepeatEvent.IsChecked == true && eventInfo.eventInfo.Repeat == 0)
                return true;
            else if (control.CheckBox_RepeatEvent.IsChecked == true)
                if (ChechRepeatEventEquels())
                    return true;
            if (control.CheckBox_Reminder.IsChecked == true && eventInfo.eventInfo.Remind == 0 || control.CheckBox_Reminder.IsChecked == false && eventInfo.eventInfo.Remind == 1)
                return true;
            else if(control.CheckBox_Reminder.IsChecked == true)
            {
                if (CheckRemindEventEquels())
                    return true;
            }

            if (control.TextBox_Locate.Text != eventInfo.eventInfo.location)
                return true;
            if (control.TextBox_Description.Text != eventInfo.eventInfo.description)
                return true;

            return false;
        }

        Boolean CheckRemindEventEquels()
        {

            if (control.ComboBox_enumRemindEvent.SelectedIndex == -1)
                return true;
            if (control.ComboBox_enumRemindEvent.Text == "За пять минут" && eventInfo.eventInfo.reminderDate != Calendarenums.ReminderDate.fiveMinutes)
                return true;
            if (control.ComboBox_enumRemindEvent.Text == "За час" && eventInfo.eventInfo.reminderDate != Calendarenums.ReminderDate.Hour)
                return true;
            if (control.ComboBox_enumRemindEvent.Text == "За день" && eventInfo.eventInfo.reminderDate != Calendarenums.ReminderDate.Day)
                return true;

            return false;
        }

        Boolean ChechRepeatEventEquels()
        {
            if (control.ComboBox_enumRepeatEvent.SelectedIndex > -1)
                return true;
            if (control.ComboBox_enumRepeatEvent.SelectedIndex == 0 && eventInfo.eventInfo.enumRepeat != Calendarenums.EnumRepeat.Day)
                return true;
            else if (control.ComboBox_enumRepeatEvent.SelectedIndex == 1 && eventInfo.eventInfo.enumRepeat != Calendarenums.EnumRepeat.WorkDay)
                return true;
            else if (control.ComboBox_enumRepeatEvent.SelectedIndex == 2 && eventInfo.eventInfo.enumRepeat != Calendarenums.EnumRepeat.Week)
                return true;
            else if (control.ComboBox_enumRepeatEvent.SelectedIndex == 3 && eventInfo.eventInfo.enumRepeat != Calendarenums.EnumRepeat.Month)
                return true;
            else if (control.ComboBox_enumRepeatEvent.SelectedIndex == 4 && eventInfo.eventInfo.enumRepeat != Calendarenums.EnumRepeat.Yaer)
                return true;

            if(control.ComboBox_enumRepeatEvent.SelectedIndex == 2)
            {
                List<Calendarenums.DayOfWeekEnum> lst = new List<Calendarenums.DayOfWeekEnum>();
                if (control.checkBox_Monday.IsChecked == true)
                    lst.Add(Calendarenums.DayOfWeekEnum.Monday);
                if(control.checkBox_Tuesday.IsChecked == true)
                    lst.Add(Calendarenums.DayOfWeekEnum.Tuesday);
                if (control.checkBox_Wednesday.IsChecked == true)
                    lst.Add(Calendarenums.DayOfWeekEnum.Wednesday);
                if (control.checkBox_Thirsday.IsChecked == true)
                    lst.Add(Calendarenums.DayOfWeekEnum.Thirsday);
                if (control.checkBox_Friday.IsChecked == true)
                    lst.Add(Calendarenums.DayOfWeekEnum.Friday);
                if (control.checkBox_Saturday.IsChecked == true)
                    lst.Add(Calendarenums.DayOfWeekEnum.Saturday);
                if (control.checkBox_Sunday.IsChecked == true)
                    lst.Add(Calendarenums.DayOfWeekEnum.Sunday);

                if(lst.Count != eventInfo.eventInfo.DayOfWeek.Count)
                    return true;
                for(Int32 count = 0; count < lst.Count; ++count)
                {
                    if (lst[count] != eventInfo.eventInfo.DayOfWeek[count])
                        return true;
                }
            }

                return false;
        }

        Boolean CheckNameEquels()
        {
            if (eventInfo.eventMainInfo == null)
                return false;
            if (control.TextBox_eventName.Text != eventInfo.eventMainInfo.EventName)
                return true;
            return false;
        }

        Boolean CheckDatesEquels()
        {
            if (eventInfo.eventMainInfo == null)
                return false;

            DateTime dateFrom, dateTo = DateTime.Now;
            dateFrom = DateTime.Parse(control.datePicker_DateFrom.Text + " " + control.TextBox_timeFrom.Text);
            if (!new Regex(@"^24:(0{1,2})").IsMatch(control.TextBox_timeTo.Text))
                dateTo = DateTime.Parse(control.datePicker_DateTo.Text + " " + control.TextBox_timeTo.Text);
            else
                dateTo = DateTime.Parse(control.datePicker_DateTo.Text + " 23:59:00");
            if (!dateFrom.Equals(eventInfo.eventMainInfo.dateFrom))
                return true;
            if (!dateTo.Equals(eventInfo.eventMainInfo.dateTo))
                return true;


            return false;
        }
    }
}
