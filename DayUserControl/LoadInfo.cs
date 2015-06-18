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

namespace DayUserControl
{
    public class LoadInfo
    {
        DayControl control;
        List<Hyperlink> hyperlinkList;
        Events events;

        public LoadInfo(DayControl control, List<Hyperlink> hyperlinkList, Events events)
        {
            this.control = control;
            this.hyperlinkList = hyperlinkList;
            this.events = events;
            LoadEventInfo_Event.GetEvent += LoadEventInfo_Event_GetEvent;
        }

        void LoadEventInfo_Event_GetEvent(object sender, LoadEventInfo e)
        {
            if (e.calendarEnum == LoadEventInfo_Event.CalendarEnum.Day)
            {
                SetInfoTexts(e.dictionaryInfo);
                events.eventDictionary = e.dictionaryInfo;
            }
        }

        public void SetInfoTexts(Dictionary<IEventMainInfo,IEventInfo> dict)
        {
            foreach(Hyperlink link in hyperlinkList)
                link.Inlines.Clear();

            foreach(Grid grid in events.gridList)
            {
                grid.Background = Brushes.White;
            }

            foreach(IEventMainInfo mainInfo in dict.Keys)
            {
                Int32 indexFrom = 0, indexTo = 0, hour = 0, min = 0;
            //Index from
                hour = mainInfo.dateFrom.Hour * 2;
                if (mainInfo.dateFrom.Minute < 30)
                    min = 0;
                else
                    min = 1;
                indexFrom = hour + min;
            //Index to
                hour = mainInfo.dateTo.Hour * 2;
                if (mainInfo.dateTo.Minute < 30)
                    min = 0;
                else
                    min = 1;
                indexTo = hour + min;
        
            //paiting Event
                Int32 indexCount = 0;
                foreach (Grid grid in events.gridList)
                {
                    if(indexCount >= indexFrom && indexCount <= indexTo)
                    {
                        if (grid.Background == Brushes.White)
                            grid.Background = new SolidColorBrush(Color.FromRgb(224, 242, 241));
                        if (indexCount == indexFrom)
                        {
                            String hyperlinkName = "h" + grid.Name.Substring(1);
                            Hyperlink link = control.FindName(hyperlinkName) as Hyperlink;
                            if (link.Inlines.Count > 0)
                            {

                                Int32 countLinks = 0;

                                String str = (link.Inlines.FirstOrDefault() as Run).Text;
                                Int32 i = str.Length - 8;
                                if (str.Length > 8 && str.Substring((str.Length - 8), str.Length - i) == " событий")
                                {
                                    Int32 index = (link.Inlines.FirstOrDefault() as Run).Text.Length - 8;
                                    String strTmp = (link.Inlines.FirstOrDefault() as Run).Text.Substring(0, index);
                                    countLinks = Int32.Parse(strTmp) + 1;
                                }
                                else
                                    countLinks = 2;
                                link.Inlines.Clear();
                                link.Inlines.Add(String.Format("{0} событий", countLinks));
                            }
                            else
                                link.Inlines.Add(mainInfo.EventName);
                        }
                    }
                    indexCount++;
                }
            }
        }

        public void ClearInfoTexts()
        {
            foreach(Hyperlink link in hyperlinkList)
            {
                link.Inlines.Clear();
            }
        }
    }
}
