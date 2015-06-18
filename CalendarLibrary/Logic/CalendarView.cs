using IDataBase;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EventLibrsries;
using System.Windows.Controls.Primitives;

namespace CalendarLibrary.Logic
{
    class CalendarView
    {
        public DateTime currentDate, tmpDate, selectedDate, prevSelectedDate;
        private static CalendarView calendarView {get;set;}
        String buttonName;
        List<Int32> dates;
        public List<Button> buttonList;
        public List<Border> borderList;
        Events events;
        SendEventToModel_Event sendEventToModel;
        SendEventAboutChanges_Event sendEventAboutChanges;
        SendToWeek_MondayDate_Event sendToWeek_mondayDate;
        EventToGetSelectedDate_Event monthEventsToSendDate;
        SendToMonthPanel_CurrentDate_Event sendDateToMonth;
        GetSelectedDate_Event sendToMonthDate;
        

        private CalendarView() 
        {
            currentDate = DateTime.Now;
            events = Events.CurrentEvents;
            SendEventToControl_Event.GetEvent += SendEventToControl_Event_GetEvent;
            sendEventToModel = new SendEventToModel_Event();
            sendEventAboutChanges = new SendEventAboutChanges_Event();
            sendToWeek_mondayDate = new SendToWeek_MondayDate_Event();
            sendDateToMonth = new SendToMonthPanel_CurrentDate_Event();
            SendToWeek_MondayDate_Event.GetEventToSendMonday += SendToWeek_MondayDate_Event_GetEventToSendMonday;
            monthEventsToSendDate = new EventToGetSelectedDate_Event();
            EventToGetSelectedDate_Event.GetEventToSendSelectedDate += EventToGetSelectedDate_Event_GetEventToSendSelectedDate;
            sendToMonthDate = new GetSelectedDate_Event();
            GetSelectedDate_Event.GetEvent += GetSelectedDate_Event_GetEvent;
            CleanEvent_Event.GetEvent += CleanEvent_Event_GetEvent;
            EventForClickCalendarButton_Event.GetEvent += EventForClickCalendarButton_Event_GetEvent;
        }

        void EventForClickCalendarButton_Event_GetEvent(object sender, EventForClickCalendarButton e)
        {
            foreach(Button button in buttonList)
            {
                if(button.Content.ToString() == e.day.ToString())
                {
                    if (button.Foreground == Brushes.Black || button.Foreground == Brushes.White)
                    {
                        button.RaiseEvent(new System.Windows.RoutedEventArgs(ButtonBase.ClickEvent));
                        break;
                    }
                }
            }
        }

        void CleanEvent_Event_GetEvent(object sender, CleanEvent e)
        {
            ResetSelectedDay();
            currentDate = DateTime.Now;
            selectedDate = DateTime.Now;
            LoadCurrentMonth(buttonList, borderList[5]);
        }

        void GetSelectedDate_Event_GetEvent(object sender, GetSelectedDate e)
        {
            sendToMonthDate.SendDate(selectedDate);
        }

        void EventToGetSelectedDate_Event_GetEventToSendSelectedDate(object sender, SendEventToGetSelectedDate e)
        {
            monthEventsToSendDate.SendSelectedDate(selectedDate);
        }

        void SendToWeek_MondayDate_Event_GetEventToSendMonday(object sender, SendToCalendarEventForReviewMondayDate e)
        {
            sendToWeek_mondayDate.SendEvent(GetMonday(selectedDate));
        }

        public void SendMondayDateToWeekPanel()
        {
            sendToWeek_mondayDate.SendEvent(GetMonday(selectedDate));
        }

        public void SendDateToMonth()
        {
            sendDateToMonth.SendEvent(selectedDate);
        }

        /// <summary>
        /// Метод для определения понедельника от текущей даты
        /// </summary>
        /// <param name="date">Текущая дата</param>
        /// <returns>Возвращает дату понедельника</returns>
         DateTime GetMonday(DateTime date)
        {
            DayOfWeek weekday = date.DayOfWeek;
            switch(weekday)
            {
                case DayOfWeek.Monday:

                    break;
                case DayOfWeek.Tuesday:
                    date = date.AddDays(-1);
                    break;
                case DayOfWeek.Wednesday:
                    date = date.AddDays(-2);
                    break;
                case DayOfWeek.Thursday:
                    date = date.AddDays(-3);
                    break;
                case DayOfWeek.Friday:
                    date = date.AddDays(-4);
                    break;
                case DayOfWeek.Saturday:
                    date = date.AddDays(-5);
                    break;
                case DayOfWeek.Sunday:
                    date = date.AddDays(-6);
                    break;
            }
            return date;
        }

