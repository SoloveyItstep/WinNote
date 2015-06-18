using EventLibrsries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using WinNote.Control;
using WinNote.Control.Events;

namespace WinNote.View
{
    class View
    {
        MainWindow window;
        ControlCS control;
        DispatcherTimer timer;
        MainView main;
        MyNotifyIcons notifyIcon;

        public static View view { get; private set; }
        public View(MainWindow window)
        {
            view = this;
            this.window = window;
            control = ControlCS.GetInstance;
            notifyIcon = new MyNotifyIcons(window);
            Events();
            if(control.LogEvents == null)
                control.CreateEvents();
            main = new MainView(window);
            
            main.Events();
            
        }

        public void Events()
        {
            ConnectionEvent.ConnectEvent += ConnectionEvent_ConnectEvent;
            timer = new  DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 4);
            window.button_close.Margin = new Thickness(0, 3, 10, 0);
            window.button_minimize.Margin = new Thickness(0, 3, 40, 0);
            
            window.MaxHeight = SystemParameters.WorkArea.Height;
            window.button_close.Click += button_close_Click;
            window.button_minimize.Click += button_minimize_Click;
            window.button_size.Click += button_size_Click;
            window.Button_GoToLoginMenu.Click += Button_GoToLoginMenu_Click;
            window.Button_GoToRegisterMenu.Click += Button_GoToRegisterMenu_Click;

            notifyIcon.LoadNotifyIcon();
            notifyIcon.SetEvents();

            if (control.Check_LogededInUser())
                ResizeToMainMenu();
            else
                ResizeToLoginMenu();
        }

        public void ResizeToMainMenu()
        {
            if (window.Grid_RegisterMenu.Visibility == Visibility.Visible)
                SetLoginMenu();
            window.Grid_firstPage.Visibility = Visibility.Hidden;
            window.Grid_MainPage.Visibility = Visibility.Visible;
            window.button_size.Visibility = Visibility.Visible;
            window.button_size.Margin = new Thickness(0, 3, 45, 0);
            window.button_minimize.Margin = new Thickness(0, 3, 80, 0);
           
            window.MinHeight = Screen.PrimaryScreen.WorkingArea.Height / 1.5;
            window.MinWidth = Screen.PrimaryScreen.WorkingArea.Width / 1.75;
            window.MaxHeight =  SystemParameters.WorkArea.Height;
            window.WindowState = WindowState.Maximized;
            window.ResizeMode = ResizeMode.CanResize;
            window.Label_UserName.Content = control.GetLogedInUserName();
        }

        public void ResizeToLoginMenu()
        {
            window.Grid_firstPage.Visibility = Visibility.Visible;
            window.Grid_MainPage.Visibility = Visibility.Hidden;
            window.button_size.Visibility = Visibility.Hidden;
            window.button_minimize.Margin = new Thickness(0, 3, 45, 0);
            window.WindowState = WindowState.Normal;
            window.MinHeight = 600;
            window.MinWidth = 400;
            window.Width = 400;
            window.Height = 600;
            
            window.ResizeMode = ResizeMode.NoResize;            
        }

        void timer_Tick(object sender, EventArgs e)
        {
            window.Label_loginMenu.Content = "";
            window.Label_RegisterMenu.Content = "";
            timer.Stop();
        }

        void ConnectionEvent_ConnectEvent(object sender, ConnectionExceptionEvent e)
        {
            window.Label_loginMenu.Content = e.message;
            window.Label_RegisterMenu.Content = e.message;
            timer.Start();
        }

        void button_size_Click(object sender, RoutedEventArgs e)
        {
            if (window.WindowState == WindowState.Maximized)
                window.WindowState = WindowState.Normal;
            else if(window.WindowState == WindowState.Normal)
                window.WindowState = WindowState.Maximized;
        }

        void Button_GoToLoginMenu_Click(object sender, RoutedEventArgs e)
        {
            SetLoginMenu();
        }

        void SetLoginMenu()
        {
            window.Grid_LoginMenu.Dispatcher.BeginInvoke(new Action(() =>
            {
                TranslateTransform trans = new TranslateTransform();
                window.Grid_LoginMenu.RenderTransform = trans;
                DoubleAnimation anim = new DoubleAnimation(-400, 0, TimeSpan.FromMilliseconds(400));
                trans.BeginAnimation(TranslateTransform.XProperty, anim);
            }));
            window.Grid_RegisterMenu.Dispatcher.BeginInvoke(new Action(() =>
            {
                TranslateTransform trans = new TranslateTransform();
                window.Grid_RegisterMenu.RenderTransform = trans;
                DoubleAnimation anim = new DoubleAnimation(-400, 0, TimeSpan.FromMilliseconds(400));
                anim.Completed += anim_Completed;
                trans.BeginAnimation(TranslateTransform.XProperty, anim);
            }));
        }

        void anim_Completed(object sender, EventArgs e)
        {
            window.Grid_RegisterMenu.Visibility = Visibility.Hidden;
        }

        void Button_GoToRegisterMenu_Click(object sender, RoutedEventArgs e)
        {
            window.Grid_LoginMenu.Dispatcher.BeginInvoke(new Action(() =>
            {
                TranslateTransform trans = new TranslateTransform();
                window.Grid_LoginMenu.RenderTransform = trans;
                DoubleAnimation anim = new DoubleAnimation(0, -400, TimeSpan.FromMilliseconds(400));
                trans.BeginAnimation(TranslateTransform.XProperty, anim);
            }));
            if (window.Grid_RegisterMenu.Visibility == Visibility.Hidden)
                window.Grid_RegisterMenu.Visibility = Visibility.Visible;

            window.Grid_RegisterMenu.Dispatcher.BeginInvoke(new Action(() =>
            {
                TranslateTransform trans = new TranslateTransform();
                window.Grid_RegisterMenu.RenderTransform = trans;
                DoubleAnimation anim = new DoubleAnimation(0, -400, TimeSpan.FromMilliseconds(400));
                trans.BeginAnimation(TranslateTransform.XProperty, anim);
            }));
        }

        void button_minimize_Click(object sender, RoutedEventArgs e)
        {
            window.WindowState = WindowState.Minimized;
        }

        void button_close_Click(object sender, RoutedEventArgs e)
        {
            if (window.Grid_firstPage.Visibility == Visibility.Visible)
                window.Close();
            else if (window.Grid_MainPage.Visibility == Visibility.Visible)
                notifyIcon.WindowHide();
        }
    }
}
