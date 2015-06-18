using EventLibrsries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinNote.Model.SQL
{
    public class SendUnfinishedActions
    {
        WorkWithSQLData SQData;
        EventCalendarInfo insertInfo;
        SendEventInfoToChange_Event updateInfo;
        SendEventToDeleteEventInfo_Event deleteInfo;

        public SendUnfinishedActions()
        {
            SQData = WorkWithSQLData.GetInstance;
            insertInfo = new EventCalendarInfo();
            updateInfo = new SendEventInfoToChange_Event();
            deleteInfo = new SendEventToDeleteEventInfo_Event();
        }

        public void SelectEvents()
        {
            EventToDoActions_Event.GetEvent += EventToDoActions_Event_GetEvent;
        }

        void EventToDoActions_Event_GetEvent(object sender, EventToDoActions e)
        {
                switch(e.Value)
                {
                    case "Delete":
                        deleteInfo.Sendinfo(e.mainInfo, e.info);
                        break;
                    case "Update":
                        updateInfo.SendEvent(e.mainInfo, e.info);
                        break;
                    case "Insert":
                        insertInfo.CalendarEventChanged(e.mainInfo, e.mainInfo.Data);
                        break;
                }
        }

        
    }
}
