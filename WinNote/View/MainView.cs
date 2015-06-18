using EventLibrsries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WinNote.Control;
using WinNote.Control.SelectData;
using WinNote.Model.Entity.CheckNewNotes;

namespace WinNote.View
{
    public class MainView
    {
        CleanEvent_Event cleanEvent;
        public String currentCalendarMenu = "week";
        String currentMenu = "calendar";
        String tmpNameMenu = "";
        public static MainView mainView;
        MainWindow window;
        ControlCS control;
        View view;
        Boolean MouseButtonDown = false;
        SelectDataToControls selectDataToControls;
        AddEventMenu_Event heightChanged_Event;
        ResetClass resetClass;
        GetCurrentCalendarPanel_Event currentpanelName;
        GetCalendarCurrentPanelName SendCurrentPanelName;
        public enum CalendarPanelActive { Day, Week, Month, Timetable };
        public CalendarPanelActive calendarActivePanel;
        SendEventToControl_Event sendEventToControl;
        SendEventAboutChanges_Event sendEventAboutChanges;
        SendToWeek_MondayDate_Event sendToCalendarEvent;
        EventToGetSelectedDate_Event eventToGetMonthDate;
        MonthControl_Popup monthControlPopup;
        Boolean monthControlMouseLeaveKey = false;
        CheckNewNotes newNotes;
    //TODO: SendEventToControl_Event - класс для передачи события в контролы информации для обновления данных в контролах - нужно в (timetable)
//TODO: при очистке (выходе польвователя) переставить с Блокнота или с Кошелька на Календарь
        public MainView(MainWindow mw)
        {
            
            mainView = this;
            window = mw;
            control = ControlCS.GetInstance;
            view = View.view;
            selectDataToControls = SelectDataToControls.GetIndtance;
            heightChanged_Event = new AddEventMenu_Event();
            resetClass = new ResetClass();
            SendCurrentPanelName = new GetCalendarCurrentPanelName();
            calendarActivePanel = CalendarPanelActive.Week;
            monthControlPopup = new MonthControl_Popup();
            cleanEvent = new CleanEvent_Event();
            
        }

        public void Events()
        {
            window.SizeChanged += window_SizeChanged;
            ChangeMenuEvent.changeMenuEvent += ChangeMenuEvent_changeMenuEvent;
            TopMenuButtonEvents.buttonEvent += TopMenuButtonEvents_buttonEvent;
            window.Hyperlink_LogOutUser.Click += Hyperlink_LogOutUser_Click;
            window.Grid_forOpacity.MouseLeftButtonDown += Grid_forOpacity_MouseLeftButtonDown;
            window.calendarMenu.events.abortButtonClickEvent += events_abortButtonClickEvent;
            mainView.SetFocus_CalendarTopPanelButtons("Week");
            resetClass.ResetEvent(true, window.Height);
            HideCalendarEventAddMenu_Event.hideMenuEvent += HideCalendarEventAddMenu_Event_hideMenuEvent;
            currentpanelName = new GetCurrentCalendarPanel_Event();
            GetCurrentCalendarPanel_Event.GetNameEvent += GetCurrentCalendarPanel_Event_SendEvent;
            sendEventToControl = new SendEventToControl_Event();
            SendEventAboutChanges_Event.GetEvent += SendEventAboutChanges_Event_GetEvent;
            sendEventAboutChanges = new SendEventAboutChanges_Event();
            WeekMouseMove_afterClick_Event.GetEvent += WeekMouseMove_afterClick_Event_GetEvent;
            SendDatesTo_AddEventMenu_Event.GetEvent += SendDatesTo_AddEventMenu_Event_GetEvent;
            sendToCalendarEvent = new SendToWeek_MondayDate_Event();
            sendEventAboutChanges.SendEvent(true);
            eventToGetMonthDate = new EventToGetSelectedDate_Event();
            window.MonthControl.MouseLeave += MonthControl_MouseLeave;
            MonthControl_Popup.GetPopupIsOpenEvent += MonthControl_Popup_GetPopupIsOpenEvent;
            SendEventWidthIeventInfoToAddEventMenu_Event.GetEvent += SendEventWidthIeventInfoToAddEventMenu_Event_GetEvent;
            DayUserControl_SendMoveEventAfterClick_Event.GetEvent += DayUserControl_SendMoveEventAfterClick_Event_GetEvent;

            newNotes = new CheckNewNotes();
            newNotes.FindCalendarEvents();
        }

