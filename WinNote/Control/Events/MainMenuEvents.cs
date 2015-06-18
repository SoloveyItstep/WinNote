using EventLibrsries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using WinNote.View;

namespace WinNote.Control.Events
{
    class MainMenuEvents
    {
        MainView mainView;
        ButtonEventNames buttonEvents;
        CalendarChangeDirection_Event calendarChangeDirection;
        String calendarCurrentPanelName;
        GetCurrentCalendarPanel_Event getCurrentPanelName;
        CalendarSelectWeekBorder_Event selectCalendarBorderEvent;

        public MainMenuEvents()
        {
            selectCalendarBorderEvent = new CalendarSelectWeekBorder_Event();
        }

        public void Events()
        {
            buttonEvents = new ButtonEventNames();
            ChangeMenuEvent.changeMenuEvent += ChangeMenuEvent_changeMenuEvent;
            TopMenuButtonEvents.buttonEvent += TopMenuButtonEvents_buttonEvent;
            Date_CkickEvent.buttonsEvent += ev1_buttonsEvent;
            mainView = MainView.mainView;
            calendarChangeDirection = new CalendarChangeDirection_Event();
            getCurrentPanelName = new GetCurrentCalendarPanel_Event();
            GetCalendarCurrentPanelName.SendEvent += GetCalendarCurrentPanelName_SendEvent;

        }

        void GetCalendarCurrentPanelName_SendEvent(object sender, GetCurrentCalendarPanel e)
        {
            calendarCurrentPanelName = e.panelName;
        }
       
        private void ev1_buttonsEvent(object sender, ButtonsEvent e)
        {
           if (mainView == null)
               mainView = MainView.mainView;
            
            mainView.SetDayDateTime(e.date);
            
            buttonEvents.SetButton("day");

        }

//TODO: events for - GoLeft and GoRight buttons (timetable)
        void TopMenuButtonEvents_buttonEvent(object sender, TopMenuButtonClick e)
        {
            if (mainView == null)
                mainView = MainView.mainView;
            //System.Windows.Forms.MessageBox.Show(e.buttonName);
            if (e.buttonName == "Button_GoLeft")
            {
                CalendarChangeDirection("Button_GoLeft");
                //System.Windows.MessageBox.Show("go left - mainEvents");
            }
            else if (e.buttonName == "Button_GoRight")
            {
                CalendarChangeDirection("Button_GoRight");
                //System.Windows.MessageBox.Show("go right - mainEvents");
            }
            else if (e.buttonName == "Button_AddEvent")
            {
                mainView.ShowClaendarAddEventMenu(DateTime.Now, DateTime.Now);
            }
            else if (e.buttonName == "Button_Day")
            {
                mainView.calendarActivePanel = MainView.CalendarPanelActive.Day;   
                selectCalendarBorderEvent.SetEvent(false);
                //mainView.SetFocus_CalendarTopPanelButtons("Day");
            }
            else if (e.buttonName == "Button_Week")
            {
                mainView.calendarActivePanel = MainView.CalendarPanelActive.Week;
                selectCalendarBorderEvent.SetEvent(true);
                mainView.SetFocus_CalendarTopPanelButtons("Week");
            }
            else if (e.buttonName == "Button_Month")
            {
                mainView.calendarActivePanel = MainView.CalendarPanelActive.Month;
                selectCalendarBorderEvent.SetEvent(false);
                mainView.SetFocus_CalendarTopPanelButtons("Month");
            }
            else if (e.buttonName == "Button_Timetable")
            {
                mainView.calendarActivePanel = MainView.CalendarPanelActive.Timetable;
                selectCalendarBorderEvent.SetEvent(false);
                mainView.SetFocus_CalendarTopPanelButtons("Timetable");
            }
        }

        void CalendarChangeDirection(String direction)
        {
            getCurrentPanelName.GetPanelName(true);
            String direct = "";
            if (direction == "Button_GoLeft")
            {
                direct = "left";
            }
            else if (direction == "Button_GoRight")
            {
                direct = "right";
            }
            
            CalendarChangeDirection_Event.DirectionSize directionSize = CalendarChangeDirection_Event.DirectionSize.Day;

            if (calendarCurrentPanelName == "Day")
            {
                directionSize = CalendarChangeDirection_Event.DirectionSize.Day;
            }
            else if (calendarCurrentPanelName == "Week")
            {
                directionSize = CalendarChangeDirection_Event.DirectionSize.Week;
            }
            else if (calendarCurrentPanelName == "Month")
            {
                directionSize = CalendarChangeDirection_Event.DirectionSize.Month;
            }
            else if (calendarCurrentPanelName == "Timetable")
            {
                directionSize = CalendarChangeDirection_Event.DirectionSize.Timetable;
            }
            calendarChangeDirection.SelectEvent(direct, directionSize);
        }

        void ChangeMenuEvent_changeMenuEvent(object sender, ChangeMenu e)
        {
            if(e.message == "calendar")
            {
                
            }
            else if(e.message == "notepad")
            {

            }
            else if(e.message == "wallet")
            {

            }
        }
    }

}
