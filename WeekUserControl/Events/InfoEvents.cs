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

namespace WeekUserControl.Events
{
    public class InfoEvents
    {
        SendEventToModel_Event sendEventToModel;
        WeekControl control;
        VisualStyles visual;
        public Info info;
        
        public InfoEvents(WeekControl control)
        {
            this.control = control;
            visual = VisualStyles.GetInstance(control);
            info = new Info();
            sendEventToModel = new SendEventToModel_Event();
        }

        public void Events()
        {
            LoadEventInfo_Event.GetEvent += LoadEventInfo_Event_GetEvent;
            sendEventToModel.SendEvent(visual.variables.selectedDate, SendEventToModel_Event.ControlNameEnum.Week);
        }

        void LoadEventInfo_Event_GetEvent(object sender, LoadEventInfo e)
        {
            if(e.calendarEnum == LoadEventInfo_Event.CalendarEnum.Week)
            {
                info.infoDictionary = e.dictionaryInfo;
                SelectEvents();
            }
        }

        void SelectEvents()
        {
            DateTime date = visual.variables.selectedDate;
            Dictionary<IEventMainInfo,IEventInfo> inf1 = new Dictionary<IEventMainInfo,IEventInfo>();
            FillDictionaries(ref inf1, 0);
            Dictionary<IEventMainInfo, IEventInfo> inf2 = new Dictionary<IEventMainInfo, IEventInfo>();
            FillDictionaries(ref inf2, 1);
            Dictionary<IEventMainInfo, IEventInfo> inf3 = new Dictionary<IEventMainInfo, IEventInfo>();
            FillDictionaries(ref inf3, 2);
            Dictionary<IEventMainInfo, IEventInfo> inf4 = new Dictionary<IEventMainInfo, IEventInfo>();
            FillDictionaries(ref inf4, 3);
            Dictionary<IEventMainInfo, IEventInfo> inf5 = new Dictionary<IEventMainInfo, IEventInfo>();
            FillDictionaries(ref inf5, 4);
            Dictionary<IEventMainInfo, IEventInfo> inf6 = new Dictionary<IEventMainInfo, IEventInfo>();
            FillDictionaries(ref inf6, 5);
            Dictionary<IEventMainInfo, IEventInfo> inf7 = new Dictionary<IEventMainInfo,IEventInfo>();
            FillDictionaries(ref inf7, 6);
            visual.ClearHyperlinks();
            visual.ClearGrids();

            if(inf1 != null)
                foreach (IEventMainInfo inf in inf1.Keys)
                    SelectInfo(inf,visual.hyperlinkLists.monday, visual.variables.mondayList);

            if (inf2 != null)
                foreach(IEventMainInfo inf in inf2.Keys)
                    SelectInfo(inf, visual.hyperlinkLists.tuesday, visual.variables.tuesdayList);

            if (inf3 != null)
                foreach (IEventMainInfo inf in inf3.Keys)
                    SelectInfo(inf, visual.hyperlinkLists.wednesday, visual.variables.wednesdayList);

            if (inf4 != null)
                foreach (IEventMainInfo inf in inf4.Keys)
                    SelectInfo(inf, visual.hyperlinkLists.thirsday, visual.variables.thirsdayList);

            if (inf5 != null)
                foreach (IEventMainInfo inf in inf5.Keys)
                    SelectInfo(inf, visual.hyperlinkLists.friday, visual.variables.fridayList);

            if (inf6 != null)
                foreach (IEventMainInfo inf in inf6.Keys)
                    SelectInfo(inf, visual.hyperlinkLists.saturday, visual.variables.saturdayList);

            if (inf7 != null)
            foreach (IEventMainInfo inf in inf7.Keys)
                SelectInfo(inf, visual.hyperlinkLists.sunday, visual.variables.sundayList);
        }

        void FillDictionaries(ref Dictionary<IEventMainInfo,IEventInfo> dict, Int32 index)
        {
               foreach(var v in info.infoDictionary)
                {
                    if (v.Key.dateFrom.Day == visual.variables.selectedDate.AddDays(index).Day)
                        dict.Add(v.Key,v.Value);
                }
        }

        void SelectInfo(IEventMainInfo info, List<Hyperlink> hyperLST, List<Grid> gridLST)
        {
            Int32 indexFrom = 0, indexTo = 0;
            indexFrom = info.dateFrom.Hour * 2;
            if (info.dateFrom.Minute >= 30)
                indexFrom += 1;

            indexTo = info.dateTo.Hour * 2;
            if (info.dateTo.Minute >= 0 && info.dateTo.Minute < 30)
                indexTo += 1;
            else if(info.dateTo.Minute >= 30)
                indexTo += 2;

            if (indexTo > 0)
                indexTo -= 2;

            for(Int32 index = 0; index < 48; ++index)
            {
                if(index >= indexFrom && index <= indexTo)
                {
            //======================== GRID
                    gridLST[index].Background = new SolidColorBrush(Color.FromRgb(224, 242, 241));
            //======================== HYPERLINK
                    if(index == indexFrom)
                    {
                        if (hyperLST[index].Inlines.Count > 0)
                        {

                            Int32 countLinks = 0;

                            String str = (hyperLST[index].Inlines.FirstOrDefault() as Run).Text;
                            Int32 i = str.Length - 8;
                            if (str.Length > 8 && str.Substring((str.Length - 8), str.Length - i) == " событий")
                            {
                                Int32 ind = (hyperLST[index].Inlines.FirstOrDefault() as Run).Text.Length - 8;
                                String strTmp = (hyperLST[index].Inlines.FirstOrDefault() as Run).Text.Substring(0, ind);
                                countLinks = Int32.Parse(strTmp) + 1;
                            }
                            else
                                countLinks = 2;
                            hyperLST[index].Inlines.Clear();
                            hyperLST[index].Inlines.Add(String.Format("{0} событий", countLinks));
                        }
                        else
                            hyperLST[index].Inlines.Add(info.EventName);
                    }
                }
            }
        }
    }
}
