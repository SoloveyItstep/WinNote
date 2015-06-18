using EventLibrsries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Calendar_topMenu_UserControl
{
    public class Events
    {
        Calendar_topMenu_UserControl control;
        public static String currentButton = "Button_Week";
        TopMenuButtonEvents buttonEv;
        
        public Events(Calendar_topMenu_UserControl control)
        {
            this.control = control;
            buttonEv = new TopMenuButtonEvents();
            ButtonEventNames.buttonEv += ButtonEventNames_buttonEv;
        }

        void ButtonEventNames_buttonEv(object sender, ButtonEvent e)
        {
            if(e.buttonName == "day")
            {
                control.Button_Day.RaiseEvent(new System.Windows.RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        public void SetEvents()
        {
            control.Button_AddEvent.Click += Button_AddEvent_Click;
            control.Button_GoLeft.Click += Button_Go_Click;
            control.Button_GoRight.Click += Button_Go_Click;

            control.Button_Day.Click += Button_Click;
            control.Button_Month.Click += Button_Click;
            control.Button_Timetable.Click += Button_Click;
            control.Button_Week.Click += Button_Click;

            control.Button_Day.MouseLeave += Button_MouseLeave;
            control.Button_Month.MouseLeave += Button_MouseLeave;
            control.Button_Timetable.MouseLeave += Button_MouseLeave;
            control.Button_Week.MouseLeave += Button_MouseLeave;

            control.Button_GoLeft.MouseEnter += Button_Go_MouseEnter;
            control.Button_GoRight.MouseEnter += Button_Go_MouseEnter;
            control.Button_GoLeft.MouseLeave += Button_Go_MouseLeave;
            control.Button_GoRight.MouseLeave += Button_Go_MouseLeave;

            CleanEvent_Event.GetEvent += CleanEvent_Event_GetEvent;
        }

        void CleanEvent_Event_GetEvent(object sender, CleanEvent e)
        {
            control.Button_Week.RaiseEvent(new System.Windows.RoutedEventArgs(ButtonBase.ClickEvent));
        }

        public void SetButtonFocus(String ButtonName)
        {
            if(ButtonName == "Day")
            {
                control.Button_Day.Focus();
            }
        }

        void Button_Go_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Background = null;
        }

        void Button_Go_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Background = new SolidColorBrush(Color.FromRgb(79, 195, 247));
        }

        void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button button = sender as Button;
            if (button.Name == currentButton)
            {
                button.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 203, 196));
                button.Foreground = System.Windows.Media.Brushes.White;
            }
        }

        void Button_Go_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button button = sender as Button;
            buttonEv.ButtonClickEvent(button.Name);
        }

        void Button_AddEvent_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button button = sender as Button;
            buttonEv.ButtonClickEvent(button.Name);
        }

        void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Name == currentButton)
                return;
            buttonEv.ButtonClickEvent(button.Name);
            if (currentButton == "Button_Day")
            {
                control.Button_Day.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(227, 242, 253));
                control.Button_Day.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 136, 229));
            }
            else if (currentButton == "Button_Month")
            {
                control.Button_Month.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(227, 242, 253));
                control.Button_Month.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 136, 229));
            }
            else if (currentButton == "Button_Timetable")
            {
                control.Button_Timetable.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(227, 242, 253));
                control.Button_Timetable.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 136, 229));
            }
            else if (currentButton == "Button_Week")
            {
                control.Button_Week.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(227, 242, 253));
                control.Button_Week.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 136, 229));
            }
            button.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 203, 196));
            button.Foreground = System.Windows.Media.Brushes.White;
            currentButton = button.Name;
        }

        public void MainStyle()
        {
            control.Button_AddEvent.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(227, 242, 253));
            control.Button_Day.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(227, 242, 253));
            control.Button_Month.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(227, 242, 253));
            control.Button_Timetable.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(227, 242, 253));
            control.Button_Week.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 203, 196));
            control.Button_Week.Foreground = System.Windows.Media.Brushes.White;
            control.Button_Day.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 136, 229));
            control.Button_Month.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 136, 229));
            control.Button_Timetable.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 136, 229));
        }
    }
}
