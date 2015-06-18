using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventLibrsries
{
    public class DayUserControl_SendMoveEventAfterClick: EventArgs
    {
        public Boolean key;
        public DayUserControl_SendMoveEventAfterClick(Boolean key)
        {
            this.key = key;
        }
    }

    /// <summary>
    /// Класс событие для отправки к главному классу событие об выделении нового события в DayControl
    /// </summary>
    public class DayUserControl_SendMoveEventAfterClick_Event
    {
        public static event EventHandler<DayUserControl_SendMoveEventAfterClick> GetEvent;
        public void SendEvent(Boolean key)
        {
            if (GetEvent != null)
                GetEvent(this, new DayUserControl_SendMoveEventAfterClick(key));
        }
    }

    public class GetCurrentDateToDayControl: EventArgs
    {
        public DateTime date { get; private set; }
        public Boolean key { get; private set; }
        public GetCurrentDateToDayControl(DateTime date)
        {
            this.date = date;
        }
        public GetCurrentDateToDayControl(Boolean key)
        {
            this.key = key;
        }
    }

    public class GetCurrentDateToDayControl_Event
    {
        public static event EventHandler<GetCurrentDateToDayControl> GetEvent;
        public static event EventHandler<GetCurrentDateToDayControl> GetDate;
        public void SendEvent(Boolean key)
        {
            if (GetEvent != null)
                GetEvent(this, new GetCurrentDateToDayControl(key));
        }
        public void SendDate(DateTime date)
        {
            if (GetDate != null)
                GetDate(this, new GetCurrentDateToDayControl(date));
        }
    }

    public class EventForClickCalendarButton: EventArgs
    {
        public Int32 day { get; private set; }
        public EventForClickCalendarButton(Int32 day)
        {
            this.day = day;
        }
    }

    /// <summary>
    /// Класс событие для передачи в CalendarLibrary число для нажатия кнопки с данной датой
    /// </summary>
    public class EventForClickCalendarButton_Event
    {
        public static event EventHandler<EventForClickCalendarButton> GetEvent;
        public void SendEvent(Int32 day)
        {
            if (GetEvent != null)
                GetEvent(this, new EventForClickCalendarButton(day));
        }
    }
}
