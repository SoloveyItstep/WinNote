using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CalendarAddMenu_UserControl.Event
{
    public class AnimationEvents
    {
        private static AnimationEvents animationEv { get; set; }
        private CalendarAddMenu_UserControl control;
        private AnimationEvents(CalendarAddMenu_UserControl control)
        { this.control = control; }

        public static AnimationEvents GetInstance(CalendarAddMenu_UserControl control)
        {
            if (animationEv == null)
                animationEv = new AnimationEvents(control);
            return animationEv;
        }

        public void SetAnimateEvents()
        {
            control.CheckBox_RepeatEvent.Checked += CheckBox_Checked;
            control.CheckBox_RepeatEvent.Unchecked += CheckBox_Unchecked;
            control.CheckBox_Reminder.Checked += CheckBox_Checked;
            control.CheckBox_Reminder.Unchecked += CheckBox_Unchecked;

            control.ComboBox_enumRepeatEvent.SelectionChanged += ComboBox_SelectionChanged;
        }

        public void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            Double height = control.Height;
            if (checkBox.Name == "CheckBox_RepeatEvent")
            {
                if (control.Grid_RepeatingEvent.Height == 0)
                {
                    ControlAnimationHeight(height + 60);
                    AnimationHeight(0, 60, ref control.Grid_RepeatingEvent);

                    AnimationThickness(control.Grid_Reminder_menu.Margin, new Thickness(0, control.Grid_Reminder_menu.Margin.Top + 60, 0, 0), ref control.Grid_Reminder_menu);
                    AnimationThickness(control.Grid_BottomMenu.Margin, new Thickness(0, control.Grid_BottomMenu.Margin.Top + 60, 0, 0), ref control.Grid_BottomMenu);
                    Double marginTop = control.Grid_RemindEvent.Margin.Top + 60;
                    AnimationThickness(control.Grid_RemindEvent.Margin, new Thickness(0, marginTop, 0, 0), ref control.Grid_RemindEvent);
                }
            }
            else if (checkBox.Name == "CheckBox_Reminder")
            {
                ControlAnimationHeight(height + 60);
                AnimationHeight(0, 60, ref control.Grid_RemindEvent);

                Double topMargin = control.Grid_BottomMenu.Margin.Top + 60;
                AnimationThickness(control.Grid_BottomMenu.Margin, new Thickness(0, topMargin, 0, 0), ref control.Grid_BottomMenu);
                Double d = control.Grid_Reminder_menu.Margin.Top + 25;
                AnimationThickness(control.Grid_Reminder_menu.Margin, new Thickness(0, d, 0, 0), ref control.Grid_RemindEvent);
            }
        }

        void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            Double height = control.Height;
            DoubleAnimation animGridReminder;
            ThicknessAnimation thickAnim = new ThicknessAnimation();
            if (checkBox.Name == "CheckBox_RepeatEvent")
            {
                Double heightGrid = control.Grid_RepeatingEvent.Height;

                animGridReminder = new DoubleAnimation(heightGrid, 0, TimeSpan.FromMilliseconds(100));
                if (heightGrid == 120)
                    AnimationHeight(60, 0, ref control.Grid_RepeatEvent_Weeks);

                ControlAnimationHeight(height - heightGrid);
                AnimationHeight(heightGrid, 0, ref control.Grid_RepeatingEvent);
                AnimationThickness(control.Grid_Reminder_menu.Margin, new Thickness(0, 275, 0, 0), ref control.Grid_Reminder_menu);
                Double marginTop = control.Grid_RemindEvent.Margin.Top - heightGrid;
                AnimationThickness(control.Grid_RemindEvent.Margin, new Thickness(0, marginTop, 0, 0), ref control.Grid_RemindEvent);

                if (control.Grid_RemindEvent.Height == 0)
                    AnimationThickness(control.Grid_BottomMenu.Margin, new Thickness(0, 310, 0, 0), ref control.Grid_BottomMenu);
                else
                    AnimationThickness(control.Grid_BottomMenu.Margin, new Thickness(0, control.Grid_BottomMenu.Margin.Top - heightGrid, 0, 0), ref control.Grid_BottomMenu);

                control.ComboBox_enumRepeatEvent.Items.Refresh();
                control.ComboBox_enumRepeatEvent.SelectedIndex = -1;
                control.ComboBox_enumRepeatEvent.Text = "";
            }
            else if (checkBox.Name == "CheckBox_Reminder")
            {
                AnimationHeight(60, 0, ref control.Grid_RemindEvent);
                control.ComboBox_enumRemindEvent.Items.Refresh();
                control.ComboBox_enumRemindEvent.SelectedIndex = -1;
                control.ComboBox_enumRemindEvent.Text = "";

                Double margimTop = control.Grid_BottomMenu.Margin.Top - 60;
                AnimationThickness(control.Grid_BottomMenu.Margin, new Thickness(0, margimTop, 0, 0), ref control.Grid_BottomMenu);
                ControlAnimationHeight(height - 60);
            }
        }

        public void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            if (combo.Name == "ComboBox_enumRepeatEvent")
            {
                String str = "";
                if (combo.SelectedItem != null)
                    str = combo.SelectedItem.ToString();
                else
                    str = combo.Text;
                Double height = control.Height;
                if (str.Length == 51)
                {
                    if (str.Substring(38, 13) == "Каждую неделю")
                    {
                        AnimationHeight(60, 120, ref control.Grid_RepeatingEvent);
                        AnimationHeight(0, 60, ref control.Grid_RepeatEvent_Weeks);
                        ControlAnimationHeight(height + 60);

                        AnimationThickness(control.Grid_Reminder_menu.Margin, new Thickness(0, 390, 0, 0), ref control.Grid_Reminder_menu);
                        if (control.Grid_RemindEvent.Height == 0)
                            AnimationThickness(control.Grid_BottomMenu.Margin, new Thickness(0, 430, 0, 0), ref control.Grid_BottomMenu);
                        else
                        {
                            Double marginTop = control.Grid_BottomMenu.Margin.Top + 60;
                            AnimationThickness(control.Grid_BottomMenu.Margin, new Thickness(0, marginTop, 0, 0), ref control.Grid_BottomMenu);
                            marginTop = control.Grid_RemindEvent.Margin.Top + 60;
                            AnimationThickness(control.Grid_RemindEvent.Margin, new Thickness(0, marginTop, 0, 0), ref control.Grid_RemindEvent);
                        }
                        control.checkBox_Monday.IsChecked = false;
                        control.checkBox_Tuesday.IsChecked = false;
                        control.checkBox_Wednesday.IsChecked = false;
                        control.checkBox_Thirsday.IsChecked = false;
                        control.checkBox_Friday.IsChecked = false;
                        control.checkBox_Saturday.IsChecked = false;
                        control.checkBox_Sunday.IsChecked = false;
                    }
                }
                else if (str.Length <= 15 && str == "Каждую неделю")
                {
                    return;
                }
                else if (str.Length != 51 || str.Substring(38, 13) == "Каждую неделю")
                {
                    if (control.Grid_RepeatEvent_Weeks.Height == 60)
                    {
                        AnimationHeight(120, 60, ref control.Grid_RepeatingEvent);
                        AnimationHeight(60, 0, ref control.Grid_RepeatEvent_Weeks);
                        ControlAnimationHeight(height - 60);
                        AnimationThickness(control.Grid_RemindEvent.Margin, new Thickness(0, control.Grid_RemindEvent.Margin.Top - 60, 0, 0),
                            ref control.Grid_RemindEvent);

                        AnimationThickness(control.Grid_Reminder_menu.Margin, new Thickness(0, 330, 0, 0), ref control.Grid_Reminder_menu);
                        AnimationThickness(control.Grid_BottomMenu.Margin, new Thickness(0, control.Grid_BottomMenu.Margin.Top - 60, 0, 0),
                            ref control.Grid_BottomMenu);
                    }
                }
            }

        }

        public void AnimationHeight(Double from, Double to, ref Grid grid)
        {
            DoubleAnimation animation = new DoubleAnimation(from, to, TimeSpan.FromMilliseconds(100));
            grid.BeginAnimation(Rectangle.HeightProperty, animation);
        }

        public void AnimationThickness(Thickness marginFrom, Thickness marginTo, ref Grid grid)
        {
            ThicknessAnimation animation = new ThicknessAnimation();
            animation.From = marginFrom;
            animation.To = marginTo;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            grid.BeginAnimation(Grid.MarginProperty, animation);
        }

        public void ControlAnimationHeight(Double to)
        {
            DoubleAnimation animation = new DoubleAnimation(control.Height, to, TimeSpan.FromMilliseconds(100));
            control.BeginAnimation(Rectangle.HeightProperty, animation);
        }

    }
}
