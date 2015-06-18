using EventLibrsries;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinNote.Control.Entity;
using WinNote.Model.Entity.DataBase;

namespace WinNote.Model.Entity.Notepad
{
    public class SendNotepadData
    {
        /// <summary>
        /// Обьект передающий в NotepadUserControl данные:
        ///         INotepadCategories и INotepadTextDocuments
        /// </summary>
        SendDataToNotepad_Events sendEventToNotepad;
        public void SelectEvents()
        {
            SendEventToGetNotepadData_Event.GetEvent += SendEventToGetNotepadData_Event_GetEvent;
            sendEventToNotepad = new SendDataToNotepad_Events();
        }

        /// <summary>
        /// Событие полученное от Notepad для передачи данных обратно в NotepadUserControl 
        /// </summary>
        /// <param name="e">Класс с Boolean переменной (без привязки и обработки)</param>
        void SendEventToGetNotepadData_Event_GetEvent(object sender, SendEventToGetNotepadData e)
        {
            GetDataAndSendToNotepad();
        }

        /// <summary>
        /// Получение и отправка категорий, текстовых документов и отправка их в NotepadUserControl
        /// через событие SendDataToNotepad_Events
        /// </summary>
        public void GetDataAndSendToNotepad()
        {
            List<INotepadCategories> categories = GetCategories();
            List<INotepadTextDocuments> textDocuments = GetTextDocuments();

            sendEventToNotepad.SendEvent(categories, textDocuments);
        }

        /// <summary>
        /// Получение List(INotepadCategories) - категорий с базы данных
        /// </summary>
        /// <returns>List(INotepadCategories)</returns>
        List<INotepadCategories> GetCategories()
        {
            var categories = (from data in EntityData.EntityDataBase.NotepadCategories
                                            where data.UserId == Variables.Id
                                            select data).ToList();
            List<INotepadCategories> lst = new List<INotepadCategories>();
            foreach (NotepadCategories cat in categories)
            {
                lst.Add(new NotepadCategory()
                    {
                        CategoryName = cat.CategoryName,
                        Created = DateTime.Parse(cat.Created),
                        ServerId = Variables.ServerId,
                        UserId = Variables.Id,
                        CategoryId = cat.Id
                    });
            }

            return lst;
        }

        /// <summary>
        /// Получение List(INotepadTextDocuments) - текстовых документов с бызы данных
        /// </summary>
        /// <returns>List(INotepadTextDocuments)</returns>
        List<INotepadTextDocuments> GetTextDocuments()
        {
            var documents = (from data in EntityData.EntityDataBase.NotepadTextDocuments
                             where data.UserId == Variables.Id
                             select data).ToList();

            List<INotepadTextDocuments> lst = new List<INotepadTextDocuments>();
            foreach(NotepadTextDocuments doc in documents)
            {
                lst.Add(new NotepadTextDocument()
                    {
                        CategoryId = doc.CategoryId,
                        Created = DateTime.Parse(doc.Created),
                        Data = doc.Data,
                        ServerId = doc.ServerId,
                        UserId = doc.UserId,
                        CategoryName = (from data in EntityData.EntityDataBase.NotepadCategories
                                        where data.Id == doc.CategoryId
                                        select data).ToList()[0].CategoryName
                    });
            }

            return lst;
        }
    }
}
