using CalendarAddMenu_UserControl.Event;
using CalendarAddMenu_UserControl.FillControll;
using CalendarAddMenu_UserControl.Model;
using CalendarAddMenu_UserControl.OrganiseInfo;
using EventLibrsries;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CalendarAddMenu_UserControl
{
    public class Events
    {
        CalendarAddMenu_UserControl control;
        DateTime dateFrom { get; set; }
        DateTime dateTo { get; set; }
        Calendar_AddEvent calendarAddEvent;
        static Double windowHeight;
        AnimationEvents animateClass;
        TextEvents textClass;
        CheckOnCorrectEntrance checkCorrectEntrance;
        CheckCorrectEntrance checkInput;
        Info info;
        SendEventAboutChanges_Event sendEventAboutChanges;
        public VarEventInfo eventInfo;
        FillControllWithData fillControllWithData;
        CheckForChanges checkChanges;
        
        public Events(CalendarAddMenu_UserControl control)
        {
            this.control = control;
            calendarAddEvent = new Calendar_AddEvent();
            animateClass = AnimationEvents.GetInstance(control);
            textClass = TextEvents.GetInstance(control);
            checkCorrectEntrance = new CheckOnCorrectEntrance();
            checkInput = new CheckCorrectEntrance();
            info = new Info(control);
            sendEventAboutChanges = new SendEventAboutChanges_Event();
            eventInfo = VarEventInfo.GetInstance;
            fillControllWithData = new FillControllWithData(control);
            checkChanges = new CheckForChanges(control);
        }

        public void SetEvents()
        {
            animateClass.SetAnimateEvents();
            textClass.SetTextEvents();

            control.scroll.ValueChanged += scroll_ValueChanged;
            control.SizeChanged += control_SizeChanged;
            control.Button_Abort.Click += Button_Abort_Click;

            control.Label_AllDay.MouseLeftButtonDown += Label_MouseLeftButtonDown;
            control.Label_RepeatEvent.MouseLeftButtonDown += Label_MouseLeftButtonDown;
            control.Label_Reminder.MouseLeftButtonDown += Label_MouseLeftButtonDown;

            control.Button_Save.Click += Button_Save_Click;

            control.SizeChanged+=control_SizeChanged;
            AddEventMenu_Event.changeHeightEvent += AddEventMenu_Event_changeHeightEvent;
            control.ComboBox_enumRepeatEvent.PreviewMouseWheel += ComboBox_PreviewMouseWheel;
            control.ComboBox_enumRemindEvent.PreviewMouseWheel += ComboBox_PreviewMouseWheel;
            control.MouseWheel += control_MouseWheel;

            ResetClass.resetEvent += ResetClass_resetEvent;
            checkCorrectEntrance.CheckEvent(ref control);
            SendEventWidthIeventInfoToAddEventMenu_Event.GetEvent += SendEventWidthIeventInfoToAddEventMenu_Event_GetEvent;
            CleanEvent_Event.GetEvent += CleanEvent_Event_GetEvent;
        }

        void CleanEvent_Event_GetEvent(object sender, CleanEvent e)
        {
            VarEventInfo.GetInstance.eventInfo = null;
            VarEventInfo.GetInstance.eventMainInfo = null;

        }

        void SendEventWidthIeventInfoToAddEventMenu_Event_GetEvent(object sender, SendEventWidthIeventInfoToAddEventMenu e)
        {
            control.Button_Save.Content = "Изменить";
            eventInfo.eventMainInfo = e.eventMainInfo;
            eventInfo.eventInfo = e.eventInfo;
            fillControllWithData.FillData();
        }

        void ResetClass_resetEvent(object sender, ResetClassEvent e)
        {
            if(e.reset)
                ResetMenu();
            windowHeight = e.height;
        }
      
        void control_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if(control.scroll.Visibility == Visibility.Visible)
            {
                if(e.Delta == 120)
                {
                    if (control.scroll.Value >= 20)
                        control.scroll.Value -= 20;
                }
                else if(e.Delta == -120)
                {
                    if (control.scroll.Value <= 80)
                        control.scroll.Value += 20;
                }
            }
        }

        void ComboBox_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        void AddEventMenu_Event_changeHeightEvent(object sender, AddEventmenu e)
        {
            windowHeight = e.height - 50;
            if(control.Height > windowHeight)
            {
                control.scroll.Visibility = Visibility.Visible;
            }
            else
            {
                if (control.scroll.Visibility == Visibility.Visible)
                    control.scroll.Visibility = Visibility.Hidden;
                if (control.Grid_Control.Margin.Top > 0 || control.Grid_Control.Margin.Top < 0)
                {
                    animateClass.AnimationThickness(control.Grid_Control.Margin, new Thickness(0, 0, 0, 0), ref control.Grid_Control);
                    control.scroll.Value = 0;
                }
            }
        }     

//TODO: Введение даты (С меньше-равно До)
        void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            Button saveButton = sender as Button;
            if (saveButton.Content.ToString() == "Сохранить")
            {
                if (checkInput.CheckAllEntrance())
                {
                    info.GetAndSend();
                }
                ResetMenu();
                Thread.Sleep(30);
                sendEventAboutChanges.SendEvent(true);
            }
            else if(saveButton.Content.ToString() == "Изменить")
            {
                if (checkInput.CheckAllEntrance() && checkChanges.CheckInfo())
                {
                    info.GetAndChange();
                }
                else
                {
                    info.HideMenu();
                }

                ResetMenu();
                Thread.Sleep(30);
                sendEventAboutChanges.SendEvent(true);
            }
        }

        IEventInfo GetInfo()
        {
            IEventInfo info = new EventInfo();

            return info;
        }

        IEventMainInfo GetMainInfo()
        {
            IEventMainInfo mainInfo = new EventMainInfo();
            String eventName = control.TextBox_eventName.Text;

            if (eventName == "Название события" || eventName == "")
                eventName = "Без названия";

            return mainInfo;
        }

        void Label_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Label label = (Label)sender;
            if(label.Name == "Label_AllDay")
            {
                if (control.CheckBox_AllDay.IsChecked.Value == true)
                    control.CheckBox_AllDay.IsChecked = false;
                else
                    control.CheckBox_AllDay.IsChecked = true;
            }
            else if(label.Name == "Label_RepeatEvent")
            {
                if(control.CheckBox_RepeatEvent.IsChecked == true)
                    control.CheckBox_RepeatEvent.IsChecked = false;
                else
                    control.CheckBox_RepeatEvent.IsChecked = true;
            }
            else if(label.Name == "Label_Reminder")
            {
                if (control.CheckBox_Reminder.IsChecked == true)
                    control.CheckBox_Reminder.IsChecked = false;
                else
                    control.CheckBox_Reminder.IsChecked = true;
            }

        }
       
        public void SelectDates(DateTime dateFrom, DateTime dateTo)
        {
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
            control.datePicker_DateFrom.SelectedDate = dateFrom;
            control.datePicker_DateTo.SelectedDate = dateTo;
            
            if(dateTo.Minute == 0)
                control.TextBox_timeTo.Text = dateTo.Hour + ":" + dateTo.Minute + "0";
            else
                control.TextBox_timeTo.Text = dateTo.Hour + ":" + dateTo.Minute;
            if (dateFrom.Minute == 0)
                control.TextBox_timeFrom.Text = dateFrom.Hour + ":" + dateFrom.Minute + "0";
            else
                control.TextBox_timeFrom.Text = dateFrom.Hour + ":" + dateFrom.Minute;
        }

        void Button_Abort_Click(object sender, RoutedEventArgs e)
        {
            AbortEvent(true);
            ResetMenu();
            Thread.Sleep(30);
            sendEventAboutChanges.SendEvent(true);
        }

        public void ResetMenu()
        {
            control.TextBox_Description.Text = "Описание";
            control.TextBox_Description.Foreground = Brushes.LightGray;
            control.TextBox_eventName.Text = "Название события";
            control.TextBox_eventName.Foreground = Brushes.LightGray;
            control.TextBox_Locate.Text = "Место проведения";
            control.TextBox_Locate.Foreground = Brushes.LightGray;
            control.TextBox_timeFrom.Text = "00:00";
            control.TextBox_timeTo.Text = "24:00";
            control.datePicker_DateFrom.SelectedDate = DateTime.Now;
            control.datePicker_DateTo.SelectedDate = DateTime.Now;

            control.ComboBox_enumRemindEvent.SelectedIndex = -1;
            control.ComboBox_enumRemindEvent.Text = "";
            control.ComboBox_enumRepeatEvent.SelectedIndex = -1;
            control.ComboBox_enumRepeatEvent.Text = "";

            control.CheckBox_AllDay.IsChecked = false;
            if (control.Button_Save.Content.ToString() == "Изменить")
                control.Button_Save.Content = "Сохранить";
        }

        void control_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (control.Height > windowHeight && windowHeight > 0)
                control.scroll.Visibility = Visibility.Visible;
            else
                control.scroll.Visibility = Visibility.Hidden;
        }

        void scroll_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            ScrollBar scroll = (ScrollBar)sender;
            Double oldValue = e.OldValue;
            Double newValue = e.NewValue;
            if (e.OldValue < e.NewValue)
            {
                if (e.NewValue > 0 && e.NewValue < 20)
                {
                    scroll.Value = 20;
                    newValue = 20;
                }
                else if (e.NewValue > 20 && e.NewValue < 40)
                {
                    scroll.Value = 40;
                    newValue = 40;
                }
                else if (e.NewValue > 40 && e.NewValue < 60)
                {
                    scroll.Value = 60;
                    newValue = 60;
                }
                else if (e.NewValue > 60 && e.NewValue < 80)
                {
                    scroll.Value = 80;
                    newValue = 80;
                }
                else if (e.NewValue > 80 && e.NewValue < 100)
                {
                    scroll.Value = 100;
                    newValue = 100;
                }
            }
            else if (e.OldValue > e.NewValue)
            {
                if (e.NewValue > 0 && e.NewValue < 20)
                {
                    scroll.Value = 0;
                    newValue = 0;
                }
                else if (e.NewValue > 20 && e.NewValue < 40)
                {
                    scroll.Value = 20;
                    newValue = 20;
                }
                else if (e.NewValue > 40 && e.NewValue < 60)
                {
                    scroll.Value = 40;
                    newValue = 40;
                }
                else if (e.NewValue > 60 && e.NewValue < 80)
                {
                    scroll.Value = 60;
                    newValue = 60;
                }
                else if (e.NewValue > 80 && e.NewValue < 100)
                {
                    scroll.Value = 80;
                    newValue = 80;
                }
            }
            if(e.NewValue % 20 != 0)
            {
                Double value = 0;
                value = control.scroll.Value / 20;
                control.scroll.Value = value * 20;
            }
            if (control.scroll.Visibility == Visibility.Visible)
            {
                Double Value = e.NewValue / 20;
                Double scrollHeight = ((control.Height - windowHeight) / 5) * Value;
                if (Value == 0)
                    animateClass.AnimationThickness(control.Grid_Control.Margin, new Thickness(0, 0, 0, 0), ref control.Grid_Control);
                else
                {
                    if(e.NewValue > e.OldValue)
                        animateClass.AnimationThickness(control.Grid_Control.Margin, new Thickness(0, control.Grid_Control.Margin.Top - scrollHeight, 0, 0), ref control.Grid_Control);
                    else
                        animateClass.AnimationThickness(control.Grid_Control.Margin, new Thickness(0, control.Grid_Control.Margin.Top + scrollHeight, 0, 0), ref control.Grid_Control);
                }
            }
            else
            {
                if(control.scroll.Value != 0)
                {
                    control.scroll.Value = 0;
                    animateClass.AnimationThickness(control.Grid_Control.Margin, new Thickness(0, 40, 0, 0), ref control.Grid_Control);
                }
            }
        }

        public event EventHandler<AbortButtonClickEvent> abortButtonClickEvent;
       
        void AbortEvent(Boolean abortEvent)
        {
            if (abortButtonClickEvent != null)
                abortButtonClickEvent(this, new AbortButtonClickEvent(true));
        }

    }

    

   
}
