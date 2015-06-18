using EventLibrsries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CalendarLibrary.Logic
{
    public class NextOrPreviousEvent
    {
        Events events;
        CalendarControl calendarCS;
        CalendarView calendarView;
        GetCurrentDateToDayControl_Event sendCurrentDateToDayControl;

        public NextOrPreviousEvent(CalendarControl calendarCS)
        {
            this.calendarCS = calendarCS;
            events = Events.CurrentEvents;
            
        }

        public void SetEvents()
        {
            CalendarChangeDirection_Event.changeDayEvent += CalendarChangeDirection_Event_changeDayEvent;
            this.calendarView = CalendarView.GetInstance;
            CalendarSelectWeekBorder_Event.GetEvent += CalendarSelectWeekBorder_Event_GetEvent;
            if (calendarView.selectedDate.Year < 100)
                calendarView.selectedDate = DateTime.Now;
            SetBorderStyle(calendarView.selectedDate.Day.ToString());

            sendCurrentDateToDayControl = new GetCurrentDateToDayControl_Event();
            GetCurrentDateToDayControl_Event.GetEvent += GetCurrentDateToDayControl_Event_GetEvent;
        }

        void GetCurrentDateToDayControl_Event_GetEvent(object sender, GetCurrentDateToDayControl e)
        {
            sendCurrentDateToDayControl.SendDate(calendarView.selectedDate);
        }

        void CalendarSelectWeekBorder_Event_GetEvent(object sender, CalendarSelectWeekBorder e)
        {
            if (!e.key)
                ClearBorderStyle();
            else
                SetBorderStyle(calendarView.selectedDate.Day.ToString());
        }

        private void CalendarChangeDirection_Event_changeDayEvent(object sender, CalendarChangeDays e)
        {
            if (e.direction == "left")
                LeftEvent(e.directionSize);
            else
                RightEvent(e.directionSize);
        }
//TODO: Events for goLeft and goRight (timetable)
        void LeftEvent(CalendarChangeDirection_Event.DirectionSize directionSize)
        {
            if (calendarView.selectedDate.Year < 100)
                calendarView.selectedDate = DateTime.Now;

            switch (directionSize)
            {
                case CalendarChangeDirection_Event.DirectionSize.Day:
                    if (calendarView.selectedDate.Day > 1)
                        ButtonClick(calendarView.selectedDate.AddDays(-1).Day.ToString());
                    else
                    {
                        calendarView.SetMonth(calendarView.buttonList, "previous", calendarCS.Border_sixthWeek);
                        Int32 daysInMonth = DateTime.DaysInMonth(calendarView.currentDate.Year, calendarView.currentDate.Month);
                        ButtonClick(calendarView.selectedDate.AddDays(-1).Day.ToString());
                        calendarCS.label_MonthName.Content = calendarView.GetMonthName();
                    }
                    break;
                case CalendarChangeDirection_Event.DirectionSize.Week:
                    DateTime prevDay;
                    if (calendarView.selectedDate.Day > 7)
                    {
                        prevDay = calendarView.selectedDate.AddDays(-7);
                        calendarView.selectedDate = prevDay;
                    }
                    else
                    {
                        calendarView.SetMonth(calendarView.buttonList, "previous", calendarCS.Border_sixthWeek);
                        prevDay = calendarView.selectedDate.AddDays(-7);
                        calendarView.selectedDate = prevDay;
                        calendarCS.label_MonthName.Content = calendarView.GetMonthName();
                    }
                    SetBorderStyle(calendarView.selectedDate.Day.ToString());
                    calendarView.SetMonth(calendarView.buttonList, "current", calendarCS.Border_sixthWeek);
                    calendarView.SendMondayDateToWeekPanel();
                    break;
                case CalendarChangeDirection_Event.DirectionSize.Month:
                    DateTime date = calendarView.selectedDate.AddMonths(-1);
                    calendarView.selectedDate = date;
                    calendarView.SendDateToMonth();
                    calendarView.SetMonth(calendarView.buttonList, "previous", calendarCS.Border_sixthWeek);
                    calendarCS.label_MonthName.Content = calendarView.GetMonthName();
                    
                    break;
                case CalendarChangeDirection_Event.DirectionSize.Timetable:

                    break;
                default:

                    break;
            }
        }

        void RightEvent(CalendarChangeDirection_Event.DirectionSize directionSize)
        {
            if (calendarView.selectedDate.Year < 100)
                calendarView.selectedDate = DateTime.Now;

            switch (directionSize)
            {
                case CalendarChangeDirection_Event.DirectionSize.Day:
                    Int32 daysInMonth = DateTime.DaysInMonth(calendarView.currentDate.Year, calendarView.currentDate.Month);

                    if (calendarView.selectedDate.Day < daysInMonth)
                        ButtonClick(calendarView.selectedDate.AddDays(1).Day.ToString());
                    else
                    {
                        calendarView.SetMonth(calendarView.buttonList, "next", calendarCS.Border_sixthWeek);
                        ButtonClick("1");
                        calendarCS.label_MonthName.Content = calendarView.GetMonthName();
                    }
                    break;
                case CalendarChangeDirection_Event.DirectionSize.Week:
                    DateTime nextDay;
                    Int32 daysInMonth_week = DateTime.DaysInMonth(calendarView.selectedDate.Year, calendarView.selectedDate.Month);
                    if (calendarView.selectedDate.Day < daysInMonth_week - 6)
                    {
                        nextDay = calendarView.selectedDate.AddDays(7);
                        calendarView.selectedDate = nextDay;
                    }
                    else
                    {
                        calendarView.SetMonth(calendarView.buttonList, "next", calendarCS.Border_sixthWeek);
                        nextDay = calendarView.selectedDate.AddDays(7);
                        calendarView.selectedDate = nextDay;
                        calendarCS.label_MonthName.Content = calendarView.GetMonthName();
                    }
                    SetBorderStyle(calendarView.selectedDate.Day.ToString());
                    calendarView.SetMonth(calendarView.buttonList, "current", calendarCS.Border_sixthWeek);
                    calendarView.SendMondayDateToWeekPanel();
                    break;
                case CalendarChangeDirection_Event.DirectionSize.Month:
                    DateTime date = calendarView.selectedDate.AddMonths(1);
                    calendarView.selectedDate = date;
                    calendarView.SendDateToMonth();
                    calendarView.SetMonth(calendarView.buttonList, "next", calendarCS.Border_sixthWeek);
                    calendarCS.label_MonthName.Content = calendarView.GetMonthName();
                    break;
                case CalendarChangeDirection_Event.DirectionSize.Timetable:

                    break;
                default:

                    break;
            }
        }

        void ButtonClick(String buttonContent)
        {
            
            Button button = new Button();
            foreach(Button but in calendarView.buttonList)
            {
                if (but.Content.ToString() == buttonContent && but.Foreground != Brushes.Gray)
                {
                    button = but;
                    break;
                }
            }
            if(button.Content.ToString() != "")
                button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                
        }

        void ClearBorderStyle()
        {
            foreach (Border bord in calendarView.borderList)
            {
                if (bord.BorderThickness == new Thickness(2))
                    bord.BorderThickness = new Thickness(0);
            }
        }

        public void SetBorderStyle(String buttonContent)
        {
            ClearBorderStyle();
            Border border = new Border();
            foreach(Button button in calendarView.buttonList)
            {
                if(button.Content.ToString() == buttonContent && button.Foreground != Brushes.Gray)
                {
                    border = (button.Parent as Grid).Parent as Border;
                    break;
                }
            }

            foreach(Border bord in calendarView.borderList)
                bord.BorderThickness = new Thickness(0);

            if (border.BorderThickness == new Thickness(0))
            {
                border.BorderThickness = new Thickness(2);
            }

        }
    }
}
