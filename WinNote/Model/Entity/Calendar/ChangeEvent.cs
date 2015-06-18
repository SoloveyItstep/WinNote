using EventLibrsries;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinNote.Control.Entity;

namespace WinNote.Model.Entity
{
    public class ChangeEvent
    {
        EntityData entityData;
        IEventMainInfo eventMainInfo;
        IEventInfo eventInfo;
        //EntityData.EntityDataBase - БД
        public ChangeEvent(EntityData entityData)
        {
            this.entityData = entityData;
        }
        
        public void SetEvents()
        {
            SendEventInfoToChange_Event.GetEvent += SendEventInfoToChange_Event_GetEvent;
        }

        void SendEventInfoToChange_Event_GetEvent(object sender, SendEventInfoToChange e)
        {
            this.eventMainInfo = e.eventMainInfo;
            this.eventInfo = e.eventInfo;
            SaveChanges();
        }

        void SaveChanges()
        {
            eventMainInfo.Data = entityData.SerializeToXml(eventInfo);

            String createdDate = eventMainInfo.created.Year + "." + eventMainInfo.created.Month + "." + eventMainInfo.created.Day + " " +
                                    eventMainInfo.created.Hour + ":" + eventMainInfo.created.Minute + ":" + eventMainInfo.created.Second;
            var originalEvent = EntityData.EntityDataBase.Calendars.FirstOrDefault(data => 
                                data.Created == createdDate);
            if(originalEvent != null)
            {
                originalEvent.DateFrom = eventMainInfo.dateFrom.Year + "." + eventMainInfo.dateFrom.Month + "." + eventMainInfo.dateFrom.Day + " " + eventMainInfo.dateFrom.Hour + ":" + eventMainInfo.dateFrom.Minute + ":" + eventMainInfo.dateFrom.Second;
                originalEvent.DateTo = eventMainInfo.dateFrom.Year + "." + eventMainInfo.dateTo.Month + "." + eventMainInfo.dateTo.Day + " " + eventMainInfo.dateTo.Hour + ":" + eventMainInfo.dateTo.Minute + ":" + eventMainInfo.dateTo.Second;
                originalEvent.EventName = eventMainInfo.EventName;
                originalEvent.ShareKey = eventMainInfo.ShareKey;
                originalEvent.ServerId = eventMainInfo.ServerId;
                originalEvent.Data = eventMainInfo.Data;
                EntityData.EntityDataBase.SaveChanges();
            }
        }
        
    }
}
