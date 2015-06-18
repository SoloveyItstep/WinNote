using EventLibrsries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace RegisterUserControl
{
    class Events
    {
        CreateStandartCategoriesTextDocuments_Event createStandartNotes;

        public static RegisterControl control;
        public Events(RegisterControl control)
        {
            AfterLogInEvent.logInEvent += AfterLogInEvent_logInEvent;
            Events.control = control;
            control.TextBox_Name_RegisterMenu.GotFocus += TextBoxGotFocus;
            control.TextBox_Name_RegisterMenu.GotKeyboardFocus += TextBoxGotFocus;
            control.TextBox_Name_RegisterMenu.LostFocus += TextBoxLostFocus;
            control.TextBox_Name_RegisterMenu.LostKeyboardFocus += TextBoxLostFocus;
            control.TextBox_LastName_RegisterMenu.GotFocus += TextBoxGotFocus;
            control.TextBox_LastName_RegisterMenu.GotKeyboardFocus += TextBoxGotFocus;
            control.TextBox_LastName_RegisterMenu.LostFocus += TextBoxLostFocus;
            control.TextBox_LastName_RegisterMenu.LostKeyboardFocus += TextBoxLostFocus;
            control.TextBox_Email_RegisterMenu.GotFocus += TextBoxGotFocus;
            control.TextBox_Email_RegisterMenu.GotKeyboardFocus += TextBoxGotFocus;
            control.TextBox_Email_RegisterMenu.LostFocus += TextBoxLostFocus;
            control.TextBox_Email_RegisterMenu.LostKeyboardFocus += TextBoxLostFocus;
            control.TextBox_Email_RegisterMenu.KeyUp += TextBox_LiginMenu_KeyDown;
            control.TextBox_Password_RegisterMenu.GotFocus += TextBoxGotFocus;
            control.TextBox_Password_RegisterMenu.LostFocus += TextBoxLostFocus;
            control.TextBox_ConfirmPassword_RegisterMenu.GotFocus += TextBoxGotFocus;
            control.TextBox_ConfirmPassword_RegisterMenu.LostFocus += TextBoxLostFocus;
            control.TextBox_ConfirmPassword_RegisterMenu.KeyUp += TextBox_ConfirmPassword_RegisterMenu_KeyDown;
            control.TextBox_Password_RegisterMenu.KeyUp += TextBox_ConfirmPassword_RegisterMenu_KeyDown;
            control.Button_Register.Click += Button_Register_Click;
            createStandartNotes = new CreateStandartCategoriesTextDocuments_Event();
        }

        void AfterLogInEvent_logInEvent(object sender, AfterLogin e)
        {
            if (e.logedIn)
            {
                control.TextBox_Name_RegisterMenu.Foreground = Brushes.LightGray;
                control.TextBox_Name_RegisterMenu.Text = "Имя";
                control.Border_NameTxt_RegisterMenu.BorderBrush = Brushes.LightGray;
                control.TextBox_LastName_RegisterMenu.Foreground = Brushes.LightGray;
                control.TextBox_LastName_RegisterMenu.Text = "Фамилия";
                control.Border_LastNameTxt_RegisterMenu.BorderBrush = Brushes.LightGray;
                control.TextBox_Password_RegisterMenu.Foreground = Brushes.LightGray;
                control.TextBox_Password_RegisterMenu.Text = "Пароль";
                control.Border_PasswordTxt_RegisterMenu.BorderBrush = Brushes.LightGray;
                control.TextBox_Email_RegisterMenu.Foreground = Brushes.LightGray;
                control.TextBox_Email_RegisterMenu.Text = "Email";
                control.Border_EmailTxt_RegisterMenu.BorderBrush = Brushes.LightGray;
                control.TextBox_ConfirmPassword_RegisterMenu.Foreground = Brushes.LightGray;
                control.TextBox_ConfirmPassword_RegisterMenu.Text = "Пароль ещё раз";
                control.Border_ConfirmPasswordTxt_RegisterMenu.BorderBrush = Brushes.LightGray;
            }
            createStandartNotes.SendEvent(true);
        }

        String GetConfirmPass()
        {
            return control.TextBox_ConfirmPassword_RegisterMenu.Text;
        }

        void TextBox_ConfirmPassword_RegisterMenu_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Name == "TextBox_ConfirmPassword_RegisterMenu")
            {
                if (!String.Equals(textBox.Text, control.TextBox_Password_RegisterMenu.Text) && textBox.Text != "Пароль ещё раз" && textBox.Text != "")
                {
                    control.Border_ConfirmPasswordTxt_RegisterMenu.BorderBrush = Brushes.Red;
                    control.Border_PasswordTxt_RegisterMenu.BorderBrush = Brushes.Red;
                }
                else
                {
                    control.Border_ConfirmPasswordTxt_RegisterMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
                    control.Border_PasswordTxt_RegisterMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
                }
            }
            else if (textBox.Name == "TextBox_Password_RegisterMenu" && control.TextBox_ConfirmPassword_RegisterMenu.Text != "Пароль ещё раз")
            {
                control.Border_ConfirmPasswordTxt_RegisterMenu.BorderBrush = Brushes.Red;
                control.Border_PasswordTxt_RegisterMenu.BorderBrush = Brushes.Red;
            }
        }

        void Button_Register_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(control.TextBox_Name_RegisterMenu.Text == "Имя")
            {
                control.Border_NameTxt_RegisterMenu.BorderBrush = Brushes.Red;
                return;
            }
            else
                control.Border_NameTxt_RegisterMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
            
            if(control.TextBox_LastName_RegisterMenu.Text == "Фамилия")
            {
                control.Border_LastNameTxt_RegisterMenu.BorderBrush = Brushes.Red;
                return;
            }
            else
                control.Border_LastNameTxt_RegisterMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
   
            if (control.TextBox_Email_RegisterMenu.Text == "Email")
            {
                control.Border_EmailTxt_RegisterMenu.BorderBrush = Brushes.Red;
                return;
            }
            else
                control.Border_EmailTxt_RegisterMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));

            if (control.TextBox_Password_RegisterMenu.Text == "Пароль")
            {
                control.Border_PasswordTxt_RegisterMenu.BorderBrush = Brushes.Red;
                return;
            }
            else
                control.Border_PasswordTxt_RegisterMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));

            if (control.TextBox_ConfirmPassword_RegisterMenu.Text == "Пароль ещё раз")
            {
                control.Border_ConfirmPasswordTxt_RegisterMenu.BorderBrush = Brushes.Red;
                return;
            }
            else
                control.Border_ConfirmPasswordTxt_RegisterMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));

            RegisterEventData ev = new RegisterEventData();
            ev.GetData(new RegisterData() { Name = control.TextBox_Name_RegisterMenu.Text,
                                                LastName = control.TextBox_LastName_RegisterMenu.Text, Email = control.TextBox_Email_RegisterMenu.Text,
                                                Password = control.TextBox_Password_RegisterMenu.Text});
            GetLoginDataEvent even = new GetLoginDataEvent();
            even.GetData(new LoginData() { login = control.TextBox_Email_RegisterMenu.Text, password = control.TextBox_Password_RegisterMenu.Text });

            //RegisterEvent ev = new RegisterEvent(new RegisterData() { Name = control.TextBox_Name_RegisterMenu.Text,
            //                                    LastName = control.TextBox_LastName_RegisterMenu.Text, Email = control.TextBox_Email_RegisterMenu.Text,
            //                                    Password = control.TextBox_Password_RegisterMenu.Text});
        }

        private void TextBox_LiginMenu_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (!Regex.IsMatch(textBox.Text, @"((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)([,;]\s*)*)+"))
                    control.Border_EmailTxt_RegisterMenu.BorderBrush = Brushes.Red;
            else
                    control.Border_EmailTxt_RegisterMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
        }

        private void TextBoxLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Name == "TextBox_Name_RegisterMenu")
            {
                if (textBox.Text == "" && textBox.Foreground == Brushes.Gray)
                {
                    textBox.Foreground = Brushes.LightGray;
                    textBox.Text = "Имя";
                }
                control.Border_NameTxt_RegisterMenu.BorderBrush = Brushes.LightGray;
            }
            else if (textBox.Name == "TextBox_LastName_RegisterMenu")
            {
                if (textBox.Text == "" && textBox.Foreground == Brushes.Gray)
                {
                    textBox.Foreground = Brushes.LightGray;
                    textBox.Text = "Фамилия";
                }
                control.Border_LastNameTxt_RegisterMenu.BorderBrush = Brushes.LightGray;
            }
            else if (textBox.Name == "TextBox_Password_RegisterMenu")
            {
                if (textBox.Text == "" && textBox.Foreground == Brushes.Gray)
                {
                    textBox.Foreground = Brushes.LightGray;
                    textBox.Text = "Пароль";
                }
                control.Border_PasswordTxt_RegisterMenu.BorderBrush = Brushes.LightGray;
            }
            else if (textBox.Name == "TextBox_Email_RegisterMenu")
            {
                if (textBox.Text == "" && textBox.Foreground == Brushes.Gray)
                {
                    textBox.Foreground = Brushes.LightGray;
                    textBox.Text = "Email";
                }
                control.Border_EmailTxt_RegisterMenu.BorderBrush = Brushes.LightGray;
            }
            else if (textBox.Name == "TextBox_ConfirmPassword_RegisterMenu")
            {
                if (textBox.Text == "" && textBox.Foreground == Brushes.Gray)
                {
                    textBox.Foreground = Brushes.LightGray;
                    textBox.Text = "Пароль ещё раз";
                }
                control.Border_ConfirmPasswordTxt_RegisterMenu.BorderBrush = Brushes.LightGray;
            }
        }

        private void TextBoxGotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            
            if (textBox.Name == "TextBox_Name_RegisterMenu")
            {
                if (textBox.Text == "Имя" && textBox.Foreground == Brushes.LightGray)
                {
                    textBox.Foreground = Brushes.Gray;
                    textBox.Text = "";
                }
                control.Border_NameTxt_RegisterMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
            }
            else if (textBox.Name == "TextBox_Password_RegisterMenu")
            {
                if (textBox.Text == "Пароль" && textBox.Foreground == Brushes.LightGray)
                {
                    textBox.Foreground = Brushes.Gray;
                    textBox.Text = "";
                }
                control.Border_PasswordTxt_RegisterMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
            }
            else if (textBox.Name == "TextBox_LastName_RegisterMenu")
            {
                if (textBox.Text == "Фамилия" && textBox.Foreground == Brushes.LightGray)
                {
                    textBox.Foreground = Brushes.Gray;
                    textBox.Text = "";
                }
                control.Border_LastNameTxt_RegisterMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
            }
            else if (textBox.Name == "TextBox_Email_RegisterMenu")
            {
                if (textBox.Text == "Email" && textBox.Foreground == Brushes.LightGray)
                {
                    textBox.Foreground = Brushes.Gray;
                    textBox.Text = "";
                }
                control.Border_EmailTxt_RegisterMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
            }
            else if (textBox.Name == "TextBox_ConfirmPassword_RegisterMenu")
            {
                if (textBox.Text == "Пароль ещё раз" && textBox.Foreground == Brushes.LightGray)
                {
                    textBox.Foreground = Brushes.Gray;
                    textBox.Text = "";
                }
                control.Border_ConfirmPasswordTxt_RegisterMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
            }
        }
    }
}
