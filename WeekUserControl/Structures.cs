using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WeekUserControl
{
    public struct VisualVariables
    {
        public List<Grid> allMiddleGrids { get; set; }
        public List<Grid> mondayList { get; set; }
        public List<Grid> tuesdayList { get; set; }
        public List<Grid> wednesdayList { get; set; }
        public List<Grid> thirsdayList { get; set; }
        public List<Grid> fridayList { get; set; }
        public List<Grid> saturdayList { get; set; }
        public List<Grid> sundayList { get; set; }
        public Int32 columnNum { get; set; }
        public List<Label> labelsList { get; set; }
        public DateTime selectedDate { get; set; }
        public String gridName { get; set; }
        public Int32 selectedIndex { get; set; }
    }

    public struct HyperKinkLists
    {
        public List<Hyperlink> monday { get; set; }
        public List<Hyperlink> tuesday { get; set; }
        public List<Hyperlink> wednesday { get; set; }
        public List<Hyperlink> thirsday { get; set; }
        public List<Hyperlink> friday { get; set; }
        public List<Hyperlink> saturday { get; set; }
        public List<Hyperlink> sunday { get; set; }
    }

    public struct Dates
    {
        public DateTime from { get; set; }
        public DateTime to { get; set; }
    }

    public struct Info
    {
        public Dictionary<IEventMainInfo, IEventInfo> infoDictionary { get; set; }
    }
}
