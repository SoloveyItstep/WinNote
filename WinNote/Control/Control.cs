using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinNote.Control.Entity;
using WinNote.Control.Events;
using WinNote.Model;
using WinNote.Model.Entity.DataBase;

namespace WinNote.Control
{
    class ControlCS
    {
        private static ControlCS control { get; set; }
        public LoginRegisterEvents LogEvents;
        EntityData entityData;
        MainMenuEvents mainEvents;

        private ControlCS() { }

        public static ControlCS GetInstance
        {
            get
            {
                if (control == null)
                    control = new ControlCS();
                return control;
            }
            set
            {
                control = value;
            }
        }

        public void CreateEvents()
        {
            if(LogEvents == null)
                LogEvents = new LoginRegisterEvents();
            LogEvents.GetLoginData();
            if (entityData == null)
                entityData = EntityData.GetInstance;
            if(mainEvents == null)
                mainEvents = new MainMenuEvents();
            mainEvents.Events();
        }

        public Boolean Check_LogededInUser()
        {
            Boolean key = false;
            if(entityData == null)
                entityData = EntityData.GetInstance;
            List<LogedInUser> logUser = entityData.GetLogedInUser();
            if (logUser.Count > 0)
            {
                Variables.Id = logUser[0].UserId;
                Users user = entityData.GetUser(Variables.Id);
                Variables.Email = user.Email;
                Variables.Name = user.LastName + " " + user.FirstName;
                key = true;
            }
            return key;
        }

        public Boolean LogOut()
        {
            entityData.LogOut();
            return true;
        }
        public String GetLogedInUserName()
        {
            if (LogEvents == null)
                CreateEvents();
            return LogEvents.GetLogedInUserName();
        }
    }
}