        void DayUserControl_SendMoveEventAfterClick_Event_GetEvent(object sender, DayUserControl_SendMoveEventAfterClick e)
        {
            Double Y = Mouse.GetPosition(window).Y;
            Double top = 90, bottom = window.Height - 10;
            Double scrollOffset = window.ScrollViewer_Day.VerticalOffset;

            if (Y >= bottom)
            {
                window.ScrollViewer_Day.ScrollToVerticalOffset(scrollOffset + 10);
                Thread.Sleep(50);
            }
            else if (Y <= top)
            {
                window.ScrollViewer_Day.ScrollToVerticalOffset(scrollOffset - 10);
                Thread.Sleep(50);
            }
        }

        void MonthControl_Popup_GetPopupIsOpenEvent(object sender, MonthControlMouseLeave_PopupIsOpen e)
        {
            monthControlMouseLeaveKey = e.key;
        }

        void MonthControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if(monthControlMouseLeaveKey)
            {
                monthControlPopup.SendMouseLeaveEventFromMainPage(true);
            }
        }

        void SendEventWidthIeventInfoToAddEventMenu_Event_GetEvent(object sender, SendEventWidthIeventInfoToAddEventMenu e)
        {
            if (window.calendarMenu.Visibility == Visibility.Hidden)
                ShowHideCalendarMenu();
        }

        void SendDatesTo_AddEventMenu_Event_GetEvent(object sender, SendDatesTo_AddEventMenu e)
        {
            ShowClaendarAddEventMenu(e.from, e.to);
        }

        void WeekMouseMove_afterClick_Event_GetEvent(object sender, WeekMouseMove_afterClick e)
        {
            Double Y = Mouse.GetPosition(window).Y;
            Double top = 90, bottom = window.Height - 10;
            Double scrollOffset = window.scrollViewer_weekControl.VerticalOffset;

            if(Y >= bottom)
            {
                window.scrollViewer_weekControl.ScrollToVerticalOffset(scrollOffset + 10);
                Thread.Sleep(50);
            }
            else if(Y <= top)
            {
                window.scrollViewer_weekControl.ScrollToVerticalOffset(scrollOffset - 10);
                Thread.Sleep(50);
            }
        }

        void GetCurrentCalendarPanel_Event_SendEvent(object sender, GetCurrentCalendarPanel e)
        {
            if (calendarActivePanel == CalendarPanelActive.Day)
                SendCurrentPanelName.SetEvent("Day");
            else if (calendarActivePanel == CalendarPanelActive.Week)
                SendCurrentPanelName.SetEvent("Week");
            else if (calendarActivePanel == CalendarPanelActive.Month)
                SendCurrentPanelName.SetEvent("Month");
            else if (calendarActivePanel == CalendarPanelActive.Timetable)
                SendCurrentPanelName.SetEvent("Timetable");
        }

        void HideCalendarEventAddMenu_Event_hideMenuEvent(object sender, HideCalendarAddEventMenu e)
        {
            ShowHideCalendarMenu();
            //window.Control_day.events.ReloadData();
        }

