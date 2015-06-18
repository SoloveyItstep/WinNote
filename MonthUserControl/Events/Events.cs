using EventLibrsries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace MonthUserControl.Events
{
    class Events
    {
        MonthControl control;
        Visual.VisualStyle visual;
        LoadInfo loadInfo;
        SendDatesTo_AddEventMenu_Event senddatesToAddEventMenu;
        MonthControl_Popup monthControlPopup;
        System.Windows.Threading.DispatcherTimer timer;
        SendEventWidthIeventInfoToAddEventMenu_Event sendEventToChangeIt;
        SendEventToDeleteEventInfo_Event sendEventToDelete;
        SendEventAboutChanges_Event sendEventAboutChanges;

        public Events(MonthControl control)
        {
            this.control = control;
            visual = Visual.VisualStyle.GetInstance(control);
            loadInfo = new LoadInfo(control);
            monthControlPopup = new MonthControl_Popup();
            sendEventAboutChanges = new SendEventAboutChanges_Event();
            sendEventToChangeIt = new SendEventWidthIeventInfoToAddEventMenu_Event();
            sendEventToDelete = new SendEventToDeleteEventInfo_Event();
        }

        public void LoadComponents()
        {
            visual.InitializeComponents();
            visual.FillByColumnsAndRows();
            visual.FillByGrids();
            visual.FillTopRows();
            visual.FillGrids();
            visual.SetDays();

            SelectTimer();
            
        }

        void SelectTimer()
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0,0,3);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            control.popup.IsOpen = false;
            control.dockPanelPopup.Children.Clear();
            timer.Stop();
        }

        public void SelectEvents()
        {
            EventToGetSelectedDate_Event.GetSelectedDate += EventToGetSelectedDate_Event_GetSelectedDate;
            LoadEventInfo_Event.GetEvent += LoadEventInfo_Event_GetEvent;
            SendToMonthPanel_CurrentDate_Event.GetDateEvent += SendToMonthPanel_CurrentDate_Event_GetDateEvent;
            senddatesToAddEventMenu = new SendDatesTo_AddEventMenu_Event();
            foreach(Grid grid in visual.variables.gridList)
                grid.MouseLeftButtonDown += grid_MouseLeftButtonDown;

            foreach (List<Hyperlink> lst in visual.variables.daysHyperlinkDictionary.Values)
            {
                foreach (Hyperlink hyperlink in lst)
                {
                    hyperlink.MouseEnter += hyperlink_MouseEnter;
                }
            }
            control.popup.MouseEnter += popup_MouseEnter;
            control.popup.MouseLeave += popup_MouseLeave;
            control.MouseDown += control_MouseDown;
            MonthControl_Popup.GetMouseLeaveEvent += MonthControl_Popup_GetMouseLeaveEvent;
            control.ButtonPopupChange.Click += ButtonPopupChange_Click;
            control.ButtonPopupRemove.Click += ButtonPopupRemove_Click;
            CleanEvent_Event.GetEvent += CleanEvent_Event_GetEvent;
        }

        void CleanEvent_Event_GetEvent(object sender, CleanEvent e)
        {
            foreach (List<Hyperlink> lst in visual.variables.daysHyperlinkDictionary.Values)
                foreach (Hyperlink hyperlink in lst)
                    hyperlink.Inlines.Clear();

            visual.variables.selectedDate = DateTime.Now;
            foreach (Grid grid in visual.variables.gridList)
                grid.Background = Brushes.White;

            foreach (Hyperlink hyperlink in visual.variables.hyperlinkList)
                hyperlink.Inlines.Clear();

            foreach (Label label in visual.variables.labelsDaysNameList)
                label.Content = "";

        }

        void ButtonPopupRemove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            sendEventToDelete.Sendinfo(visual.mypopup.currentEventMainInfo, visual.mypopup.currentEventInfo);
            Thread.Sleep(25);
            sendEventAboutChanges.SendEvent(true);
            control.popup.IsOpen = false;
        }

        void ButtonPopupChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            sendEventToChangeIt.SendEvent(visual.mypopup.currentEventMainInfo, visual.mypopup.currentEventInfo);
            control.popup.IsOpen = false;
        }

        void popup_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            timer.Stop();
        }

        void MonthControl_Popup_GetMouseLeaveEvent(object sender, MonthControlMouseLeave_SendToPopup e)
        {
            if(control.popup.IsOpen && !control.popup.IsMouseCaptured)
            {
                control.popup.IsOpen = false;
                monthControlPopup.SendPopupIsOpenEvent(false);
            }
        }

        void control_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            control.popup.IsOpen = false;
        }
      
        void popup_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            control.popup.IsOpen = false;            
        }

        void hyperlink_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Hyperlink hyperlink = sender as Hyperlink;
            control.popup.Placement = PlacementMode.Mouse;
            if (control.popup.IsOpen)
            {
                control.popup.IsOpen = false;
            }
            control.popup.IsOpen = true;
            timer.Start();
            
            Int32 index = Int32.Parse(hyperlink.Name.Substring(1));
            Int32 hyperlinkIndex = GetHyperlinkIndex(ref hyperlink, index);
            Int32 day = GetDayFromHyperlink(index);
            loadInfo.SetPopupEvents((hyperlink.Inlines.FirstOrDefault() as Run).Text, day, hyperlinkIndex,true);
        }

        Int32 GetDayFromHyperlink(Int32 index)
        {
            
            foreach(Label label in visual.variables.labelsDaysNameList)
            {
                if (label.Name == "l" + index)
                    return Int32.Parse(label.Content.ToString());
            }
            return 0;
        }

        Int32 GetHyperlinkIndex(ref Hyperlink hyperlink, Int32 listIndex)
        {
            List<Hyperlink> lst = visual.variables.daysHyperlinkDictionary["d" + listIndex.ToString()];
            
            Int32 index = 0;
            foreach(Hyperlink hyp in lst)
            {
                if (hyp.Name == hyperlink.Name)
                {
                    ++index;
                    break;
                }
                ++index;
            }
            return index;
        }

        void grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            Int32 i = Int32.Parse(grid.Name.Substring(1)) - 1;
            if(visual.variables.labelsDaysNameList[i].Foreground != Brushes.Gray)
            {
                DateTime date = new DateTime(visual.variables.selectedDate.Year, visual.variables.selectedDate.Month, 
                    Int32.Parse(visual.variables.labelsDaysNameList[i].Content.ToString()),DateTime.Now.Hour,DateTime.Now.Minute,0);
                senddatesToAddEventMenu.SendEvent(date, date);
            }
        }

        void SendToMonthPanel_CurrentDate_Event_GetDateEvent(object sender, SendToMonthPanel_CurrentDate e)
        {
            visual.variables.selectedDate = e.date;
            visual.SetDays();
        }

        void LoadEventInfo_Event_GetEvent(object sender, LoadEventInfo e)
        {
            if (e.calendarEnum == LoadEventInfo_Event.CalendarEnum.Month)
            {
                visual.eventInfo.dictionatyEventInfo = e.dictionaryInfo;
                loadInfo.UpdateInfo();
            }
        }

        void EventToGetSelectedDate_Event_GetSelectedDate(object sender, GetEventWithySelectedDate e)
        {
            if (visual.variables.selectedDate != e.date)
            {
                visual.variables.selectedDate = e.date;
                visual.SetDays();
            }
        }
    }
}
