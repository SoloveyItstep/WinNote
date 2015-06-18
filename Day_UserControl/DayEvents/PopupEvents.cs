using Day_UserControl.Model;
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

namespace Day_UserControl.DayEvents
{
    public class PopupEvents
    {
        DayUserControl control;
        Variables variables;
        System.Windows.Threading.DispatcherTimer timer;
        MyPopup mypopup;
        SendEventToDeleteEventInfo_Event sendEventToDelete;
        SendEventAboutChanges_Event sendEventAboutChanges;
        SendEventWidthIeventInfoToAddEventMenu_Event sendEventToChangeIt;
        MonthControl_Popup monthControlPopup;

        public PopupEvents(DayUserControl control)
        {
            this.control = control;
            variables = Variables.GetInstance;
            mypopup = MyPopup.GetInstance;
            sendEventToDelete = new SendEventToDeleteEventInfo_Event();
            sendEventAboutChanges = new SendEventAboutChanges_Event();
            sendEventToChangeIt = new SendEventWidthIeventInfoToAddEventMenu_Event();
            monthControlPopup = new MonthControl_Popup();
        }

        public void SetEvents()
        {
            control.popup.MouseEnter += popup_MouseEnter;
            control.popup.MouseLeave += popup_MouseLeave;
            control.MouseDown += control_MouseDown;
            MonthControl_Popup.GetMouseLeaveEvent += DayControl_Popup_GetMouseLeaveEvent;
            control.ButtonPopupChange.Click += ButtonPopupChange_Click;
            control.ButtonPopupRemove.Click += ButtonPopupRemove_Click;

            foreach(Hyperlink hyperlink in variables.hyperlinkList)
                hyperlink.MouseEnter += hyperlink_MouseEnter;
            SelectTimer();
            SetMyPopups();
        }

        void SetMyPopups()
        {
            mypopup.hyperlinkIndex = new Dictionary<int, int>();
            mypopup.popupHyperlinkDictionary = new Dictionary<int, Hyperlink>();
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
            Int32 hyperlinkIndex = Int32.Parse(hyperlink.Name.Substring(1));
            if(hyperlink.Inlines.Count > 0)
                SetPopupEvents((hyperlink.Inlines.FirstOrDefault() as Run).Text, hyperlinkIndex, true);
            else
                control.popup.IsOpen = false;
        }

        void SetPopupEvents(String name, Int32 index, Boolean key)
        {
            control.dockPanelPopup.Children.Clear();
            Int32 dayCount = 0, count = 0, hyperlinkCount = 0;
            if (key)
            {
                mypopup.popupHyperlinkDictionary.Clear();
                mypopup.hyperlinkDay = 0;
                mypopup.hyperlinkIndex.Clear();
            }

            foreach(var v in variables.eventInfoDictionary)
            {
                if(v.Key.EventName == name)
                {
                    if(control.GridButtons.Visibility == System.Windows.Visibility.Hidden)
                        control.GridButtons.Visibility = System.Windows.Visibility.Visible;

                    mypopup.currentEventMainInfo = null;
                    mypopup.currentEventInfo = null;

                    String str = "";
                    str += String.Format("{0}\n\nДата с: {1}\nДата до:{2}\n",v.Key.EventName,v.Key.dateFrom,v.Key.dateTo);
                    str += String.Format("Описание: {0}\n\nМесто: {1}",v.Value.description,v.Value.location);
                    TextBlock txt = new TextBlock();
                    txt.TextWrapping = System.Windows.TextWrapping.Wrap;
                    txt.Text = str;
                    txt.Margin = new System.Windows.Thickness(10,10,10,10);

                    mypopup.currentEventMainInfo = v.Key;
                    mypopup.currentEventInfo = v.Value;

                    control.dockPanelPopup.Children.Add(txt);
                }
                if (name.Length > 8 && name.Substring(name.Length - 8) == " событий" && count >= 4)
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
                    mypopup.hyperlinkDay = variables.currentDate.Day;
                    
                    hyperlink.PreviewMouseLeftButtonDown += hyperlink_MouseLeftButtonDown;
                    hyperlinkCount++;

                    control.dockPanelPopup.Children.Add(txt);
                }
                ++dayCount;
                if (v.Key.dateFrom.Day == mypopup.hyperlinkDay)
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

            SetPopupEvents(name, countIndex, false);

        }

        private void ButtonPopupRemove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            sendEventToDelete.Sendinfo(mypopup.currentEventMainInfo, mypopup.currentEventInfo);
            Thread.Sleep(25);
            sendEventAboutChanges.SendEvent(true);
            control.popup.IsOpen = false;
            
        }

        private void ButtonPopupChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            sendEventToChangeIt.SendEvent(mypopup.currentEventMainInfo, mypopup.currentEventInfo);
            control.popup.IsOpen = false;
        }

        private void DayControl_Popup_GetMouseLeaveEvent(object sender, MonthControlMouseLeave_SendToPopup e)
        {
            if (control.popup.IsOpen && !control.popup.IsMouseCaptured)
            {
                control.popup.IsOpen = false;
                monthControlPopup.SendPopupIsOpenEvent(false);
            }
        }

        private void control_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            control.popup.IsOpen = false;
        }

        private void popup_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            control.popup.IsOpen = false;
        }

        private void popup_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            timer.Stop();
        }

       
    }
}
