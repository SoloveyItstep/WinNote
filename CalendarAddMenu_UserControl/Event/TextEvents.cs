using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CalendarAddMenu_UserControl.Event
{
    public class TextEvents
    {
        private static CalendarAddMenu_UserControl control;
        private static TextEvents textEv { get; set; }
        private TextEvents(CalendarAddMenu_UserControl cont)
        { control = cont; }
        public static TextEvents GetInstance(CalendarAddMenu_UserControl cont)
        {
            if (textEv == null)
                textEv = new TextEvents(cont);
            return textEv;
        }

        public void SetTextEvents()
        {
            control.TextBox_eventName.GotFocus += TextBox_GotFocus;
            control.TextBox_Description.GotFocus += TextBox_GotFocus;
            control.TextBox_Locate.GotFocus += TextBox_GotFocus;
            control.TextBox_eventName.GotKeyboardFocus += TextBox_GotFocus;
            control.TextBox_Description.GotKeyboardFocus += TextBox_GotFocus;
            control.TextBox_Locate.GotKeyboardFocus += TextBox_GotFocus;

            control.TextBox_eventName.LostFocus += TextBox_LostFocus;
            control.TextBox_Description.LostFocus += TextBox_LostFocus;
            control.TextBox_Locate.LostFocus += TextBox_LostFocus;
            control.TextBox_eventName.LostKeyboardFocus += TextBox_LostFocus;
            control.TextBox_Description.LostKeyboardFocus += TextBox_LostFocus;
            control.TextBox_Locate.LostKeyboardFocus += TextBox_LostFocus;

            control.TextBox_timeFrom.PreviewTextInput += TextBox_PreviewTextInput;
            control.TextBox_timeTo.PreviewTextInput += TextBox_PreviewTextInput;
            control.TextBox_timeFrom.TextChanged += TextBox_TextChanged;
            control.TextBox_timeTo.TextChanged += TextBox_TextChanged;
        }

        void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            if (textbox.Name == "TextBox_eventName")
            {
                if (textbox.Text == "")
                {
                    textbox.Foreground = Brushes.LightGray;
                    textbox.Text = "Название события";
                }
            }
            else if (textbox.Name == "TextBox_Description")
            {
                if (textbox.Text == "")
                {
                    textbox.Foreground = Brushes.LightGray;
                    textbox.Text = "Описание";
                }
            }
            else if (textbox.Name == "TextBox_Locate")
            {
                if (textbox.Text == "")
                {
                    textbox.Foreground = Brushes.LightGray;
                    textbox.Text = "Место проведения";
                }
            }
        }

        void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            if (textbox.Name == "TextBox_eventName")
            {
                if (textbox.Text == "Название события" && textbox.Foreground == Brushes.LightGray)
                {
                    textbox.Foreground = Brushes.Gray;
                    textbox.Text = "";
                }
            }
            else if (textbox.Name == "TextBox_Description")
            {
                if (textbox.Text == "Описание" && textbox.Foreground == Brushes.LightGray)
                {
                    textbox.Foreground = Brushes.Gray;
                    textbox.Text = "";
                }
            }
            else if (textbox.Name == "TextBox_Locate")
            {
                if (textbox.Text == "Место проведения" && textbox.Foreground == Brushes.LightGray)
                {
                    textbox.Foreground = Brushes.Gray;
                    textbox.Text = "";
                }
            }
          }

        void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            String timeStr = CheckTimeString(textBox.Text);
            if (!String.Equals(timeStr, textBox.Text))
            {
                Int32 index = textBox.CaretIndex;
                textBox.Text = timeStr;
                textBox.CaretIndex = index;
            }
            if (textBox.Text.Length > 1)
            {
                String time = CheckTimeCorectlyEntered(textBox.Text, textBox.Name);

                if (!String.Equals(time, textBox.Text))
                {
                    Int32 index = textBox.CaretIndex;
                    textBox.Text = time;
                    textBox.CaretIndex = index;
                }
            }
        }

        public String CheckTimeCorectlyEntered(String time, String textBoxName)
        {
            if (Char.IsDigit(time[0]) && Char.IsDigit(time[1]))
            {
                Int32 hour = Int32.Parse(time.Substring(0, 2));
                if (hour > 24)
                {
                    if (time.Length == 2)
                        time = "24";
                    else if (time.Length == 3)
                        time = "24:";
                    else
                        time = "24" + time.Substring(2, time.Length - 2);
                }
            }
            if (textBoxName == "TextBox_timeFrom" && control.TextBox_timeTo.Text != "" && new Regex(@"^(\d{1,2}:(\d{2}))").IsMatch(time))
            {

                String timeTo = control.TextBox_timeTo.Text;
                Int32 hourFrom = 0, minFrom = 0, hourTo = 0, minTo = 0;
                hourFrom = Int32.Parse(time.Substring(0, control.TextBox_timeFrom.Text.Length - 3));
                Int32 length = time.Length;
                String tmp = time.Substring(length - 2, 2);
                minFrom = Int32.Parse(time.Substring(length - 2, 2));
                length = timeTo.Length;
                if (timeTo.Length == 3 && timeTo[1] == ':' && Char.IsDigit(timeTo[0]))
                {
                    hourTo = Int32.Parse(timeTo[0].ToString());
                    minTo = Int32.Parse(timeTo.Substring(2, 1));
                }
                else if(timeTo.Length == 4 && timeTo[2] == ':')
                {
                    hourTo = Int32.Parse(timeTo.Substring(0, 2));
                    minTo = Int32.Parse(timeTo.Substring(3));
                }
                else
                {
                    hourTo = Int32.Parse(timeTo.Substring(0, length - 3));
                    minTo = Int32.Parse(timeTo.Substring(length - 2, 2));
                }
                if(hourFrom > hourTo || hourFrom == hourTo && minFrom > minTo)
                    time = timeTo;
                

            }
            return time;
        }

        public String CheckTimeString(String time)
        {
            if (time.Length == 3 && !(new Regex(@"^(\d{2}):").IsMatch(time)))
            {
                if (!Char.IsDigit(time[0]))
                    time = time.Substring(1, 2) + ":";
                else if (new Regex(@"^(\d{3})").IsMatch(time))
                    time = time.Substring(0, 2) + ":" + time[2];
            }
            else if (time.Length == 4 && !(new Regex(@"^(\d{2}):(\d{1})").IsMatch(time)))
            {
                if (new Regex(@"^(\d{3})").IsMatch(time.Substring(0, 3)))
                    time = time.Substring(0, 2) + ":" + time[2].ToString();
            }
            else if (time.Length == 5 && !(new Regex(@"^(\d{2}):(\d{2})").IsMatch(time)))
            {
                if (new Regex(@"^(\d{3})").IsMatch(time.Substring(0, 3)))
                    time = time.Substring(0, 2) + ":" + time[2].ToString() + time[4].ToString();
            }
            return time;
        }

        void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text.Length == 1)
            {
                if (Char.IsDigit(e.Text[0]))
                    return;
                else if (e.Text[0] != ':')
                    e.Handled = true;
            }
            else if (textBox.Text.Length == 2)
            {
                if (Char.IsDigit(e.Text[0]) && textBox.Text[1] == ':')
                { }
                else if (e.Text[0] != ':')
                    e.Handled = true;
            }
            else if (textBox.Text.Length > 4)
                e.Handled = true;
            else if (textBox.Text.Length == 4 && textBox.Text[1] == ':')
                e.Handled = true;
            else if (!Char.IsDigit(e.Text[0]))
                e.Handled = true;

        }

    }
}