        void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            heightChanged_Event.ChangeMainWindowEvent(window.Height);
        }

        public void SetDayDateTime(DateTime date)
        {
            //window.Control_day.events.SetDate(date);
        }

        void events_abortButtonClickEvent(object sender, CalendarAddMenu_UserControl.Event.AbortButtonClickEvent e)
        {
            ShowHideCalendarMenu();
            //window.Control_day.events.ReloadData();
        }

        void Grid_forOpacity_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowHideCalendarMenu();
            //window.Control_day.events.ReloadData();
            Thread.Sleep(30);
            sendEventAboutChanges.SendEvent(true);
        }

        public void ShowClaendarAddEventMenu(DateTime dateFrom, DateTime dateTo)
        {
            if (window.calendarMenu.Visibility == Visibility.Hidden)
                ShowHideCalendarMenu();

            window.calendarMenu.events.SelectDates(dateFrom, dateTo);
        }

        //void events_SetTimeEvent(object sender, DayUserControl.MouseLeftButtonUp_FromUserControl e)
        //{
        //    ShowClaendarAddEventMenu(e.From, e.To);
        //}

        void ShowHideCalendarMenu()
        {
            TranslateTransform trans = new TranslateTransform();
            DoubleAnimation animation;
            DoubleAnimation animationOpacity;
            window.calendarMenu.RenderTransform = trans;
            if (window.calendarMenu.Visibility == Visibility.Hidden)
            {
                window.calendarMenu.Visibility = Visibility.Visible;
                animation = new DoubleAnimation(0, -400, TimeSpan.FromMilliseconds(250));
                window.Grid_forOpacity.Visibility = Visibility.Visible;
                animationOpacity = new DoubleAnimation(0.0, 0.3, TimeSpan.FromMilliseconds(250));
                window.Grid_forOpacity.BeginAnimation(Grid.OpacityProperty, animationOpacity);
            }
            else
            {
                animation = new DoubleAnimation(-400, 0, TimeSpan.FromMilliseconds(250));
                animation.Completed += HideMenuAnimationComplited;
                //window.Grid_forOpacity.Visibility = Visibility.Hidden;
                animationOpacity = new DoubleAnimation(0.3, 0.0, TimeSpan.FromMilliseconds(250));
                animationOpacity.Completed += delegate(object sender, EventArgs e)
                {
                    window.Grid_forOpacity.Visibility = Visibility.Hidden;
                };
                window.Grid_forOpacity.BeginAnimation(Grid.OpacityProperty, animationOpacity);
            }
            trans.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        void HideMenuAnimationComplited(object sender, EventArgs e)
        {
            window.calendarMenu.Visibility = Visibility.Hidden;
        }

        public void SetFocus_CalendarTopPanelButtons(String buttonName)
        {          
            window.UserControl_Top_calendar.events.SetButtonFocus(buttonName);            
        }

        void window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(MouseButtonDown)
                MouseButtonDown = false;
        }

        void window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(MouseButtonDown)
            {
                //window.Scroll_Day.events.MouseMoveEvent(Mouse.GetPosition(window));
            }
        }
//TODO: Выход пользователя: полная очистка
        void Hyperlink_LogOutUser_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            control.LogOut();
            view.ResizeToLoginMenu();
            if (window.calendarMenu.Visibility == Visibility.Visible)
                ShowHideCalendarMenu();
            cleanEvent.SendEvent(EnumCleanEvent.CleanClasses.All);
            window.Grid_Notepad_RightMenu.Visibility = Visibility.Hidden;
            window.Grid_Calendar_RightMenu.Visibility = Visibility.Visible;
            ResetPanels();
        }

        void ResetPanels()
        {
            window.Grid_Notepad_RightMenu.Visibility = Visibility.Hidden;
            window.Grid_Calendar_RightMenu.Visibility = Visibility.Visible;
            window.Grid_Wallet_RightMenu.Visibility = Visibility.Hidden;
            window.Grid_Calendar_RightMenu.Margin = new Thickness(0);
            window.month.Visibility = Visibility.Hidden;
            window.day.Visibility = Visibility.Hidden;
            if (window.week.Margin.Left != 0)
                ResetPanels();
            Thread.Sleep(200);
        }

        public void SetName_TopPanel(String name)
        {
            window.Label_UserName.Content = name;
        }

        void GetEnumForSendInToUserControl(ref SendEventToControl_Event.ControlNameEnum controlName)
        {
            if (calendarActivePanel == CalendarPanelActive.Day)
                controlName = SendEventToControl_Event.ControlNameEnum.Day;
            else if (calendarActivePanel == CalendarPanelActive.Week)
                controlName = SendEventToControl_Event.ControlNameEnum.Week;
            else if (calendarActivePanel == CalendarPanelActive.Month)
                controlName = SendEventToControl_Event.ControlNameEnum.Month;
            else if (calendarActivePanel == CalendarPanelActive.Timetable)
                controlName = SendEventToControl_Event.ControlNameEnum.Timetable;

        }

      
        /// <summary>
        /// метод для принятия события об изминении или добавлении события (информации)
        /// </summary>
        void SendEventAboutChanges_Event_GetEvent(object sender, SendEventAboutChanges e)
        {
            SendEventToControl_Event.ControlNameEnum controlName = SendEventToControl_Event.ControlNameEnum.Day;
            GetEnumForSendInToUserControl(ref controlName);
            sendEventToControl.SendEvent(controlName);
        }

