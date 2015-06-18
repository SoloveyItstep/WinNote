using EventLibrsries;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Timetable_UserControl.View
{
    public class VisualStyle
    {
        Timetable_UserControl control;
        EventForClickCalendarButton_Event eventForcLick;
        Dictionary<IEventMainInfo, IEventInfo> infoDictionary;

        public VisualStyle(Timetable_UserControl control)
        {
            this.control = control;
            infoDictionary = new Dictionary<IEventMainInfo, IEventInfo>();
            eventForcLick = new EventForClickCalendarButton_Event();
        }
        public void SelectEvents()
        {
            LoadEventInfo_Event.GetEvent += LoadEventInfo_Event_GetEvent;
        }

        void LoadEventInfo_Event_GetEvent(object sender, LoadEventInfo e)
        {
            if (e.calendarEnum == LoadEventInfo_Event.CalendarEnum.Timetable)
            {
                infoDictionary = e.dictionaryInfo;
                control.MainGrid.Children.Clear();

                Int32 index = 40;
                Int32 count = 0;

                List<IEventMainInfo> list = GetSortedInfo();

                foreach (IEventMainInfo info in list)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.MaxWidth = 500;
                    textBlock.Margin = new System.Windows.Thickness(30, index, 0, 0);
                    textBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Top;

                    Hyperlink hyperlink = new Hyperlink();
                    hyperlink.Inlines.Add(info.EventName + "\t" + info.dateFrom.ToLongDateString());
                    hyperlink.Name = "h" + count.ToString();
                    hyperlink.TextDecorations = null;
                    hyperlink.Click += hyperlink_Click;

                    textBlock.Inlines.Add(hyperlink);

                    control.MainGrid.Children.Add(textBlock);
                    count++;
                    index += 40;
                }
            }
        }

        List<IEventMainInfo> GetSortedInfo()
        {
            List<IEventMainInfo> lst = new List<IEventMainInfo>();
            foreach (IEventMainInfo mainInfo in infoDictionary.Keys)
                lst.Add(mainInfo);
            
            lst.OrderBy(data => data.dateFrom);
            return lst;
        }

        void hyperlink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Hyperlink hyperlink = sender as Hyperlink;
            Int32 count = Int32.Parse(hyperlink.Name.Substring(1));
            Int32 index = 0;

            foreach (IEventMainInfo info in infoDictionary.Keys)
            {
                if(index == count)
                {
                    eventForcLick.SendEvent(info.dateFrom.Day);
                    break;
                }
                index++;
            }

        }

        
    }
}
