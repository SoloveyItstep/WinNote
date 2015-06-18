using ConnectionToServer;
using EventLibrsries;
using IInterfaces;
using NoteLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinNote.Control.Entity;
using WinNote.Model;
using WinNote.Model.Entity;
using WinNote.Model.Entity.DataBase;


namespace WinNote.Model.SQL
{
    public class WorkWithSQLData: MD5_Key
    {
        SaveCalendarEventToAction_Delete_Add_Change SaveToActionTable;
        SendUnfinishedActions sendUnfinishedActions;
        SendBackEventAfterDoAction_Event sendBackAfterDoAction;
        public Boolean key = false;
        private static WorkWithSQLData sqlData { get; set; }
        private WorkWithSQLData()
        {
            EventCalendarInfo.calendarInfoChanged += EventCalendarInfo_calendarInfoChanged;
            SendEventInfoToChange_Event.GetEvent += SendEventInfoToChange_Event_GetEvent;
            SendEventToDeleteEventInfo_Event.GetEvent += SendEventToDeleteEventInfo_Event_GetEvent;
            
        }

        public void SelectEvents()
        {
            sendUnfinishedActions = new SendUnfinishedActions();
            sendUnfinishedActions.SelectEvents();
            sendBackAfterDoAction = new SendBackEventAfterDoAction_Event();
        }

