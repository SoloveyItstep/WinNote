using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarAddMenu_UserControl
{
    public class EventInfo: IEventInfo
    {
        public int AllDay { get; set; }
        public List<Calendarenums.DayOfWeekEnum> DayOfWeek { get; set; }
        public int Remind { get; set; }
        public int Reminded { get; set; }
        public int Repeat { get; set; }
        public string description { get; set; }
        public Calendarenums.EnumRepeat enumRepeat { get; set; }
        public string location { get; set; }
        public Calendarenums.ReminderDate reminderDate { get; set; }
    }

    public class EventMainInfo: IEventMainInfo
    {
        public string Data { get; set; }
        public string EventName { get; set; }
        public int ServerId { get; set; }
        public string ShareKey { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public DateTime created { get; set; }
        
    }
}
