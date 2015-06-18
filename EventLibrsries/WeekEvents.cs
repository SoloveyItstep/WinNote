using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventLibrsries
{
    public class WeekMouseMove_afterClick: EventArgs
    {
        public Boolean key { get; private set; }
        public WeekMouseMove_afterClick(Boolean key)
        {
            this.key = key;
        }
    }

    public class WeekMouseMove_afterClick_Event
    {
        public static event EventHandler<WeekMouseMove_afterClick> GetEvent;
        public void SendEvent(Boolean key)
        {
            if (GetEvent != null)
                GetEvent(this, new WeekMouseMove_afterClick(key));
        }
    }

    public class SendToCalendarEventForReviewMondayDate: EventArgs
    {
        public Boolean key { get; private set; }
        public SendToCalendarEventForReviewMondayDate(Boolean key)
        {
            this.key = key;
        }
    }

    public class SendToWeek_MondayDate: EventArgs
    {
        public DateTime monday { get; private set; }
        public SendToWeek_MondayDate(DateTime monday)
        {
            this.monday = monday;
        }
    }

    public class SendToWeek_MondayDate_Event
    {
        public static event EventHandler<SendToWeek_MondayDate> GetEvent;
        public static event EventHandler<SendToCalendarEventForReviewMondayDate> GetEventToSendMonday;

        public void SendEventToSendMonday(Boolean key)
        {
            if (GetEventToSendMonday != null)
                GetEventToSendMonday(this, new SendToCalendarEventForReviewMondayDate(key));
        }
        public void SendEvent(DateTime monday)
        {
            if (GetEvent != null)
                GetEvent(this, new SendToWeek_MondayDate(monday));
        }
    }

    public class SendDatesTo_AddEventMenu : EventArgs
    {
        public DateTime from { get; private set; }
        public DateTime to { get; private set; }
        public SendDatesTo_AddEventMenu(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }
    }

    public class SendDatesTo_AddEventMenu_Event
    {
        public static event EventHandler<SendDatesTo_AddEventMenu> GetEvent;
        public void SendEvent(DateTime from, DateTime to)
        {
            if (GetEvent != null)
                GetEvent(this, new SendDatesTo_AddEventMenu(from, to));
        }
    }
}