   /// <summary>
   ///   Событие принимающее события с базы данных, для отображения
   /// </summary>
   /// <param name="e">содержит Dictionary(IEventMainInfo, IEveinInfo) и имя активной панели</param>
        void SendEventToControl_Event_GetEvent(object sender, SendEventToControl e)
        {
            SendEventToModel_Event.ControlNameEnum en = SendEventToModel_Event.ControlNameEnum.Day;
            if(e.controlName == SendEventToControl_Event.ControlNameEnum.Week)
                en = SendEventToModel_Event.ControlNameEnum.Week;
            else if(e.controlName == SendEventToControl_Event.ControlNameEnum.Month)
                en = SendEventToModel_Event.ControlNameEnum.Month;
            else if(e.controlName == SendEventToControl_Event.ControlNameEnum.Timetable)
                en = SendEventToModel_Event.ControlNameEnum.Timetable;
            
                
            if(e.controlName == SendEventToControl_Event.ControlNameEnum.Week)
            {
                DateTime date = GetMonday(selectedDate);
                sendEventToModel.SendEvent(date, en);
            }
            else
                sendEventToModel.SendEvent(selectedDate, en);

        }

        public void ResetSelectedDay()
        {
            selectedDate = new DateTime(0001, 01, 01);
            prevSelectedDate = selectedDate;
        }

        public static CalendarView GetInstance
        {
            get
            {
                if(calendarView == null)
                    calendarView = new CalendarView();
                return calendarView;
            }
            set
            {
                calendarView = value;
            }
        }
        
        public void LoadCurrentMonth(List<Button> buttonList, Border sixWeek)
        {
            
            selectedDate = currentDate;
            this.buttonList = buttonList;
            tmpDate = currentDate.AddMonths(-1);
            Int32 firstDayWeekOfCurrentMonth = NumOfFirstDayWeek(currentDate);
            Int32 DaysInPrevMonth = DateTime.DaysInMonth(tmpDate.Year,tmpDate.Month);
            Int32 PrevMonthStartDate = DaysInPrevMonth - firstDayWeekOfCurrentMonth + 2;
            Int32 daysInCurrentMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            Int32 count = 0;
            
            while(count < firstDayWeekOfCurrentMonth - 1)
            {
                buttonList[count].Content = PrevMonthStartDate.ToString();
                buttonList[count].Foreground = Brushes.Gray;
                PrevMonthStartDate++;
                count++;
            }
            Int32 currentDay = 1;
            while (currentDay <= daysInCurrentMonth)
            {
                buttonList[count].Content = currentDay.ToString();
                buttonList[count].Foreground = Brushes.Black;
                
                if (buttonList[count].Content.ToString() == DateTime.Now.Day.ToString())
                {
                    buttonName = buttonList[count].Name;
                    buttonList[count].Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(38, 166, 154));
                    buttonList[count].Foreground = System.Windows.Media.Brushes.White;
                }
                    
                currentDay++;
                count++;
            }
            if (count < 36)
                sixWeek.Visibility = System.Windows.Visibility.Hidden;
            else
                sixWeek.Visibility = System.Windows.Visibility.Visible;
            Int32 nextMonthDays = 1;
            while(count < 42)
            {
                
                buttonList[count].Content = nextMonthDays.ToString();
                buttonList[count].Foreground = Brushes.Gray;
                nextMonthDays++;
                count++;
            }
            sendEventAboutChanges.SendEvent(true);
                    
        }

        DateTime GetMonday()
        {
            DayOfWeek weekday = selectedDate.DayOfWeek;
            DateTime monday = new DateTime();
            switch(weekday)
            {
                case DayOfWeek.Monday:
                    break;
                case DayOfWeek.Tuesday:
                    monday = selectedDate.AddDays(-1);
                    break;
                case DayOfWeek.Wednesday:
                    monday = selectedDate.AddDays(-2);
                    break;
                case DayOfWeek.Thursday:
                    monday = selectedDate.AddDays(-3);
                    break;
                case DayOfWeek.Friday:
                    monday = selectedDate.AddDays(-4);
                    break;
                case DayOfWeek.Saturday:
                    monday = selectedDate.AddDays(-5);
                    break;
                case DayOfWeek.Sunday:
                    monday = selectedDate.AddDays(-6);
                    break;
            }
            return monday;
        }

