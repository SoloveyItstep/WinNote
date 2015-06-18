using EventLibrsries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinNote.Control.Entity;
using WinNote.Model.Entity.DataBase;

namespace WinNote.Model.Entity.Notepad
{
    public class CreateNotepadStandartNotes
    {
        public CreateNotepadStandartNotes()
        {
            
        }

        public void SelectEvents()
        {
            CreateStandartCategoriesTextDocuments_Event.GetEvent += CreateStandartCategoriesTextDocuments_Event_GetEvent;
        }

        void CreateStandartCategoriesTextDocuments_Event_GetEvent(object sender, CreateStandartCategoriesTextDocuments e)
        {
            FillVariablesClass();

            String[] strArray = new String[5];
            strArray[0] = "Мои записи";
            strArray[1] = "Заметки";
            strArray[2] = "Напоминания";
            strArray[3] = "Документации";
            strArray[4] = "Разное";

            for (Int32 count = 0; count < 5; count++)
            {
                CreateCategory(strArray[count]);
                CreateTextDocument(strArray[count]);
            }
            

        }

        Users GetUser()
        {
            var logedUser = (from data in EntityData.EntityDataBase.LogedInUsers select data).ToList();
            Users user = new Users();
            foreach(var logUs in logedUser)
            {
                var userList = (from data in EntityData.EntityDataBase.Users where data.Id == logUs.UserId select data).ToList();
                foreach (Users us in userList)
                    user = us;
            }

            Variables.Email = user.Email;
            Variables.Id = user.Id;
            Variables.Name = user.FirstName;
            Variables.ServerId = user.ServerId;

            return user;
        }

        void CreateCategory(String name)
        {

            Users user = GetUser();

            var category = new NotepadCategories()
            {
                CategoryName = name,
                Created = String.Format("{0}.{1}.{2} {3}:{4}:{5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                        DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                ServerId = Variables.ServerId,
                UserId = Variables.Id
            };
            EntityData.EntityDataBase.NotepadCategories.Add(category);
            EntityData.EntityDataBase.SaveChanges();
        }

        void CreateTextDocument(String categoryName)
        {
            var category = (from data in EntityData.EntityDataBase.NotepadCategories
                            where data.CategoryName == categoryName
                            select data).ToList()[0];

            var document = new NotepadTextDocuments()
            {
                CategoryId = category.Id,
                Created = String.Format("{0}.{1}.{2} {3}:{4}:{5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                        DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                ServerId = Variables.ServerId,
                UserId = Variables.Id,
                Data = ""
            };

            EntityData.EntityDataBase.NotepadTextDocuments.Add(document);

        }

        void FillVariablesClass()
        {
            EntityData.entityData.GetServerUserId();
        }
    }
}