//TODO: событие при открытии программы (LoadEventInfo_Event - с )
//TODO: MessageBoxes - Calendar Top Panel Buttons Click
        void TopMenuButtonEvents_buttonEvent(object sender, TopMenuButtonClick e)
        {
            //control name - for send in to UserControls
            SendEventToControl_Event.ControlNameEnum controlName = SendEventToControl_Event.ControlNameEnum.Day;
            GetEnumForSendInToUserControl(ref controlName);


            if (e.buttonName == "Button_GoLeft")
            {
                sendEventToControl.SendEvent(controlName);
            }
            else if (e.buttonName == "Button_GoRight")
            {
                sendEventToControl.SendEvent(controlName);
            }
            else if (e.buttonName == "Button_AddEvent")
            {
            
            }
            else if (e.buttonName == "Button_Day")
            {
                AnimateCalendar_ShowMenu("day");
                AnimateCalendar_HideMenu();
                currentCalendarMenu = "day";
                sendEventToControl.SendEvent(SendEventToControl_Event.ControlNameEnum.Day);
            }
            else if (e.buttonName == "Button_Week")
            {
                AnimateCalendar_ShowMenu("week");
                AnimateCalendar_HideMenu();
                currentCalendarMenu = "week";
                //sendEventToControl.SendEvent(SendEventToControl_Event.ControlNameEnum.Week);
                SendToWeek_MondayDate_Event sendToCalendarEvent_ToGetMonday = new SendToWeek_MondayDate_Event();
                sendToCalendarEvent_ToGetMonday.SendEventToSendMonday(true);
                sendToCalendarEvent.SendEventToSendMonday(true);
                sendEventToControl.SendEvent(SendEventToControl_Event.ControlNameEnum.Week);
            }
            else if (e.buttonName == "Button_Month")
            {
                AnimateCalendar_ShowMenu("month");
                AnimateCalendar_HideMenu();
                currentCalendarMenu = "month";
                sendEventToControl.SendEvent(SendEventToControl_Event.ControlNameEnum.Month);
                eventToGetMonthDate.SendEvent_SelectedDate(true);
            }
            else if (e.buttonName == "Button_Timetable")
            {
                AnimateCalendar_ShowMenu("timetable");
                AnimateCalendar_HideMenu();
                currentCalendarMenu = "timetable";
                sendEventToControl.SendEvent(SendEventToControl_Event.ControlNameEnum.Timetable);
            }
            

        }

        public void AnimateCalendar_HideMenu()
        {
            Double width = window.Width - 300;
            DoubleAnimation anim_hide = new DoubleAnimation(0, width, TimeSpan.FromMilliseconds(300));
            TranslateTransform trans = new TranslateTransform();

            if (currentCalendarMenu == "day")
            {
                window.day.RenderTransform = trans;
                window.day.Visibility = System.Windows.Visibility.Visible;
            }
            else if (currentCalendarMenu == "week")
            {
                window.week.RenderTransform = trans;
                window.week.Visibility = System.Windows.Visibility.Visible;
            }
            else if (currentCalendarMenu == "month")
            {
                window.month.RenderTransform = trans;
                window.month.Visibility = System.Windows.Visibility.Visible;
            }
            else if (currentCalendarMenu == "timetable")
            {
                window.timetable.RenderTransform = trans;
                window.timetable.Visibility = System.Windows.Visibility.Visible;
            }
            trans.BeginAnimation(TranslateTransform.XProperty, anim_hide);
        }

        public void AnimateCalendar_ShowMenu(String menu)
        {
            Double width = window.Width - 300;
            DoubleAnimation anim_show = new DoubleAnimation(width,0,TimeSpan.FromMilliseconds(300));
            TranslateTransform trans = new TranslateTransform();

            if (menu == "day")
            {
                window.day.RenderTransform = trans;
                window.day.Visibility = System.Windows.Visibility.Visible;
            }
            else if (menu == "week")
            {
                window.week.RenderTransform = trans;
                window.week.Visibility = System.Windows.Visibility.Visible;
            }
            else if (menu == "month")
            {
                window.month.RenderTransform = trans;
                window.month.Visibility = System.Windows.Visibility.Visible;
            }
            else if (menu == "timetable")
            {
                window.timetable.RenderTransform = trans;
                window.timetable.Visibility = System.Windows.Visibility.Visible;
            }
            trans.BeginAnimation(TranslateTransform.XProperty, anim_show);

        }

        void ChangeMenuEvent_changeMenuEvent(object sender, ChangeMenu e)
        {
            tmpNameMenu = e.message;
            AnimateMenu_show(e.message);
            AnimateMenu_hide();        
        }
        
 //TODO: сделать сброс информации
        
        void AnimateMenu_hide()
        {
            Double height = window.Height - 95;
            DoubleAnimation anim_hide = new DoubleAnimation(0, height, TimeSpan.FromMilliseconds(300));
            anim_hide.Completed += anim_hide_Completed;
            TranslateTransform trans = new TranslateTransform();
            if (currentMenu == "calendar")
                window.Grid_Calendar_RightMenu.RenderTransform = trans;
            else if (currentMenu == "notepad")
                window.Grid_Notepad_RightMenu.RenderTransform = trans;
            else if (currentMenu == "wallet")
                window.Grid_Wallet_RightMenu.RenderTransform = trans;
            
            trans.BeginAnimation(TranslateTransform.YProperty, anim_hide);

        }

        void anim_hide_Completed(object sender, EventArgs e)
        {
            if (currentMenu == "calendar")
            {
                window.Grid_Calendar_RightMenu.Visibility = System.Windows.Visibility.Hidden;
                window.UserControl_Top_calendar.Visibility = Visibility.Hidden;
            }
            else if (currentMenu == "notepad")
            {
                window.Grid_Notepad_RightMenu.Visibility = System.Windows.Visibility.Hidden;
                window.UserControl_Top_notepad.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (currentMenu == "wallet")
            {
                window.Grid_Wallet_RightMenu.Visibility = System.Windows.Visibility.Hidden;
                window.UserControl_Top_wallet.Visibility = System.Windows.Visibility.Hidden;
            }
            currentMenu = tmpNameMenu;

            if (currentMenu == "calendar")
                window.UserControl_Top_calendar.Visibility = Visibility.Visible;
            else if (currentMenu == "notepad")
                window.UserControl_Top_notepad.Visibility = System.Windows.Visibility.Visible;
            else if (currentMenu == "wallet")
                window.UserControl_Top_wallet.Visibility = System.Windows.Visibility.Visible;

        }

        void AnimateMenu_show(String menu)
        {
            Double height = window.Height - 95;
            DoubleAnimation anim_show = new DoubleAnimation(height, 0, TimeSpan.FromMilliseconds(300));
            TranslateTransform trans = new TranslateTransform();
            //AnimationTopMenuShow(menu);
            //AnimateTopMenuHide();
            if (menu == "calendar")
            {
                window.Grid_Calendar_RightMenu.RenderTransform = trans;
                window.Grid_Calendar_RightMenu.Visibility = System.Windows.Visibility.Visible;
            }
            else if (menu == "notepad")
            {
                window.Grid_Notepad_RightMenu.RenderTransform = trans;
                window.Grid_Notepad_RightMenu.Visibility = System.Windows.Visibility.Visible;
            }
            else if (menu == "wallet")
            {
                window.Grid_Wallet_RightMenu.RenderTransform = trans;
                window.Grid_Wallet_RightMenu.Visibility = System.Windows.Visibility.Visible;
            }
            trans.BeginAnimation(TranslateTransform.YProperty, anim_show);
        }

        
    }
}
