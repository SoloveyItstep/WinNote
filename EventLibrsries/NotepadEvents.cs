using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventLibrsries
{
    public class CreateStandartCategoriesTextDocuments: EventArgs
    {
        public Boolean key { get; private set; }
        public CreateStandartCategoriesTextDocuments(Boolean key)
        {
            this.key = key;
        }
    }

    /// <summary>
    /// Класс событие. После регистрации создание стандартных категорий и текстовых документов в Notepad
    /// </summary>
    public class CreateStandartCategoriesTextDocuments_Event
    {
        public static event EventHandler<CreateStandartCategoriesTextDocuments> GetEvent;
        public void SendEvent(Boolean key)
        {
            if (GetEvent != null)
                GetEvent(this, new CreateStandartCategoriesTextDocuments(key));
        }
    }

    public class SendEventToGetNotepadData: EventArgs
    {
        public Boolean key { get; private set; }
        public SendEventToGetNotepadData(Boolean key)
        {
            this.key = key;
        }
    }

    /// <summary>
    /// Класс событие. Передача уведомления в Model для получения данных с базы данных категорий и текстовых документов:
    ///         INotepadCategories и INotepadTextDocuments
    /// </summary>
    public class SendEventToGetNotepadData_Event
    {
        public static event EventHandler<SendEventToGetNotepadData> GetEvent;
        public void SendEvent(Boolean key)
        {
            if (GetEvent != null)
                GetEvent(this, new SendEventToGetNotepadData(key));
        }
    }

    public class SendDataToNotepad: EventArgs
    {
        public List<INotepadCategories> notepadCategoriesList { get; private set; }
        public List<INotepadTextDocuments> notepadTextDocumentsList { get; private set; }
        public SendDataToNotepad(List<INotepadCategories> categoriesList, List<INotepadTextDocuments> textDocumentsList)
        {
            this.notepadCategoriesList = categoriesList;
            this.notepadTextDocumentsList = textDocumentsList;
        }
    }

    /// <summary>
    /// Класс событие. Передача из Model в Notepad данных:
    ///        List INotepadCategories - лист с категориями
    ///        List INotepadTextDocuments - лист с текстовыми документами
    /// </summary>
    public class SendDataToNotepad_Events
    {
        public static event EventHandler<SendDataToNotepad> GetEvent;
        public void SendEvent(List<INotepadCategories> categoriesList, List<INotepadTextDocuments> textDocumentsList)
        {
            if (GetEvent != null)
                GetEvent(this, new SendDataToNotepad(categoriesList, textDocumentsList));
        }
    }

    public class SendCreatedTextToNotepad : EventArgs
    {
        public List<INotepadTextDocuments> notepadTextDocumentsList { get; private set; }
        public SendCreatedTextToNotepad(List<INotepadTextDocuments> textDocumentsList)
        {
            this.notepadTextDocumentsList = textDocumentsList;
        }
    }

    /// <summary>
    /// Класс событие. Передача всех TextDocumenst
    /// </summary>
    public class SendCreatedTextToNotepad_Events
    {
        public static event EventHandler<SendCreatedTextToNotepad> GetEvent;
        public void SendEvent(List<INotepadTextDocuments> textDocumentsList)
        {
            if (GetEvent != null)
                GetEvent(this, new SendCreatedTextToNotepad(textDocumentsList));
        }
    }

    public class NotepadToTopMenu : EventArgs
    {
        public String value { get; private set; }
        public NotepadToTopMenu(String value)
        {
            this.value = value;
        }
    }

    /// <summary>
    /// Класс событие. Передача уведомления для верхней панели кнопок
    /// </summary>
    public class NotepadToTopMenu_Event
    {
        public static event EventHandler<NotepadToTopMenu> GetEvent;
        public void SendEvent(String value)
        {
            if (GetEvent != null)
                GetEvent(this, new NotepadToTopMenu(value));
        }
    }

    public class Notepad_TopMenuButtonsClick: EventArgs
    {
        public String name { get; private set; }
        public Notepad_TopMenuButtonsClick(String name)
        {
            this.name = name;
        }
    }

    /// <summary>
    /// Класс событие. Передача названия нажатой кнопки
    /// </summary>
    public class Notepad_TopMenuButtonsClick_Event
    {
        public static event EventHandler<Notepad_TopMenuButtonsClick> GetEvent;
        public void SendEvent(String name)
        {
            if (GetEvent != null)
                GetEvent(this, new Notepad_TopMenuButtonsClick(name));
        }
    }

    public class CreateTextDocument: EventArgs
    {
        public String categoryName { get; private set; }
        public Int32 categoryId { get; private set; }
        public CreateTextDocument(String categoryName, Int32 categoryId)
        {
            this.categoryName = categoryName;
            this.categoryId = categoryId;
        }
    }

    /// <summary>
    /// Класс событие. Отправка в Model 
    /// </summary>
    public class CreateTextDocument_Event
    {
        public static event EventHandler<CreateTextDocument> GetEvent;
        public void SendEvent(String categoryName, Int32 categoryId)
        {
            if (GetEvent != null)
                GetEvent(this, new CreateTextDocument(categoryName,categoryId));
        }
    }

    public class EventAboutNotepadPanelIsOpening: EventArgs
    {
        public Boolean key { get; private set; }
        public EventAboutNotepadPanelIsOpening(Boolean key)
        {
            this.key = key;
        }
    }

    /// <summary>
    /// Класс событие. отправка уведомления о переключении на панель Notepad
    /// </summary>
    public class EventAboutNotepadPanelIsOpening_Event
    {
        public static event EventHandler<EventAboutNotepadPanelIsOpening> GetEvent;
        public void SendEvent(Boolean key)
        {
            if (GetEvent != null)
                GetEvent(this, new EventAboutNotepadPanelIsOpening(key));
        }
    }
}
