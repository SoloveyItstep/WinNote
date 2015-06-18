using EventLibrsries;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace WeekUserControl.Events
{
    public class Events
    {
        VisualStyles visual;
        WeekControl control;
        WeekMouseMove_afterClick_Event weekMouseMoveEvent;
        SendToWeek_MondayDate_Event sendToCalendarEventToGetMonday;
        SendDatesTo_AddEventMenu_Event sendDatesToAddMenu;
        Dates dates;
        InfoEvents info;
        MyPopup mypopup;
        SendEventToDeleteEventInfo_Event sendEventToDelete;
        SendEventAboutChanges_Event sendEventAboutChanges;
        SendEventWidthIeventInfoToAddEventMenu_Event sendEventToChangeIt;
        DispatcherTimer timer;

        public Events(WeekControl control)
        {
            this.control = control;
            visual = VisualStyles.GetInstance(control);
            SelectViualFill();
            weekMouseMoveEvent = new WeekMouseMove_afterClick_Event();
            dates = new Dates();
            sendDatesToAddMenu = new SendDatesTo_AddEventMenu_Event();
            info = new InfoEvents(control);
            mypopup = MyPopup.GetInstance;
            sendEventToDelete = new SendEventToDeleteEventInfo_Event();
            sendEventAboutChanges = new SendEventAboutChanges_Event();
            sendEventToChangeIt = new SendEventWidthIeventInfoToAddEventMenu_Event();
        }

        public void SelectViualFill()
        {
            visual.CreateVariables();
            visual.SelectColumnsAndRows();
            visual.FillColumnsAndRows();
            visual.SelectBorders();
            visual.FillTopRows();
            visual.FillMiddleRows();
        }

        public void SetEvents()
        {
            foreach(Grid grid in visual.variables.allMiddleGrids)
                grid.MouseLeftButtonDown += grid_MouseLeftButtonDown;

            control.MouseMove += control_MouseMove;
            control.MouseLeftButtonUp += control_MouseLeftButtonUp;
            SendToWeek_MondayDate_Event.GetEvent += SendToWeek_MondayDate_Event_GetEvent;
            sendToCalendarEventToGetMonday = new SendToWeek_MondayDate_Event();
            sendToCalendarEventToGetMonday.SendEventToSendMonday(true);
            info.Events();
            SelectTimer();
            SetHyperlinkEvents();

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
            visual.ClearGrids();
            visual.ClearHyperlinks();
            visual.variables.columnNum = 0;
            visual.variables.gridName = "";
            foreach (Label label in visual.variables.labelsList)
                label.Content = "";
            visual.variables.selectedDate = DateTime.Now;
            visual.variables.selectedIndex = 0;
        }

        void SetHyperlinkEvents()
        {
            foreach(Hyperlink hyperlink in visual.hyperlinkLists.monday)
                hyperlink.MouseEnter += hyperlink_MouseEnter;

            foreach (Hyperlink hyperlink in visual.hyperlinkLists.tuesday)
                hyperlink.MouseEnter += hyperlink_MouseEnter;

            foreach (Hyperlink hyperlink in visual.hyperlinkLists.wednesday)
                hyperlink.MouseEnter += hyperlink_MouseEnter;

            foreach (Hyperlink hyperlink in visual.hyperlinkLists.thirsday)
                hyperlink.MouseEnter += hyperlink_MouseEnter;

            foreach (Hyperlink hyperlink in visual.hyperlinkLists.friday)
                hyperlink.MouseEnter += hyperlink_MouseEnter;

            foreach (Hyperlink hyperlink in visual.hyperlinkLists.saturday)
                hyperlink.MouseEnter += hyperlink_MouseEnter;

            foreach (Hyperlink hyperlink in visual.hyperlinkLists.sunday)
                hyperlink.MouseEnter += hyperlink_MouseEnter;
        }

        void popup_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            control.popup.IsOpen = false;
        }

        void hyperlink_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Thread.Sleep(200);
            Hyperlink hyperlink = sender as Hyperlink;
            if (hyperlink.Inlines.Count == 0)
                return;
            control.popup.Placement = PlacementMode.Mouse;
            if (control.popup.IsOpen)
            {
                control.popup.IsOpen = false;
            }
            control.popup.IsOpen = true;
            timer.Start();


            Int32 hyperlinkIndex = Int32.Parse(hyperlink.Name.Substring(2));
            Int32 day = Int32.Parse(hyperlink.Name[1].ToString());
            SetPopupEvents((hyperlink.Inlines.FirstOrDefault() as Run).Text, day, hyperlinkIndex, true);
        }      

        Int32 GetDayFromHyperlink(Int32 index)
        {

            foreach (Label label in visual.variables.labelsList)
            {
                if (label.Name == "l" + index)
                    return Int32.Parse(label.Content.ToString());
            }
            return 0;
        }

        public void SetPopupEvents(String name, Int32 day, Int32 index, Boolean key)
        {
            control.dockPanelPopup.Children.Clear();
            Int32 dayCount = 0, count = 0, hyperlinkCount = 0;
            if (key)
            {
                mypopup.popupHyperlinkDictionary.Clear();
                mypopup.hyperlinkDay = 0;
                mypopup.hyperlinkIndex.Clear();
            }
            String strDate = visual.variables.labelsList[day - 1].Content.ToString().Substring(visual.variables.labelsList[day - 1].Content.ToString().Length - 2);
            Int32 Day = 0;
            if (Char.IsDigit(strDate[0]))
                Day = Int32.Parse(strDate);
            else
                Day = Int32.Parse(strDate[1].ToString());

            foreach (var v in info.info.infoDictionary)
            {
                if (v.Key.dateFrom.Day == Day && v.Key.EventName == name)
                {
                    if (control.GridButtons.Visibility == System.Windows.Visibility.Hidden)
                        control.GridButtons.Visibility = System.Windows.Visibility.Visible;

                    mypopup.currentEventMainInfo = null;
                    mypopup.currentEventInfo = null;

                    String str = "";
                    str += String.Format("{0}\n\nДата с: {1}\nДата до:{2}\n", v.Key.EventName, v.Key.dateFrom, v.Key.dateTo);
                    str += String.Format("Описание: {0}\n\nМесто: {1}", v.Value.description, v.Value.location);
                    TextBlock txt = new TextBlock();
                    txt.TextWrapping = System.Windows.TextWrapping.Wrap;
                    txt.Text = str;
                    txt.Margin = new System.Windows.Thickness(10, 10, 10, 10);

                    mypopup.currentEventMainInfo = v.Key;
                    mypopup.currentEventInfo = v.Value;

                    control.dockPanelPopup.Children.Add(txt);
                }
                if (name.Length > 8 && name.Substring(name.Length - 8) == " событий" && count >= 4 && v.Key.dateFrom.Day == Day)
                {
                    if (control.GridButtons.Visibility == System.Windows.Visibility.Visible)
                        control.GridButtons.Visibility = System.Windows.Visibility.Hidden;

                    TextBlock txt = new TextBlock();
                    txt.TextWrapping = System.Windows.TextWrapping.Wrap;
                    txt.Margin = new System.Windows.Thickness(5, 20 * hyperlinkCount, 5, 5);
                    txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                    Hyperlink hyperlink = new Hyperlink();
                    hyperlink.Name = "h" + hyperlinkCount.ToString();
                    hyperlink.Inlines.Add(v.Key.EventName);
                    txt.Inlines.Add(hyperlink);

                    mypopup.popupHyperlinkDictionary.Add(index + hyperlinkCount, hyperlink);
                    mypopup.hyperlinkDay = day;

                    hyperlink.PreviewMouseLeftButtonDown += hyperlink_MouseLeftButtonDown;
                    hyperlinkCount++;

                    control.dockPanelPopup.Children.Add(txt);
                }
                ++dayCount;
                if (v.Key.dateFrom.Day == Day)
                    count++;
            }
        }

        void hyperlink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Hyperlink hyperlink = sender as Hyperlink;
            String name = (hyperlink.Inlines.FirstOrDefault() as Run).Text;
            Int32 countIndex = 0;
            foreach (Hyperlink hyp in mypopup.popupHyperlinkDictionary.Values)
            {
                if (hyp.Name == hyperlink.Name)
                {
                    countIndex++;
                    break;
                }
                countIndex++;
            }

            SetPopupEvents(name, mypopup.hyperlinkDay, countIndex, false);

        }

        List<Hyperlink> GetCurrentHyperlinkList(String hyperlinkName)
        {
            List<Hyperlink> lst = new List<Hyperlink>();
            switch(Int32.Parse(hyperlinkName[1].ToString()))
            {
                case 1:
                    lst = visual.hyperlinkLists.monday;
                    break;
                case 2:
                    lst = visual.hyperlinkLists.tuesday;
                    break;
                case 3:
                    lst = visual.hyperlinkLists.wednesday;
                    break;
                case 4:
                    lst = visual.hyperlinkLists.thirsday;
                    break;
                case 5:
                    lst = visual.hyperlinkLists.friday;
                    break;
                case 6:
                    lst = visual.hyperlinkLists.saturday;
                    break;
                case 7:
                    lst = visual.hyperlinkLists.sunday;
                    break;
            }
            return lst;
        }

        Int32 GetHyperlinkIndex(ref Hyperlink hyperlink, Int32 listIndex)
        {
            List<Hyperlink> lst = GetCurrentHyperlinkList(hyperlink.Name);

            Int32 index = 0;
            foreach (Hyperlink hyp in lst)
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

        void MonthControl_Popup_GetMouseLeaveEvent(object sender, MonthControlMouseLeave_SendToPopup e)
        {
            if (control.popup.IsOpen && !control.popup.IsMouseCaptured)
            {
                control.popup.IsOpen = false;
                //monthControlPopup.SendPopupIsOpenEvent(false);
            }
        }

        void control_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            control.popup.IsOpen = false;
        }

        void ButtonPopupRemove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            sendEventToDelete.Sendinfo(mypopup.currentEventMainInfo, mypopup.currentEventInfo);
            Thread.Sleep(25);
            sendEventAboutChanges.SendEvent(true);
            control.popup.IsOpen = false;
        }

        void ButtonPopupChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            sendEventToChangeIt.SendEvent(mypopup.currentEventMainInfo, mypopup.currentEventInfo);
            control.popup.IsOpen = false;
        }

        void popup_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            timer.Stop();
        }

        void SelectTimer()
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 3);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            control.popup.IsOpen = false;
            control.dockPanelPopup.Children.Clear();
            timer.Stop();
        }

        void SendToWeek_MondayDate_Event_GetEvent(object sender, SendToWeek_MondayDate e)
        {
            visual.variables.selectedDate = e.monday;
            Int32[] arr = new Int32[7];
            arr[0] = e.monday.Day;
            arr[1] = e.monday.AddDays(1).Day;
            arr[2] = e.monday.AddDays(2).Day;
            arr[3] = e.monday.AddDays(3).Day;
            arr[4] = e.monday.AddDays(4).Day;
            arr[5] = e.monday.AddDays(5).Day;
            arr[6] = e.monday.AddDays(6).Day;
            visual.SelectTopWeekdayNames(arr);
        }

        void control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (control.IsMouseCaptured)
            {
                control.ReleaseMouseCapture();
                GetEventTimeToSend();
            }
        }

        void control_MouseMove(object sender, MouseEventArgs e)
        {
            if(control.IsMouseCaptured)
            {
                foreach (Grid grid in GetCurrentList())
                    grid.Background = Brushes.White;

                weekMouseMoveEvent.SendEvent(true);
                Int32 endIndex = GetEndIndex();
                
                for (Int32 i = 0; i < 48; ++i)
                {
                    if(endIndex > visual.variables.selectedIndex)
                    {
                        if (i <= endIndex && i >= visual.variables.selectedIndex)
                            GetCurrentList()[i].Background = new SolidColorBrush(Color.FromRgb(128, 203, 196));
                    }
                    else if(endIndex == visual.variables.selectedIndex && endIndex == i)
                        GetCurrentList()[i].Background = new SolidColorBrush(Color.FromRgb(128, 203, 196));
                    else
                    {
                        if (i >= endIndex && i <= visual.variables.selectedIndex)
                            GetCurrentList()[i].Background = new SolidColorBrush(Color.FromRgb(128, 203, 196));
                    }
                }
            }
        }

        Int32 GetEndIndex()
        {
            Int32 index = 0;
            Double Y = Mouse.GetPosition(control).Y - 40;
            if (Y >= 0 && Y <= control.ActualHeight - 40)
            {
                Double reminder = Y % 25;
                index = Int32.Parse(((Y - reminder) / 25).ToString()); 
            }
            else if(Y > control.ActualHeight - 40)
            {
                index = 47;
            }
            return index;
        }

        void GetEventTimeToSend()
        {
            Int32 hourFrom = 0, hourTo = 0, minFrom = 0, minTo = 0;
            Boolean key = false;
            Int32 count = 0;
            foreach(Grid grid in GetCurrentList())
            {
                if(grid.Background != Brushes.White)
                {
                    if(!key)
                    {
                        Int32 reminder = count % 2;
                        if (reminder == 0)
                            minFrom = 0;
                        else
                            minFrom = 30;
                        
                        hourFrom = count / 2;
                        key = true;
                    }
                    if(count % 2 == 0)
                    {
                        hourTo = count / 2;
                        minTo = 30;
                    }
                    else
                    {
                        hourTo = count / 2 + 1;
                        minTo = 0;
                    }
                }
                    count++;
            }
            DateTime tmp = visual.variables.selectedDate.AddDays(visual.variables.columnNum - 1);
            dates.from = new DateTime(tmp.Year,tmp.Month,tmp.Day,hourFrom,minFrom,0);

            dates.to = new DateTime(tmp.Year, tmp.Month, tmp.Day, hourTo, minTo, 0);

            sendDatesToAddMenu.SendEvent(dates.from,dates.to);
        }

        List<Grid> GetCurrentList()
        {
            if (visual.variables.columnNum == 2)
                return visual.variables.tuesdayList;
            else if (visual.variables.columnNum == 3)
                return visual.variables.wednesdayList;
            else if (visual.variables.columnNum == 4)
                return visual.variables.thirsdayList;
            else if (visual.variables.columnNum == 5)
                return visual.variables.fridayList;
            else if (visual.variables.columnNum == 6)
                return visual.variables.saturdayList;
            else if (visual.variables.columnNum == 7)
                return visual.variables.sundayList;

            return visual.variables.mondayList;
        }

        void grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            visual.variables.columnNum = Int32.Parse(grid.Name[1].ToString());
            
            foreach (Grid gr in GetCurrentList())
                gr.Background = Brushes.White;

            grid.Background = new SolidColorBrush(Color.FromRgb(128, 203, 196));

            visual.variables.gridName = grid.Name;
            visual.variables.selectedIndex = GetIndex(ref grid);
            control.CaptureMouse();
        }

        Int32 GetIndex(ref Grid grid)
        {
            Double height = Mouse.GetPosition(control).Y - 40;
            Double discrepancy = height % 25;
            Int32 index = 0;
            if (discrepancy > 0)
                index = Int32.Parse(((height - discrepancy) / 25).ToString());
            else
                index = Int32.Parse(((height - discrepancy) / 25).ToString());

            return index;
        }
    }
}
