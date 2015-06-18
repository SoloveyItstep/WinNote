using ConnectionToServer;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinNote.Model.Entity.DataBase;

namespace WinNote.Model.SQL
{
    public class NotepadSQLData
    {
        WorkWithSQLData sql;
        private static NotepadSQLData notepadSQLData { get; set; }
        private NotepadSQLData() { sql = WorkWithSQLData.GetInstance; }

        public static NotepadSQLData GetInstance
        {
            get
            {
                if (notepadSQLData == null)
                    notepadSQLData = new NotepadSQLData();
                return notepadSQLData;
            }
            set
            {
                notepadSQLData = value;
            }
        }

        public void SaveCategory(NotepadCategories categories)
        {

        }

        public void SaveTextDocument(NotepadTextDocuments document, String categoryCreated)
        {
            if (!sql.CheckConnection())
            {
                if (sql.Connect())
                {
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("INSERT INTO `Calendar` (`UserId`, `CategoryId`, `Data`, `Created`,`CategoryCreated`) VALUES ");
                    strBuilder.Append("('" + document.UserId + "','" + document.CategoryId + "', '" + document.Data + "', '" + document.Created);
                    strBuilder.Append("', '" + categoryCreated + "')");
                    try
                    {
                        Connection.SetRequest(strBuilder.ToString());
                        //sendBackAfterDoAction.SendEvent(e.mainInfo, "Insert");  --- при появлении соединения с сервером
                        
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
                if (Connection.CheckConnection() != "open")
                {
                    
                    //if (SaveToActionTable == null)
                    //    SaveToActionTable = SaveCalendarEventToAction_Delete_Add_Change.GetInstance;
                    //SaveToActionTable.Save(mainInfo, "Insert");  ==== при отсутствии соединения с сервером
                }
            }
            else
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("INSERT INTO `Calendar` (`UserId`, `CategoryId`, `Data`, `Created`,`CategoryCreated`) VALUES ");
                strBuilder.Append("('" + document.UserId + "','" + document.CategoryId + "', '" + document.Data + "', '" + document.Created);
                strBuilder.Append("', '" + categoryCreated + "')");
                try
                {
                    Connection.SetRequest(strBuilder.ToString());
                    //if (key == true)
                    //{
                    //    sendBackAfterDoAction.SendEvent(e.mainInfo, "Insert");
                    //}
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
            Connection.CloseConnection();
        }
    }
}
