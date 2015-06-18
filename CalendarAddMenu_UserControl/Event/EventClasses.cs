using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalendarAddMenu_UserControl.Event
{
    public class AbortButtonClickEvent : EventArgs
    {
        public Boolean AbortEvent { get; private set; }
        public AbortButtonClickEvent(Boolean abortEvent)
        {
            AbortEvent = abortEvent;
        }
    }

    public class CheckCorrectEntranceEvent: EventArgs
    {
        public CalendarAddMenu_UserControl control { get; private set; }
        public Boolean key { get; set; }
        public CheckCorrectEntranceEvent(ref CalendarAddMenu_UserControl control)
        {
            this.control = control;
        }

        public CheckCorrectEntranceEvent(Boolean key)
        {
            this.key = key;
        }
    }

    public class CheckOnCorrectEntrance
    {
        public static event EventHandler<CheckCorrectEntranceEvent> setEvent;
        public static event EventHandler<CheckCorrectEntranceEvent> getCheckedEvent;
        public void CheckEvent(ref CalendarAddMenu_UserControl control)
        {
            setEvent(this, new CheckCorrectEntranceEvent(ref control));
        }

        public static void SendCheckedEventAnswer(Boolean key)
        {
            getCheckedEvent(new CheckOnCorrectEntrance(), new CheckCorrectEntranceEvent(key));
        }
    }

    
}
