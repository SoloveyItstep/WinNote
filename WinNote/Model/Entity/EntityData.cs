using EventLibrsries;
using IInterfaces;
using NoteLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WinNote.Model.Entity.DataBase;
using System.Data.Entity.SqlServer;
using WinNote.Model;
using System.Text.RegularExpressions;
using WinNote.Model.Entity;
using WinNote.Model.Entity.Notepad;

namespace WinNote.Control.Entity
{
    public class EntityData: MD5_Key
    {
        public static WinEntities EntityDataBase { get; set; }
        public static EntityData entityData;
        EventCalendarInfo inf;
        CalendarDeserialisation deserialization;
        CalendarEventsFromControls calendarEventsForControl;
        SaveCalendarEventToAction_Delete_Add_Change sendActionEvents;
        CheckEventsClass checkEvent;
        ChangeEvent changeEvent;
        DeleteEvent deleteEvent;
        CheckEventsClass checkEvents;
        CreateNotepadStandartNotes notepad;
        SendNotepadData getNotepadData;
        
        static String Name { get; set; }
        static Int32 Id { get; set; }

        private EntityData() 
        {
            if (EntityDataBase == null)
                EntityDataBase = new WinEntities();
            CalendarEventInfo.eventData += CalendarEventInfo_eventData;
            deserialization = new CalendarDeserialisation(EntityDataBase);
            calendarEventsForControl = new CalendarEventsFromControls();
            calendarEventsForControl.Events();
            changeEvent = new ChangeEvent(this);
            changeEvent.SetEvents();
            deleteEvent = new DeleteEvent();
            sendActionEvents = SaveCalendarEventToAction_Delete_Add_Change.GetInstance;
            checkEvent = new CheckEventsClass();
            checkEvents = new CheckEventsClass();
            notepad = new CreateNotepadStandartNotes();
            notepad.SelectEvents();
            getNotepadData = new SendNotepadData();
            getNotepadData.SelectEvents();
            checkEvents.SetTimer();
            new TextDocuments().SelectEvents();
        }

        void CalendarEventInfo_eventData(object sender, CalendarEventData e)
        {
            SetCalendarNewEvent(e.eventInfo, e.eventMainInfo);
        }

        public List<LogedInUser> GetLogedInUser()
        {
            List<LogedInUser> logUser = (from data in EntityDataBase.LogedInUsers select data).ToList();
            return logUser;
        }

        public static EntityData GetInstance
        {
            get
            {
                if (entityData == null)
                    entityData = new EntityData();
                return entityData;
            }
            set
            {
                entityData = value;
            }
        }

        public List<Users> GetUserList()
        {
            return (from data in EntityData.EntityDataBase.Users select data).ToList();
        }

        public Users GetUser(Int32 Id)
        {
            return (from data in EntityDataBase.Users where data.Id == Id select data).ToList()[0];
        }

        public void AddUser(Users user)
        {
            String password = MD5_Create_Key(user.Password);
            Users usertmp = new Users() { Password = password, Email = user.Email, 
                                        FirstName = user.FirstName, LastName = user.LastName, ServerId = user.ServerId };
            EntityDataBase.Users.Add(usertmp);
            EntityDataBase.SaveChanges();
        }

        public void LogOut()
        {
            List<LogedInUser> lst = (from data in EntityDataBase.LogedInUsers select data).ToList();
            foreach (LogedInUser logInUser in lst)
            {
                EntityDataBase.LogedInUsers.Remove(logInUser);
                EntityDataBase.SaveChanges();
                Variables.Id = 0;
                Variables.Email = "";
                Variables.Name = "";
            }
        }

        public void LogIn(String email, String password)
        {
            String pass =  MD5_Create_Key(password);
            List<Users> usertmp = (from data in EntityDataBase.Users where data.Email == email && data.Password == pass select data).ToList();
            foreach (Users user in usertmp)
            {
                EntityDataBase.LogedInUsers.Add(new LogedInUser(){ UserId = user.Id });
                EntityDataBase.SaveChanges();
                Name = user.LastName + " " + user.FirstName;
                Id = user.Id;
                Variables.Email = user.Email;
                Variables.Name = user.FirstName;
                Variables.Id = user.Id;
                Variables.ServerId = user.ServerId;
            }


        }

        public Boolean CheckUserExists(String email, String password)
        {
            String pass = MD5_Create_Key(password);
            List<Users> userList = (from data in EntityDataBase.Users
                                   where data.Email == email &&
                                       data.Password == pass
                                   select data).ToList();
            foreach(Users us in userList)
                return true;

            return false;
        }

        public void SetCalendarNewEvent(IEventInfo info, IEventMainInfo mainInfo)
        {
            String strXml = SerializeToXml(info);

            mainInfo.ServerId = GetServerUserId();
            DateTime now = DateTime.Now;

            if(Variables.Id == 0)
            {
                try
                {
                    List<LogedInUser> logedInUser = (from data in EntityDataBase.LogedInUsers select data).ToList();
                    if (logedInUser.Count > 0)
                        Variables.Id = logedInUser[0].UserId;
                }
                catch
                {

                }
            }

            EntityDataBase.Calendars.Add(new Model.Entity.DataBase.Calendar()
            {
                Data = strXml,
                DateFrom = mainInfo.dateFrom.Year + "." + mainInfo.dateFrom.Month + "." + mainInfo.dateFrom.Day + " " + mainInfo.dateFrom.Hour + ":" + mainInfo.dateFrom.Minute + ":" + mainInfo.dateFrom.Second,
                DateTo = mainInfo.dateFrom.Year + "." + mainInfo.dateTo.Month + "." + mainInfo.dateTo.Day + " " + mainInfo.dateTo.Hour + ":" + mainInfo.dateTo.Minute + ":" + mainInfo.dateTo.Second,
                EventName = mainInfo.EventName,
                ShareKey = mainInfo.ShareKey,
                ServerId = mainInfo.ServerId,
                Created = mainInfo.created.Year + "." + mainInfo.created.Month + "." + mainInfo.created.Day + " " + mainInfo.created.Hour + ":" + mainInfo.created.Minute + ":" + mainInfo.created.Second,
                UserId = Variables.Id
            });

            EntityDataBase.SaveChanges();

            inf = new EventCalendarInfo();
            inf.CalendarEventChanged(mainInfo, strXml);
        }

        public Int32 GetServerUserId()
        {
            var user = (from data in EntityDataBase.LogedInUsers select data).ToList();
            if (user.Count == 0)
                return 0;
            Int32 id = (from data in EntityDataBase.LogedInUsers select data).ToList()[0].UserId;
            Int32 userId = (from data in EntityDataBase.Users where data.Id == id select data).ToList()[0].ServerId;
            if (Variables.Id == 0)
                Variables.Id = id;
            if (Variables.ServerId == 0)
                Variables.ServerId = userId;

            return userId;
        }

        public string SerializeToXml(IEventInfo info)
        {
            if (info != null)
            {
                StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
                XmlSerializer serializer = new XmlSerializer(info.GetType());
                serializer.Serialize(writer, info);
                return writer.ToString();
            }
            return "";
        }
    }

    
}
