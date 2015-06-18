using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventLibrsries
{
    public class SendEventToGetSelectedDate: EventArgs
    {
        public Boolean key { get; private set; }
        public SendEventToGetSelectedDate(Boolean key)
        {
            this.key = key;
        }
    }

    public class GetEventWithySelectedDate: EventArgs
    {
        public DateTime date { get; private set; }
        public GetEventWithySelectedDate(DateTime date)
        {
            this.date = date;
        }
    }

    public class EventToGetSelectedDate_Event
    {
        public static event EventHandler<SendEventToGetSelectedDate> GetEventToSendSelectedDate;
        public static event EventHandler<GetEventWithySelectedDate> GetSelectedDate;

        public void SendSelectedDate(DateTime date)
        {
            if (GetSelectedDate != null)
                GetSelectedDate(this, new GetEventWithySelectedDate(date));
        }
        public void SendEvent_SelectedDate(Boolean key)
        {
            if (GetEventToSendSelectedDate != null)
                GetEventToSendSelectedDate(this, new SendEventToGetSelectedDate(key));
        }
    }

    public class SendToMonthPanel_CurrentDate: EventArgs
    {
        public DateTime date { get; private set; }
        public SendToMonthPanel_CurrentDate(DateTime date)
        {
            this.date = date;
        }
    }

    public class SendToMonthPanel_CurrentDate_Event
    {
        public static event EventHandler<SendToMonthPanel_CurrentDate> GetDateEvent;
        public void SendEvent(DateTime date)
        {
            if(GetDateEvent != null)
            {
                GetDateEvent(this, new SendToMonthPanel_CurrentDate(date));
            }
        }
    }

    public class MonthControlMouseLeave_PopupIsOpen: EventArgs
    {
        public Boolean key { get; set; }
        public MonthControlMouseLeave_PopupIsOpen(Boolean key)
        {
            this.key = key;
        }
    }

    public class MonthControlMouseLeave_SendToPopup: EventArgs
    {
        public Boolean key { get; set; }
        public MonthControlMouseLeave_SendToPopup(Boolean key)
        {
            this.key = key;
        }
    }

    public class MonthControl_Popup
    {
        public static event EventHandler<MonthControlMouseLeave_PopupIsOpen> GetPopupIsOpenEvent;
        public static event EventHandler<MonthControlMouseLeave_SendToPopup> GetMouseLeaveEvent;
        public void SendPopupIsOpenEvent(Boolean key)
        {
            if (GetPopupIsOpenEvent != null)
                GetPopupIsOpenEvent(this, new MonthControlMouseLeave_PopupIsOpen(key));
        }
        public void SendMouseLeaveEventFromMainPage(Boolean key)
        {
            if (GetMouseLeaveEvent != null)
                GetMouseLeaveEvent(this, new MonthControlMouseLeave_SendToPopup(key));
        }
    }

    public class GetSelectedDate: EventArgs
    {
        public Boolean key { get; private set; }
        public DateTime date { get; private set; }
        public GetSelectedDate(Boolean key)
        {
            this.key = key;
        }
        public GetSelectedDate(DateTime date)
        {
            this.date = date;
        }
    }

    public class GetSelectedDate_Event
    {
        public static event EventHandler<GetSelectedDate> GetEvent;
        public static event EventHandler<GetSelectedDate> GetDate;
        public void SendEvent(Boolean key)
        {
            if (GetEvent != null)
                GetEvent(this, new GetSelectedDate(key));
        }
        public void SendDate(DateTime date)
        {
            if (GetDate != null)
                GetDate(this, new GetSelectedDate(date));
        }
    }
}
