using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventLibrsries
{
    public class EnumCleanEvent
    {
        public enum CleanClasses { All, Minimum }
    }

    public class CleanEvent: EventArgs
    {
        private EnumCleanEvent.CleanClasses cleanClasses { get; set; }
        public CleanEvent(EnumCleanEvent.CleanClasses cleanClasses)
        {
            this.cleanClasses = cleanClasses;
        }
    }

    public class CleanEvent_Event
    {
        public static event EventHandler<CleanEvent> GetEvent;
        public void SendEvent(EnumCleanEvent.CleanClasses cleanClasses)
        {
            if (GetEvent != null)
                GetEvent(this, new CleanEvent(cleanClasses));
        }
    }
}
