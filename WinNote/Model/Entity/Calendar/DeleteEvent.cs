using EventLibrsries;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinNote.Control.Entity;

namespace WinNote.Model.Entity.DataBase
{
    public class DeleteEvent
    {
        EntityData entityData;
        IEventMainInfo mainInfo;
        public DeleteEvent()
        {
            SendEventToDeleteEventInfo_Event.GetEvent += SendEventToDeleteEventInfo_Event_GetEvent;
        }

        void SendEventToDeleteEventInfo_Event_GetEvent(object sender, SendEventToDeleteEventInfo e)
        {
            if(entityData == null)
                entityData = EntityData.GetInstance;
            mainInfo = e.mainInfo;
            Delete();
        }

        public void Delete()
        {
            if (mainInfo == null)
                return;
            String created = String.Format("{0}.{1}.{2} {3}:{4}:{5}",mainInfo.created.Year,mainInfo.created.Month,mainInfo.created.Day,
                                            mainInfo.created.Hour,mainInfo.created.Minute,mainInfo.created.Second);
            var enstance = EntityData.EntityDataBase.Calendars.FirstOrDefault(x =>
                    x.EventName == mainInfo.EventName && x.Created == created);
            if (enstance != null)
            {
                EntityData.EntityDataBase.Calendars.Remove(enstance);
                EntityData.EntityDataBase.SaveChanges();
            }
        }
    }
}
