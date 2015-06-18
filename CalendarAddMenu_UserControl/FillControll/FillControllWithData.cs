using CalendarAddMenu_UserControl.Event;
using CalendarAddMenu_UserControl.Model;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CalendarAddMenu_UserControl.FillControll
{
    public class FillControllWithData
    {
        public CalendarAddMenu_UserControl control;
        VarEventInfo varEventInfo;
        AnimationEvents animationEvent;

        public FillControllWithData(CalendarAddMenu_UserControl control)
        {
            this.control = control;
            varEventInfo = VarEventInfo.GetInstance;
            animationEvent = AnimationEvents.GetInstance(control);
        }
       
        public void FillData()
        {
            if (varEventInfo.eventMainInfo == null)
                return;
    //Event name
            if (varEventInfo.eventMainInfo.EventName != "Без названия")
            {
                control.TextBox_eventName.Foreground = Brushes.Gray;
                control.TextBox_eventName.Text = varEventInfo.eventMainInfo.EventName;
            }
    //Dates
            control.datePicker_DateFrom.Text = varEventInfo.eventMainInfo.dateFrom.ToShortDateString();
            control.TextBox_timeFrom.Text = String.Format("{0}:{1}",varEventInfo.eventMainInfo.dateFrom.Hour,varEventInfo.eventMainInfo.dateFrom.Minute);
            control.datePicker_DateTo.Text = varEventInfo.eventMainInfo.dateTo.ToShortDateString();
            control.TextBox_timeTo.Text = String.Format("{0}:{1}",varEventInfo.eventMainInfo.dateTo.Hour,varEventInfo.eventMainInfo.dateTo.Minute);
    //All day checkBox
            if (varEventInfo.eventInfo.AllDay == 1)
                control.CheckBox_AllDay.IsChecked = true;
    //Repeat event
            if(varEventInfo.eventInfo.Repeat == 1)
            {
                control.CheckBox_RepeatEvent.IsChecked = true;
                control.Height += 60;
                control.Grid_Reminder_menu.Margin = new System.Windows.Thickness(0,control.Grid_Reminder_menu.Margin.Top + 60,0,0);
                control.Grid_BottomMenu.Margin = new System.Windows.Thickness(0,control.Grid_BottomMenu.Margin.Top + 60,0,0);
                control.Grid_RemindEvent.Margin = new System.Windows.Thickness(0,control.Grid_RemindEvent.Margin.Top + 60,0,0);
                SetRepeatEventInfo(varEventInfo.eventInfo.enumRepeat);
            }
    //Reminde event
            if(varEventInfo.eventInfo.Remind == 1)
            {
                control.CheckBox_Reminder.IsChecked = true;
                control.Height += 60;
                control.Grid_BottomMenu.Margin = new System.Windows.Thickness(0,control.Grid_BottomMenu.Margin.Top + 60,0,0);
                SetRemindEventInfo(varEventInfo.eventInfo.reminderDate);
            }
    //Location
            if (varEventInfo.eventInfo.location != "Не указано")
            {
                control.TextBox_Locate.Text = varEventInfo.eventInfo.location;
                control.TextBox_Locate.Foreground = Brushes.Gray;
            }
    //Description
            if (varEventInfo.eventInfo.description != "Без описания")
            {
                control.TextBox_Description.Text = varEventInfo.eventInfo.description;
                control.TextBox_Description.Foreground = Brushes.Gray;
            }
            //CheckSise();
        }
        //Grid_RepeatingEvent  Grid_RepeatEvent_Weeks  Grid_RemindEvent Grid_BottomMenu-Margin="0,310,0,0"
        void CheckSise()
        {
            Int32 height = 0;
            if(control.CheckBox_RepeatEvent.IsChecked == true)
            {
                height = 60;
                if (control.ComboBox_enumRepeatEvent.SelectedIndex == 2)
                {
                    height = 120;
                    control.Grid_RepeatEvent_Weeks.Height = 60;
                }
                control.Grid_RepeatingEvent.Height = height;
                height += 60;
            }
            control.Grid_RemindEvent.Margin = new System.Windows.Thickness(0,control.Grid_RepeatingEvent.Margin.Top + height,0,0);
            
            if(control.CheckBox_Reminder.IsChecked == true)
            {
                
                control.Grid_Reminder_menu.Height = 60;
                control.Grid_Reminder_menu.Margin = new System.Windows.Thickness(0, control.Grid_RepeatingEvent.Margin.Top + height, 0, 0);
                control.Grid_BottomMenu.Margin = new System.Windows.Thickness(0,control.Grid_BottomMenu.Margin.Top + 60,0,0);
                height += 60;
            }
            //control.Grid_Reminder_menu.Margin = new System.Windows.Thickness(0, control.Grid_RepeatingEvent.Margin.Top + height, 0, 0);
            //control.Grid_BottomMenu.Margin = new System.Windows.Thickness(0,control.Grid_RepeatingEvent.Margin.Top + height, 0, 0);
            
        }
        void SetRemindEventInfo(Calendarenums.ReminderDate eventInfo)
        {
            switch(eventInfo)
            {
                case Calendarenums.ReminderDate.fiveMinutes:
                    control.ComboBox_enumRemindEvent.SelectedIndex = 0;
                    break;
                case Calendarenums.ReminderDate.Hour:
                    control.ComboBox_enumRemindEvent.SelectedIndex = 1;
                    break;
                case Calendarenums.ReminderDate.Day:
                    control.ComboBox_enumRemindEvent.SelectedIndex = 2;
                    break;
            }
        }
        void SetRepeatEventInfo(Calendarenums.EnumRepeat eventInfo)
        {
            switch(eventInfo)
            {
                case Calendarenums.EnumRepeat.Day:
                    control.ComboBox_enumRepeatEvent.SelectedIndex = 0;
                    break;
                case Calendarenums.EnumRepeat.WorkDay:
                    control.ComboBox_enumRepeatEvent.SelectedIndex = 1;
                    break;
                case Calendarenums.EnumRepeat.Week:
                    control.ComboBox_enumRepeatEvent.SelectedIndex = 2;
                    control.Height = control.Height + 60;
                    control.Grid_RemindEvent.Margin = new System.Windows.Thickness(0, control.Grid_RemindEvent.Margin.Top + 60,0,0);
                    control.Grid_Reminder_menu.Margin = new System.Windows.Thickness(0,control.Grid_Reminder_menu.Margin.Top + 60,0,0);
                    control.Grid_BottomMenu.Margin = new System.Windows.Thickness(0,control.Grid_BottomMenu.Margin.Top + 60,0,0);
                    SetRepeatEventDays();
                    break;
                case Calendarenums.EnumRepeat.Month:
                    control.ComboBox_enumRepeatEvent.SelectedIndex = 3;
                    break;
                case Calendarenums.EnumRepeat.Yaer:
                    control.ComboBox_enumRepeatEvent.SelectedIndex = 4;
                    break;
            }
            //animationEvent.ComboBox_SelectionChanged(control.ComboBox_enumRepeatEvent, null);
        }
        void SetRepeatEventDays()
        {  
                foreach(Calendarenums.DayOfWeekEnum day in varEventInfo.eventInfo.DayOfWeek)
                {
                    switch(day)
                    {
                        case Calendarenums.DayOfWeekEnum.Monday:
                            control.checkBox_Monday.IsChecked = true;
                            break;
                        case Calendarenums.DayOfWeekEnum.Tuesday:
                            control.checkBox_Tuesday.IsChecked = true;
                            break;
                        case Calendarenums.DayOfWeekEnum.Wednesday:
                            control.checkBox_Wednesday.IsChecked = true;
                            break;
                        case Calendarenums.DayOfWeekEnum.Thirsday:
                            control.checkBox_Thirsday.IsChecked = true;
                            break;
                        case Calendarenums.DayOfWeekEnum.Friday:
                            control.checkBox_Friday.IsChecked = true;
                            break;
                        case Calendarenums.DayOfWeekEnum.Saturday:
                            control.checkBox_Saturday.IsChecked = true;
                            break;
                        case Calendarenums.DayOfWeekEnum.Sunday:
                            control.checkBox_Sunday.IsChecked = true;
                            break;
                    }
                }
        }
    }
}
