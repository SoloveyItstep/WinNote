using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarAddMenu_UserControl.Model
{
    public class VarEventInfo
    {
        private static VarEventInfo eventInf { get; set; }
        private VarEventInfo(){}

        public static VarEventInfo GetInstance
        {
            get
            {
                if (eventInf == null)
                    eventInf = new VarEventInfo();
                return eventInf;
            }
            set
            {
                eventInf = value;
            }
        }
        public IEventMainInfo eventMainInfo { get; set; }
        public IEventInfo eventInfo { get; set; }
    }
}
