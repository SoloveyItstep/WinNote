using Day_UserControl.Model;
using EventLibrsries;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Day_UserControl.DayEvents
{
    public class LoadInfo
    {
        DayUserControl control;
        Variables variables;
        GetCurrentDateToDayControl_Event getCurrentDate;

        public LoadInfo(DayUserControl control)
        {
            this.control = control;
            variables = Variables.GetInstance;
            getCurrentDate = new GetCurrentDateToDayControl_Event();
            GetCurrentDateToDayControl_Event.GetDate += GetCurrentDateToDayControl_Event_GetDate;
        }

        /// <summary>
        /// событие - получение выбранной даты в календаре
        /// </summary>
        /// <param name="e">Класс с выбранной датой</param>
        void GetCurrentDateToDayControl_Event_GetDate(object sender, GetCurrentDateToDayControl e)
        {
            variables.currentDate = e.date;
        }

        /// <summary>
        /// Создание заголовка (день недели и число)
        /// </summary>
        public void SelectMainContent()
        {
            DateTime date = DateTime.Now;
            if (variables.eventInfoDictionary.Count > 0)
            {
                foreach (IEventMainInfo mainInfo in variables.eventInfoDictionary.Keys)
                {
                    date = mainInfo.dateFrom;
                    break;
                }
            }
            else
            {
                getCurrentDate.SendEvent(true);
                date = variables.currentDate;
            }
            SelectTopContent(date.DayOfWeek, date.Day);
            variables.currentDate = new DateTime(date.Year, date.Month, date.Day);
        }

        /// <summary>
        /// Определение дня недели и вывод в заглавии
        /// </summary>
        /// <param name="dayOfWeek">Класс - День недели</param>
        /// <param name="day">Дата</param>
        void SelectTopContent(DayOfWeek dayOfWeek, Int32 day)
        {
            String content = "";
            switch(dayOfWeek)
            {
                case DayOfWeek.Monday:
                    content = "Понедельник, ";
                    break;
                case DayOfWeek.Tuesday:
                    content = "Вторник, ";
                    break;
                case DayOfWeek.Wednesday:
                    content = "Среда, ";
                    break;
                case DayOfWeek.Thursday:
                    content = "Четверг, ";
                    break;
                case DayOfWeek.Friday:
                    content = "Пятница, ";
                    break;
                case DayOfWeek.Saturday:
                    content = "Суббота, ";
                    break;
                case DayOfWeek.Sunday:
                    content = "Воскресенье, ";
                    break;
            }
            content += day.ToString();
            variables.TopGridLabel.Content = content;
        }

        /// <summary>
        /// Вывод сохраненных событий
        /// </summary>
        public void LoadEventsInfo()
        {
            foreach (Grid grid in variables.gridList)
                grid.Background = Brushes.White;

            foreach (Hyperlink hyperlink in variables.hyperlinkList)
                hyperlink.Inlines.Clear();

            foreach(IEventMainInfo info in variables.eventInfoDictionary.Keys)
            {
                Int32 hFrom = 0, hTo = 0, mFrom = 0, mTo = 0;
                hFrom = info.dateFrom.Hour * 2;
                if (hFrom > 0)
                    hFrom -= 1;
                hTo = info.dateTo.Hour * 2;
                if (info.dateFrom.Minute >= 0 && info.dateFrom.Minute < 30)
                    mFrom = 0;
                else
                    mFrom = 1;
                if (info.dateTo.Minute >= 0 && info.dateTo.Minute < 30)
                    mTo = 1;
                else
                    mTo = 2;
        //======================
                Int32 from = hFrom + mFrom + 1, to = hTo + mTo - 2;

                for(Int32 index = 0; index < 48; ++index)
                {
                    if (index == from)
                        LoadHypelinkInlines(index, info.EventName);

                    if(index >= from && index <= to)
                    {
                        if (variables.gridList[index].Background != new SolidColorBrush(Color.FromRgb(224, 242, 241)))
                            variables.gridList[index].Background = new SolidColorBrush(Color.FromRgb(224, 242, 241));

                    }
                }
            }
        }

        /// <summary>
        /// Заполнение Hyperlink названиями событий
        /// </summary>
        /// <param name="index">Индекс в перечислении (для обращении к нужному Hyperlink в 'variables.hyperlinkList')</param>
        /// <param name="name">Имя события</param>
        void LoadHypelinkInlines(Int32 index, String name)
        {

            String selectedName = "";

            if (variables.hyperlinkList[index].Inlines.Count > 0)
                selectedName = (variables.hyperlinkList[index].Inlines.FirstOrDefault() as Run).Text;
            if (selectedName == null || selectedName == "")
            {
                variables.hyperlinkList[index].Inlines.Add(name);
            }
            else if(selectedName != null && selectedName.Length > 8 && selectedName.Substring(selectedName.Length - 8) == " событий")
            {
                String tmp = "";
                if (Char.IsDigit(selectedName[0]))
                    tmp = selectedName.Substring(0, 1);
                if (Char.IsDigit(selectedName[1]))
                    tmp += selectedName[1].ToString();

                Int32 count = Int32.Parse(tmp);
                variables.hyperlinkList[index].Inlines.Add(count + " событий");
            }
            else if (selectedName.Length > 8 && selectedName.Substring(selectedName.Length - 8) != " событий" || selectedName.Length <= 8)
            {
                variables.hyperlinkList[index].Inlines.Add("2 событий");
            }

        }
    }
}