        /// <summary>
        /// <para>Переход на следующий или предыдущий месяц</para>
        /// </summary>
        /// <param name="buttonList">Кнопки (числа календаря)</param>
        /// <param name="direction"></param>
        /// <param name="sixWeek"></param>
        public void SetMonth(List<Button> buttonList, String direction, Border sixWeek)
        {
            prevSelectedDate = new DateTime(0001, 01, 01);
            if(direction == "previous")
                currentDate = currentDate.AddMonths(-1);
            else if(direction == "next")
                currentDate = currentDate.AddMonths(1);
            
            tmpDate = currentDate.AddMonths(-1);
            Int32 firstDayWeekOfCurrentMonth = NumOfFirstDayWeek(currentDate);
            Int32 DaysInPrevMonth = DateTime.DaysInMonth(tmpDate.Year, tmpDate.Month);
            Int32 PrevMonthStartDate = DaysInPrevMonth - firstDayWeekOfCurrentMonth + 2;
            Int32 daysInCurrentMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            Int32 count = 0;

            while (count < firstDayWeekOfCurrentMonth - 1)
            {

                buttonList[count].Content = PrevMonthStartDate.ToString();
                buttonList[count].Foreground = Brushes.Gray;
                buttonList[count].Background = Brushes.White;
                PrevMonthStartDate++;
                count++;
            }

            Int32 currentDay = 1;
            while (currentDay <= daysInCurrentMonth)
            {
                buttonList[count].Content = currentDay.ToString();
                buttonList[count].Foreground = Brushes.Black;
                buttonList[count].Background = Brushes.White;

                if (buttonList[count].Content.ToString() == selectedDate.Day.ToString() && selectedDate.Year != 0001 &&
                    currentDate.Year == selectedDate.Year && currentDate.Month == selectedDate.Month)
                {
                    buttonList[count].Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(79, 195, 247));
                }

                if (buttonList[count].Content.ToString() == DateTime.Now.Day.ToString() && 
                    currentDate.Month == DateTime.Now.Month && currentDate.Year == DateTime.Now.Year)
                {
                    buttonName = buttonList[count].Name;
                    buttonList[count].Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(38, 166, 154));
                    buttonList[count].Foreground = System.Windows.Media.Brushes.White;
                }
                currentDay++;
                count++;                
            }
            if (count < 36)
                sixWeek.Visibility = System.Windows.Visibility.Hidden;
            else
                sixWeek.Visibility = System.Windows.Visibility.Visible;
                
            Int32 nextMonthDays = 1;
            while (count < 42)
            {
                buttonList[count].Content = nextMonthDays.ToString();
                buttonList[count].Foreground = Brushes.Gray;
                buttonList[count].Background = Brushes.White;
                nextMonthDays++;
                count++;
            }

            sendEventAboutChanges.SendEvent(true); 
        }

        /// <summary>
        /// Получение названия месяца
        /// </summary>
        /// <returns>Название мксяца в строке</returns>
        public String GetMonthName()
        {
            String month = "";
            if (currentDate.Month == 1)
                month = "Январь";
            else if (currentDate.Month == 2)
                month = "Февраль";
            else if (currentDate.Month == 3)
                month = "Март";
            else if (currentDate.Month == 4)
                month = "Апрель";
            else if (currentDate.Month == 5)
                month = "Май";
            else if (currentDate.Month == 6)
                month = "Июнь";
            else if (currentDate.Month == 7)
                month = "Июль";
            else if (currentDate.Month == 8)
                month = "Август";
            else if (currentDate.Month == 9)
                month = "Сентябрь";
            else if (currentDate.Month == 10)
                month = "Октябрь";
            else if (currentDate.Month == 11)
                month = "Ноябрь";
            else if (currentDate.Month == 12)
                month = "Декабрь";

            return month + " "+ currentDate.Year.ToString();
        }

        /// <summary>
        /// Метод для получения числа понедельника
        /// </summary>
        /// <param name="date">Любая дата от понедельника по воскресенье</param>
        /// <returns>Возвращает число понедельника</returns>
        public Int32 NumOfFirstDayWeek(DateTime date)
        {
            DateTime tmpdate = new DateTime(date.Year, date.Month, 1);
            DayOfWeek dayOfWeek = new DateTimeOffset(tmpdate).DayOfWeek;
            Int32 dWeekInt = 0;
            switch(dayOfWeek)
            {
                case DayOfWeek.Monday:
                    dWeekInt = 1;
                    break;
                case DayOfWeek.Tuesday:
                    dWeekInt = 2;
                    break;
                case DayOfWeek.Wednesday:
                    dWeekInt = 3;
                    break;
                case DayOfWeek.Thursday:
                    dWeekInt = 4;
                    break;
                case DayOfWeek.Friday:
                    dWeekInt = 5;
                    break;
                case DayOfWeek.Saturday:
                    dWeekInt = 6;
                    break;
                case DayOfWeek.Sunday:
                    dWeekInt = 7;
                    break;
            }
            return dWeekInt;
        }
        
        public void SetMonthMentions(IData idata)
        {
            dates =  idata.GetMentionDates(currentDate);
            Int32 firstDay = NumOfFirstDayWeek(currentDate);
            Int32 daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            Int32 tmp = 0;
            while(tmp < daysInMonth)
            {
                foreach (Int32 i in dates)
                    if (buttonList[firstDay].Content.ToString() == i.ToString())
                        buttonList[firstDay].Foreground = new SolidColorBrush(Color.FromRgb(38, 166, 154));
                firstDay++;
                ++tmp;
            }

            
        }

        public void ButtonMouseEnter(Button button)
        {
            button.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(79, 195, 247));
        }

        public void SelectedDay()
        {
            foreach(Button button in buttonList)
            {
                if (button.Foreground == Brushes.Black || button.Foreground == Brushes.White)
                {
                    if (Int32.Parse(button.Content.ToString()) == selectedDate.Day)
                    {
                        button.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(79, 195, 247));
                    }
                    else if (Int32.Parse(button.Content.ToString()) == prevSelectedDate.Day)
                    {                  
                        button.Background = Brushes.White;
                    }
                }
                if(button.Foreground == Brushes.White && currentDate.Month == DateTime.Now.Month && currentDate.Year == DateTime.Now.Year &&
                    currentDate.Day == Int32.Parse(button.Content.ToString()) && selectedDate.Day != currentDate.Day)
                {
                    button.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(38, 166, 154));
                }
            }
        }

        public void ButtonMouseLeave(Button button)
        {
            Int32 firstDayWeek, daysInMonth;
            firstDayWeek = NumOfFirstDayWeek(currentDate);
            daysInMonth = DateTime.DaysInMonth(currentDate.Year,currentDate.Month);
            button.Background = System.Windows.Media.Brushes.White;
            if (button.Foreground == System.Windows.Media.Brushes.LightGray && button.Background == System.Windows.Media.Brushes.LightGray)
            {
                button.BorderBrush = System.Windows.Media.Brushes.White;
            }
            if (currentDate.Year == selectedDate.Year && currentDate.Month == selectedDate.Month)
            {
                if (button.Foreground == Brushes.White || button.Foreground == Brushes.Black)
                {
                    if (selectedDate.Day == Int32.Parse(button.Content.ToString()))
                    {
                        button.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(79, 195, 247));
                        
                    }
                }
            }
            if (dates != null)
            {
                foreach (Int32 i in dates)
                {
                    Int32 index = buttonList.IndexOf(button);
                    if (index >= firstDayWeek && index <= (firstDayWeek + daysInMonth))
                    {
                        button.Foreground = new SolidColorBrush(Color.FromRgb(38, 166, 154));
                        if(currentDate.Year == selectedDate.Year && currentDate.Month == selectedDate.Month && selectedDate.Day == Int32.Parse(button.Content.ToString()))
                            button.Foreground = new SolidColorBrush(Color.FromRgb(79, 195, 247));
                    }
                }
            }
            if (button.Name == buttonName)
            {
                if (currentDate.Year != DateTime.Now.Year || currentDate.Month != DateTime.Now.Month || button.Content.ToString() != DateTime.Now.Day.ToString())
                    button.Background = System.Windows.Media.Brushes.White;
                else
                    button.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(38, 166, 154));
            }
        }
    }
}
