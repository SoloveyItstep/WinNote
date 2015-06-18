using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalendarAddMenu_UserControl.Event
{
    public class CheckCorrectEntrance
    {
        CalendarAddMenu_UserControl control;

        public CheckCorrectEntrance()
        {
            CheckOnCorrectEntrance.setEvent += CheckOnCorrectEntrance_setEvent;
        }

        void CheckOnCorrectEntrance_setEvent(object sender, CheckCorrectEntranceEvent e)
        {
            control = e.control;
        }

        void CheckCorrectEntrance_SetEvent(object sender, CheckCorrectEntranceEvent e)
        {
            control = e.control;
            if (CheckAllEntrance())
                CheckOnCorrectEntrance.SendCheckedEventAnswer(true);
        }

        public Boolean CheckAllEntrance()
        {
            if (!Times())
                return false;
            if (control.CheckBox_Reminder.IsChecked == true && !RemindEventSelected())
                return false;
            if (control.CheckBox_RepeatEvent.IsChecked == true && !RepeatEvent_ComboBoxItemSelect())
                return false;
            if (control.CheckBox_RepeatEvent.IsChecked == true && control.ComboBox_enumRepeatEvent.Text == "Каждую неделю")
                if (!RepeatEvent_In_Week())
                    return false;
            return true;
        }

        Boolean Times()
        {
            Regex regex = new Regex(@"^(\d{1,2}:(\d{1,2}))");
            if (!regex.IsMatch(control.TextBox_timeFrom.Text) || !regex.IsMatch(control.TextBox_timeTo.Text))
                return false;
            return true;
        }
        Boolean RemindEventSelected()
        {
            if (control.CheckBox_Reminder.IsChecked == true && control.ComboBox_enumRemindEvent.SelectedIndex != -1)
                return true;
            return false;
        }
        Boolean RepeatEvent_ComboBoxItemSelect()
        {
            if (control.CheckBox_RepeatEvent.IsChecked == true)
                if (control.ComboBox_enumRepeatEvent.SelectedIndex != -1)
                    return true;
            return false;
        }
        Boolean RepeatEvent_In_Week()
        {
            Boolean key = true;
            if (control.CheckBox_RepeatEvent.IsChecked == true && control.ComboBox_enumRepeatEvent.Text == "Каждую неделю")
            {
                key = false;
                if (control.checkBox_Monday.IsChecked == true || control.checkBox_Tuesday.IsChecked == true)
                    key = true;
                else if (control.checkBox_Wednesday.IsChecked == true || control.checkBox_Thirsday.IsChecked == true)
                    key = true;
                else if (control.checkBox_Friday.IsChecked == true || control.checkBox_Saturday.IsChecked == true)
                    key = true;
                else if (control.checkBox_Sunday.IsChecked == true)
                    key = true;
            }
            return key;
        }

        String GetEventName()
        {
            String name = "";
            return name;
        }
    }
}
