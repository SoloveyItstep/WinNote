using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace MonthUserControl.Model
{
    public struct MainVariables
    {
        public DateTime selectedDate { get; set; }
        public List<Grid> gridList { get; set; }
        public List<Hyperlink> hyperlinkList { get; set; }
        public Dictionary<String, List<Hyperlink>> daysHyperlinkDictionary { get; set; }
        public List<Label> labelsDaysNameList { get; set; }

    }

    public struct EventInfo
    {
        public Dictionary<IEventMainInfo, IEventInfo> dictionatyEventInfo { get; set; }
    }

    public struct MyPopup
    {
        public Dictionary<Int32,Hyperlink> popupHyperlinkDictionary { get; set; }
        public Int32 hyperlinkDay { get; set; }
        public Dictionary<Int32, Int32> hyperlinkIndex { get; set; }
        public IEventMainInfo currentEventMainInfo { get; set; }
        public IEventInfo currentEventInfo { get; set; }
    }
}
