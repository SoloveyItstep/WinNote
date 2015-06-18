using EventLibrsries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace CalendarLibrary.Logic
{
    class Events
    {
        public static List<Button> day_buttonsList;
        public CalendarControl calenrarContr = null;
        public Date_CkickEvent events;
        CalendarView calendarView;
        private static Events ev { get; set; }
        SendEventAboutChanges_Event sendEventAboutChanges;
        NextOrPreviousEvent nextPreviousEvent;
        DispatcherTimer timer;

        public Events(CalendarControl calendarCS)
        {
            calenrarContr = calendarCS;
            ev = this;
            calendarView = CalendarView.GetInstance;
            nextPreviousEvent = new NextOrPreviousEvent(calendarCS);
            SetBorders();
        }

        public void SetBorders()
        {
            if (calendarView == null)
                calendarView = CalendarView.GetInstance;
            if (calendarView.borderList == null)
                calendarView.borderList = new List<Border>();

            calendarView.borderList.Add(calenrarContr.Border_firstWeek);
            calendarView.borderList.Add(calenrarContr.Border_secondWeek);
            calendarView.borderList.Add(calenrarContr.Border_thirdWeek);
            calendarView.borderList.Add(calenrarContr.Border_forthWeek);
            calendarView.borderList.Add(calenrarContr.Border_fifthWeek);
            calendarView.borderList.Add(calenrarContr.Border_sixthWeek);
        }
        public static Events CurrentEvents
        {
            get
            {
                return ev;
            }
        }

        public void FillButtons()
        {
            if (day_buttonsList == null)
            {
                day_buttonsList = new List<Button>();
                events = new Date_CkickEvent();
                calendarView = CalendarView.GetInstance;
            }
            else
                return;
            foreach(Button button in calenrarContr.Grid_firstWeek.Children)
                day_buttonsList.Add(button);
            foreach(Button button in calenrarContr.Grid_secondWeek.Children)
                day_buttonsList.Add(button);
            foreach (Button button in calenrarContr.Grid_thirdWeek.Children)
                day_buttonsList.Add(button);
            foreach (Button button in calenrarContr.Grid_forthWeek.Children)
                day_buttonsList.Add(button);
            foreach (Button button in calenrarContr.Grid_fifthWeek.Children)
                day_buttonsList.Add(button);
            foreach (Button button in calenrarContr.Grid_sixthWeek.Children)
                day_buttonsList.Add(button);

            calendarView.LoadCurrentMonth(day_buttonsList, calenrarContr.Border_sixthWeek);
            calenrarContr.label_MonthName.Content = calendarView.GetMonthName();

            calenrarContr.button_NextMonth.MouseEnter += button_NextPreviousMonth_MouseEnter;
            calenrarContr.button_PreviousMonth.MouseEnter += button_NextPreviousMonth_MouseEnter;
            calenrarContr.button_NextMonth.MouseLeave += button_NextPreviousMonth_MouseLeave;
            calenrarContr.button_PreviousMonth.MouseLeave += button_NextPreviousMonth_MouseLeave;
        }

        void button_NextPreviousMonth_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Background = null;
        }

        void button_NextPreviousMonth_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Background = new SolidColorBrush(Color.FromRgb(79, 195, 247));
        }

        public void SetEvents()
        {
            if (day_buttonsList == null)
                FillButtons();
            foreach(Button button in day_buttonsList)
                button.Click += button_Click;
            foreach(Button button in day_buttonsList)
            {
                button.MouseEnter += button_MouseEnter;
                button.MouseLeave += button_MouseLeave;
            }

            calenrarContr.button_PreviousMonth.Click += button_PreviousMonth_Click;
            calenrarContr.button_NextMonth.Click += button_NextMonth_Click;
            sendEventAboutChanges = new SendEventAboutChanges_Event();
            nextPreviousEvent.SetEvents();
            SetTimer();
        }

        void SetTimer()
        {
            timer = new DispatcherTimer();
            DateTime today = DateTime.Today.AddDays(1);
            DateTime next = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            DateTime now = DateTime.Now;
            Double hour, min, sec;
            Double totalSec = (next - now).TotalSeconds;
            hour = (totalSec - totalSec % 3600) / 3600;
            totalSec -= (hour * 3600);
            min = (totalSec - (totalSec % 60)) / 60;
            sec = (totalSec - (min * 60));

            if (sec > 0)
                sec -= 1;

            timer.Interval = new TimeSpan((Int32)hour, (Int32)min, (Int32)sec);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            DateChanged();
            SetTimer();
        }
       
        void DateChanged()
        {
            calendarView.LoadCurrentMonth(calendarView.buttonList, calendarView.borderList[5]);
            
        }

        void button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            calendarView.ButtonMouseLeave(sender as Button);
        }

        void button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            calendarView.ButtonMouseEnter(sender as Button);
        }

        void button_NextMonth_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            calendarView.SetMonth(day_buttonsList, "next", calenrarContr.Border_sixthWeek);
            calenrarContr.label_MonthName.Content = calendarView.GetMonthName();
        }

        void button_PreviousMonth_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            calendarView.SetMonth(day_buttonsList, "previous", calenrarContr.Border_sixthWeek);
            calenrarContr.label_MonthName.Content = calendarView.GetMonthName();
        }

        void button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Foreground == Brushes.Gray)
                return;
            DateTime date = new DateTime(calendarView.currentDate.Year,calendarView.currentDate.Month,Int32.Parse(button.Content.ToString()));
            events.Click(button, date);
            if (button.Foreground != Brushes.Gray)
            {
                if (calendarView.selectedDate.Year != 0001 && calendarView.selectedDate.Month != 01)
                    calendarView.prevSelectedDate = calendarView.selectedDate;
                calendarView.selectedDate = date;
                calendarView.SelectedDay();
                sendEventAboutChanges.SendEvent(true);
                calendarView.SetMonth(calendarView.buttonList, "current", calendarView.borderList[5]);
            }
        }
    }
}
