using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace WeekUserControl
{
    public class MyPopup
    {
        private static MyPopup mypopup { get; set; }
        private MyPopup() 
        {
            popupHyperlinkDictionary = new Dictionary<int, Hyperlink>();
            hyperlinkIndex = new Dictionary<int, int>();
        }

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
