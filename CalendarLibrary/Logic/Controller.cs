using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarLibrary.Logic
{
    public class Controller
    {
        CalendarControl control;
        Events events;
        public Controller(CalendarControl control)
        {
            this.control = control;
            events = new Events(this.control);
            events.SetEvents();
        }
    }
}
