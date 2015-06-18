using EventLibrsries;
using NotePad_UserControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NotePad_UserControl.View
{
    public class VisualStyle
    {
        NotePad_Control control;
        Variables variables;
        ShowData showData;
        MouseEvents mouseEnterLeave;
        SendEventToGetNotepadData_Event sendEventToGetData;
        
        public VisualStyle(NotePad_Control control)
        {
            this.control = control;
            variables = Variables.GetInstance;
            showData = ShowData.GetInstance(control);
            mouseEnterLeave = MouseEvents.GetInstance(control);
            sendEventToGetData = new SendEventToGetNotepadData_Event();
        }

        public void SelectEvents()
        {
            CreateObjects();
            SendDataToNotepad_Events.GetEvent += SendDataToNotepad_Events_GetEvent;
            EventAboutNotepadPanelIsOpening_Event.GetEvent += EventAboutNotepadPanelIsOpening_Event_GetEvent;
        }

        /// <summary>
        /// событие о переключении на панель Notepad
        /// </summary>
        /// <param name="e"></param>
        void EventAboutNotepadPanelIsOpening_Event_GetEvent(object sender, EventAboutNotepadPanelIsOpening e)
        {
            if (variables.categoriesList.Count == 0)
                sendEventToGetData.SendEvent(true);
            else
                Show();
        }

        public void CreateObjects()
        {
            sendEventToGetData = new SendEventToGetNotepadData_Event();
        }

        /// <summary>
        /// Событие полечающее из Model данные List(INotepadCategories) и List(INotepadTextDocuments),
        /// сохраняющий данные в обьект класса Variables
        /// и отображающий полученных данных через метод ShowData()
        /// </summary>
        /// <param name="e">Класс содержащий List(INotepadCategories) и List(INotepadTextDocuments)</param>
        void SendDataToNotepad_Events_GetEvent(object sender, SendDataToNotepad e)
        {
            variables.categoriesList = e.notepadCategoriesList;
            variables.textDocumentsList = e.notepadTextDocumentsList;
            Show();
        }

        /// <summary>
        /// Отображение категорий или текстовых документов
        /// </summary>
        void Show()
        {
            if(control.MainWrapPanel.Visibility == Visibility.Visible)
            {
                showData.ShowCategories();
                mouseEnterLeave.CategoriesMouseEnterAndLeave();
            }
            else if(control.Grid_TextPanel.Visibility == Visibility.Visible)
            {
                mouseEnterLeave.Opacityanimation("Categories");
                showData.ShowTextDocuments();
                mouseEnterLeave.TextDocumentsMouseEnterAndLeave();
            }
        }

    }
}