        void SendEventToDeleteEventInfo_Event_GetEvent(object sender, SendEventToDeleteEventInfo e)
        {
            EntityData entity = EntityData.GetInstance;
            String data = entity.SerializeToXml(e.info);
            if (!CheckConnection())
            {
                if (Connect())
                {
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("DELETE FROM `Calendar` WHERE ");
                    strBuilder.Append("`EventName` = '" + e.mainInfo.EventName + "'");
                    strBuilder.Append(" && `Created` = '" + e.mainInfo.created + "';");
                    
                    try
                    {
                        Connection.SetRequest(strBuilder.ToString());
                        if(key == true)
                        {
                            sendBackAfterDoAction.SendEvent(e.mainInfo, "Delete");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
                if (Connection.CheckConnection() != "open")
                {
                    if (SaveToActionTable == null)
                        SaveToActionTable = SaveCalendarEventToAction_Delete_Add_Change.GetInstance;
                    SaveToActionTable.Save(e.mainInfo, e.info, "Delete");
                }
            }
            else
            {
                Connection.CloseConnection();
                Connection.Connect();
                Thread.Sleep(25);
                if (Connection.CheckConnection() != "open")
                {
                    
                }
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("DELETE FROM `Calendar` WHERE ");
                strBuilder.Append("`EventName` = '" + e.mainInfo.EventName + "'");
                strBuilder.Append(" && `Created` = '" + e.mainInfo.created + "';");
                try
                {
                    Connection.SetRequest(strBuilder.ToString());
                    if (key == true)
                    {
                        sendBackAfterDoAction.SendEvent(e.mainInfo, "Delete");
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
            if(Connection.CheckConnection() == "open")
                Connection.CloseConnection();
        }

        void SendEventInfoToChange_Event_GetEvent(object sender, SendEventInfoToChange e)
        {
            if (!CheckConnection())
            {
                if (Connect())
                {
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("UPDATE `Calendar` SET ");
                    strBuilder.Append("`EventName`='" + e.eventMainInfo.EventName+"'");
                    strBuilder.Append(",`DateFrom`='" + e.eventMainInfo.dateFrom + "'");
                    strBuilder.Append(",`DateTo`='" + e.eventMainInfo.dateTo + "'");
                    strBuilder.Append(",`Data`='" + e.eventMainInfo.Data + "'");
                    strBuilder.Append(" WHERE `Created`='" + e.eventMainInfo.created + "';");

                   try
                    {
                        Connection.SetRequest(strBuilder.ToString());
                        if (key == true)
                        {
                            sendBackAfterDoAction.SendEvent(e.eventMainInfo, "Update");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
                if (Connection.CheckConnection() != "open")
                {
                    if (SaveToActionTable == null)
                        SaveToActionTable = SaveCalendarEventToAction_Delete_Add_Change.GetInstance;
                    SaveToActionTable.Save(e.eventMainInfo, "Update");
                }
            }
            else
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("UPDATE `Calendar` SET ");
                strBuilder.Append("`EventName`='" + e.eventMainInfo.EventName + "'");
                strBuilder.Append(",`DateFrom`='" + e.eventMainInfo.dateFrom + "'");
                strBuilder.Append(",`DateTo`='" + e.eventMainInfo.dateTo + "'");
                strBuilder.Append(",`Data`='" + e.eventMainInfo.Data + "'");
                strBuilder.Append(" WHERE `Created`='" + e.eventMainInfo.created + "';");
                try
                {
                    Connection.SetRequest(strBuilder.ToString());
                    if (key == true)
                    {
                        sendBackAfterDoAction.SendEvent(e.eventMainInfo, "Update");
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
            Connection.CloseConnection();
        }

        public void EventCalendarInfo_calendarInfoChanged(object sender, EventLibrsries.EventInfo e)
        {
            if (!CheckConnection())
            {
                if (Connect())
                {
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("INSERT INTO `Calendar` (`UserId`, `EventName`, `DateFrom`, `DateTo`,`Data`, `ShareKey`, `Created`) VALUES ");
                    strBuilder.Append("('"+ e.mainInfo.ServerId +"','" + e.mainInfo.EventName + "', '" + e.mainInfo.dateFrom + "', '" + e.mainInfo.dateTo);
                    strBuilder.Append("', '" + e.data + "','"+ e.mainInfo.ShareKey +"','" + e.mainInfo.created + "')");
                    try
                    {
                        Connection.SetRequest(strBuilder.ToString());
                        if (key == true)
                        {
                            sendBackAfterDoAction.SendEvent(e.mainInfo, "Insert");
                        }
                    }
                    catch(Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
                if (Connection.CheckConnection() != "open")
                {
                    IEventMainInfo mainInfo = e.mainInfo;
                    mainInfo.Data = e.data;
                    if (SaveToActionTable == null)
                        SaveToActionTable = SaveCalendarEventToAction_Delete_Add_Change.GetInstance;
                    SaveToActionTable.Save(mainInfo, "Insert");
                }
            }
            else
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("INSERT INTO `Calendar` (`UserId`, `EventName`, `DateFrom`, `DateTo`,`Data`, `ShareKey`, `Created`) VALUES ");
                strBuilder.Append("('" + e.mainInfo.ServerId + "','" + e.mainInfo.EventName + "', '" + e.mainInfo.dateFrom + "', '" + e.mainInfo.dateTo);
                strBuilder.Append("', '" + e.data + "','" + e.mainInfo.ShareKey + "','" + e.mainInfo.created + "')");
                try
                {
                    Connection.SetRequest(strBuilder.ToString());
                    if (key == true)
                    {
                        sendBackAfterDoAction.SendEvent(e.mainInfo, "Insert");
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
            Connection.CloseConnection();
        }

        public static WorkWithSQLData GetInstance 
        {
            get
            {
                if (sqlData == null)
                    sqlData = new WorkWithSQLData();
                return sqlData;
            }
            set
            {
                sqlData = value;
            }
        }

        public Boolean CheckConnection()
        {
            String connectStr = Connection.CheckConnection();
            if(connectStr == "null connection" || connectStr == "closed")
                return false;
            return true;
        }

        public Boolean Connect()
        {
            Boolean key = true;
            if (Connection.CheckConnection() != "open")
            {
                try
                {
                    Connection.Connect();
                }
                catch 
                {
                    ConnectionEvent con = new ConnectionEvent();
                    con.ConnectionException("No connection to server");
                    key = false;
                }
            }
            return key;
        }

        public String GeneratePassword(String password)
        {
            return MD5_Create_Key(password);
        }

        public Boolean GetUser(String email, String password)
        {
            if(Connection.CheckConnection() == "null connection" || Connection.CheckConnection() == "closed")
            {
                if (!Connect())
                    return false;
            }
            
            Boolean loginKey = false;
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("Select * from Users where (`email` = '" + email + "') and (`password` = '");
            String pass = MD5_Create_Key(password);
            strBuilder.Append(pass+"')");

            DataSet dataSet = Connection.GetRequest(strBuilder.ToString());

            foreach (DataTable table in dataSet.Tables)
            {
                foreach(DataRow row in table.Rows)
                {
                    if(row["email"].ToString() == email && pass == row["password"].ToString())
                    {
                        loginKey = true;
                        EntityData.EntityDataBase.Users.Add(new Users()
                        {
                            LastName = row["lastName"].ToString(),
                            FirstName = row["firstName"].ToString(),
                            Email = row["email"].ToString(),
                            Password = row["password"].ToString(),
                            ServerId = Int32.Parse(row["id"].ToString())
                        });
                        Variables.Id = Int32.Parse(row["id"].ToString());
                        try
                        {
                            EntityData.EntityDataBase.SaveChanges();
                        }
                        catch(DbUpdateException ex)
                        {
                            global::System.Windows.MessageBox.Show(ex.Message);
                        }
                        List<Users> user = (from data in EntityData.EntityDataBase.Users
                                    where data.Email == email && data.Password == pass
                                    select data).ToList();
                        foreach(Users us in user) 
                        {
                            EntityData.EntityDataBase.LogedInUsers.Add(new LogedInUser() { UserId = us.Id });
                        };
                        try
                        {
                            EntityData.EntityDataBase.SaveChanges();
                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                               global::System.Windows.MessageBox.Show(String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State));
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    global::System.Windows.MessageBox.Show(String.Format("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage));
                                }

                            }
                            throw;
                        }
                    }
                }
            }

            return loginKey;
        }

        public Boolean AddUser(Users user)
        {
            if (CheckEmailToRegister(user.Email))
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("INSERT INTO `Users` (`lastName`, `firstName`, `email`,`password`) VALUES ");
                String pas = new MD5_Key().MD5_Create_Key(user.Password);
                strBuilder.Append("('" + user.LastName + "', '" + user.FirstName + "', '" + user.Email + "', '" + pas + "')");
                if (!Connection.SetRequest(strBuilder.ToString()))
                {
                    return false;
                }
                else
                {
                    EntityData.EntityDataBase.Users.Add(new Users() { FirstName = user.FirstName, LastName = user.LastName,
                                                        Email = user.Email, Password = pas });
                    EntityData.EntityDataBase.SaveChanges();

                    var users = (from data in EntityData.EntityDataBase.Users
                                 where data.LastName == user.LastName && data.FirstName == user.FirstName && data.Email == user.Email
                                 select new
                                 {
                                     Id = data.Id
                                 }).ToList();

                    foreach (var v in users)
                    {
                        EntityData.EntityDataBase.LogedInUsers.Add(new LogedInUser() { UserId = v.Id });
                        EntityData.EntityDataBase.SaveChanges();
                    }
               }

                return true;
            }

            return false;
        }

        private Boolean CheckEmailToRegister(String email)
        {
            if (Connection.CheckConnection() == "null connection" || Connection.CheckConnection() == "closed")
            {
                if (!Connect())
                    return false;
            }

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("Select * from Users where (`email` = '" + email + "')");

            DataSet dataSet = Connection.GetRequest(strBuilder.ToString());

            foreach (DataTable table in dataSet.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    if (row["email"].ToString() == email)
                        return false;
                    else
                        return true;
                }
            }
            return true;
        }

        public DataSet GetRequest(String request)
        {
            DataTable table = new DataTable();
            if (Connection.CheckConnection() == "null connection" || Connection.CheckConnection() == "closed")
            {
                if (!Connect())
                    return null;
            }

            return Connection.GetRequest(request);
        }
    }
}
