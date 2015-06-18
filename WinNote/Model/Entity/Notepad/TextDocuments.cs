using EventLibrsries;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinNote.Control.Entity;
using WinNote.Model.Entity.DataBase;
using WinNote.Model.SQL;

namespace WinNote.Model.Entity.Notepad
{
    public class TextDocuments
    {
        SendCreatedTextToNotepad_Events sendTextDoc;
        public void SelectEvents()
        {
            CreateTextDocument_Event.GetEvent += CreateTextDocument_Event_GetEvent;
            sendTextDoc = new SendCreatedTextToNotepad_Events();
        }

        void CreateTextDocument_Event_GetEvent(object sender, CreateTextDocument e)
        {
            DateTime date = DateTime.Now;
            NotepadTextDocuments textDocument = new NotepadTextDocuments()
            {
                CategoryId = e.categoryId,
                Created = String.Format("{0}.{1}.{2} {3}:{4}:{5}",
                                        date.Year,date.Month,date.Day,date.Hour,date.Minute,date.Second),
                Data = "",
                ServerId = Variables.ServerId,
                UserId = Variables.Id
            };
            
            EntityData.EntityDataBase.NotepadTextDocuments.Add(textDocument);
            EntityData.EntityDataBase.SaveChanges();
            
            String created = (from data in EntityData.EntityDataBase.NotepadCategories 
                            where data.CategoryName == e.categoryName && data.Id == e.categoryId 
                            select data).ToList()[0].Created;

            NotepadSQLData.GetInstance.SaveTextDocument(textDocument, created);

            sendTextDoc.SendEvent(GetTextDocuments());
        }

        List<INotepadTextDocuments> GetTextDocuments()
        {
            var documents = (from data in EntityData.EntityDataBase.NotepadTextDocuments
                             where data.UserId == Variables.Id
                             select data).ToList();

            List<INotepadTextDocuments> lst = new List<INotepadTextDocuments>();
            foreach (NotepadTextDocuments doc in documents)
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
