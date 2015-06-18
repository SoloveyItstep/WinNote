using ConnectionToServer;
using EventLibrsries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinNote.Control.Entity;
using WinNote.Model.Entity.DataBase;
using WinNote.Model.SQL;
using WinNote.View;

namespace WinNote.Control.Events
{
    class LoginRegisterEvents
    {
        EntityData entity;
        WorkWithSQLData sql;
        View.View view;
        AfterLogInEvent afterLogin;
        MainView mainView;
        public void GetLoginData()
        {
            view = View.View.view;
            mainView = MainView.mainView;
            afterLogin = new AfterLogInEvent();
            
            GetLoginDataEvent.dataEvent += GetLoginDataEvent_dataEvent;
            RegisterEventData.RegEvent += RegisterEventData_RegEvent;
            entity = EntityData.GetInstance;
            sql = WorkWithSQLData.GetInstance;
            if (!sql.CheckConnection())
                sql.Connect();
        }

        private void RegisterEventData_RegEvent(object sender, RegisterEvent e)
        {
            if (!sql.GetUser(e.data.Email, e.data.Password))
            {
                Users user = new Users(){ Email = e.data.Email, LastName = e.data.LastName, FirstName = e.data.Name, Password = e.data.Password};
                Boolean b = sql.AddUser(user);
            }
        }

        public String GetLogedInUserName()
        {
            List<LogedInUser> logUser = entity.GetLogedInUser();
            Int32 id = logUser[0].UserId;
            Users user = entity.GetUser(id);
            String name = user.LastName;
            return name;
        }

        private void GetLoginDataEvent_dataEvent(object sender, LoginEvent e)
        {
            if(entity.CheckUserExists(e.data.login, e.data.password))
            {
                entity.LogIn(e.data.login, e.data.password);
                view.ResizeToMainMenu();
                afterLogin.SetEvent(true);
                if (mainView == null)
                    mainView = MainView.mainView;
                mainView.SetName_TopPanel(GetLogedInUserName());
            }
            else if(sql.GetUser(e.data.login, e.data.password))
            {
                view.ResizeToMainMenu();
                afterLogin.SetEvent(true);
                if (mainView == null)
                    mainView = MainView.mainView;
                mainView.SetName_TopPanel(GetLogedInUserName());
            }
            else if (!sql.GetUser(e.data.login, e.data.password))
            {
                ConnectionEvent con = new ConnectionEvent();
                con.ConnectionException("Неправильно введены данные");
            }
            else
            {
                ConnectionEvent con = new ConnectionEvent();
                con.ConnectionException("No user exists");
            }
        }

        
    }
}
