using EventLibrsries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace LoginUserControl
{
    public class Events
    {
        static LoginControl control {get;set;}
        public static GetLoginDataEvent ev;
        public Events(LoginControl control)
        {
            AfterLogInEvent.logInEvent += AfterLogInEvent_logInEvent;
            Events.control = control;
            control.TextBox_Login_LiginMenu.GotFocus += TextBoxGotFocus;
            control.TextBox_Login_LiginMenu.GotKeyboardFocus += TextBoxGotFocus;
            control.TextBox_Login_LiginMenu.LostFocus += TextBoxLostFocus;
            control.TextBox_Login_LiginMenu.LostKeyboardFocus += TextBoxLostFocus;
            control.TextBox_Password_LiginMenu.GotFocus += TextBoxGotFocus;
            control.TextBox_Password_LiginMenu.GotKeyboardFocus += TextBoxGotFocus;
            control.TextBox_Password_LiginMenu.LostFocus += TextBoxLostFocus;
            control.TextBox_Password_LiginMenu.LostKeyboardFocus += TextBoxLostFocus;
            control.TextBox_Login_LiginMenu.KeyDown += TextBox_LiginMenu_KeyDown;
            control.Button_Login_LoginMenu.Click += Button_Login_LoginMenu_Click;
            
        }

        void AfterLogInEvent_logInEvent(object sender, AfterLogin e)
        {
            if (e.logedIn)
            {
                control.Border_LoginTxt_LoginMenu.BorderBrush = Brushes.LightGray;
                control.TextBox_Login_LiginMenu.Foreground = Brushes.LightGray;
                control.TextBox_Login_LiginMenu.Text = "Email";
                control.TextBox_Password_LiginMenu.Foreground = Brushes.LightGray;
                control.TextBox_Password_LiginMenu.Text = "Пароль";
                control.Border_PasswordTxt_LoginMenu.BorderBrush = Brushes.LightGray;
            }
        }

        void Button_Login_LoginMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (control.TextBox_Login_LiginMenu.Text == "Email")
            {
                control.Border_LoginTxt_LoginMenu.BorderBrush = Brushes.Red;
                return;
            }
            else
                control.Border_LoginTxt_LoginMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
            if (control.TextBox_Password_LiginMenu.Text == "Пароль")
            {
                control.Border_PasswordTxt_LoginMenu.BorderBrush = Brushes.Red;
                return;
            }
            else
                control.Border_PasswordTxt_LoginMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
            
            ev = new GetLoginDataEvent();
            ev.GetData(new LoginData() { login = control.TextBox_Login_LiginMenu.Text, password = control.TextBox_Password_LiginMenu.Text });
        }

        
        /// <summary>
        /// задание цвета border из вне контролла
        /// 
        /// </summary>
        /// <param name="block">
        /// login - login border
        /// password - password border
        /// </param>
        public static void SetRedBorder(String block)
        {
            if(block == "login")
                control.Border_LoginTxt_LoginMenu.BorderBrush = Brushes.Red;
            else if(block == "password")
                control.Border_PasswordTxt_LoginMenu.BorderBrush = Brushes.Red;
        }

        public static void SetBlueBorder(String block)
        {
            if (block == "login")
                control.Border_LoginTxt_LoginMenu.BorderBrush = Brushes.Red;
            else if (block == "password")
                control.Border_PasswordTxt_LoginMenu.BorderBrush = Brushes.Red;
        }
        
        private void TextBox_LiginMenu_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (!Regex.IsMatch(textBox.Text, @"((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)([,;]\s*)*)+"))
                    control.Border_LoginTxt_LoginMenu.BorderBrush = Brushes.Red;
            else
                    control.Border_LoginTxt_LoginMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
        }

        private void TextBoxLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Name == "TextBox_Login_LiginMenu")
            {
                if (textBox.Text == "" && textBox.Foreground == Brushes.Gray)
                {
                    control.Border_LoginTxt_LoginMenu.BorderBrush = Brushes.LightGray;
                    textBox.Foreground = Brushes.LightGray;
                    textBox.Text = "Email";
                }
                if (control.Border_LoginTxt_LoginMenu.BorderBrush != Brushes.Red)
                    control.Border_LoginTxt_LoginMenu.BorderBrush = Brushes.LightGray;
            }
            else if (textBox.Name == "TextBox_Password_LiginMenu")
            {
                if (textBox.Text == "" && textBox.Foreground == Brushes.Gray)
                {
                    textBox.Foreground = Brushes.LightGray;
                    textBox.Text = "Пароль";
                }
                control.Border_PasswordTxt_LoginMenu.BorderBrush = Brushes.LightGray;
            }
        }

        private void TextBoxGotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Name == "TextBox_Login_LiginMenu")
            {
                if (textBox.Text == "Email" && textBox.Foreground == Brushes.LightGray)
                {
                    textBox.Foreground = Brushes.Gray;
                    textBox.Text = "";
                }
                if (control.Border_LoginTxt_LoginMenu.BorderBrush != Brushes.Red)
                    control.Border_LoginTxt_LoginMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
            }
            else if (textBox.Name == "TextBox_Password_LiginMenu")
            {
                if (textBox.Text == "Пароль" && textBox.Foreground == Brushes.LightGray)
                {
                    textBox.Foreground = Brushes.Gray;
                    textBox.Text = "";
                }
                control.Border_PasswordTxt_LoginMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
            }
        }
    }
}
