using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Day_UserControl.Model
{
    public class Variables
    {
        private static Variables variables { get; set; }
        private Variables()
        {
            gridList = new List<Grid>();
            hyperlinkList = new List<Hyperlink>();
            eventInfoDictionary = new Dictionary<IEventMainInfo, IEventInfo>();
        }

        public static Variables GetInstance
        {
            get
            {
                if (variables == null)
                    variables = new Variables();
                return variables;
            }
            set
            {
                variables = value;
            }
        }

        public List<Grid> gridList { get; set; }
        public List<Hyperlink> hyperlinkList { get; set; }
        public Dictionary<IEventMainInfo, IEventInfo> eventInfoDictionary { get; set; }
        public Label TopGridLabel { get; set; }
        public Grid SelectedGrid { get; set; }
        public Int32 FirstSelectedIndex { get; set; }
        public Int32 LastSelectedIndex { get; set; }
        public DateTime currentDate { get; set; }
    }

    public class MyPopup
    {
        private static MyPopup mypopup { get; set; }
        private MyPopup() { }

        public static MyPopup GetInstance
        {
            get
            {
                if (mypopup == null)
                    mypopup = new MyPopup();
                return mypopup;
            }
            set
            {
                mypopup = value;
            }
        }

        public Dictionary<Int32, Hyperlink> popupHyperlinkDictionary { get; set; }
        public Int32 hyperlinkDay { get; set; }
        public Dictionary<Int32, Int32> hyperlinkIndex { get; set; }
        public IEventMainInfo currentEventMainInfo { get; set; }
        public IEventInfo currentEventInfo { get; set; }
    }
}
