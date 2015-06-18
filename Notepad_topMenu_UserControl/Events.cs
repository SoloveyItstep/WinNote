using EventLibrsries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Notepad_topMenu_UserControl
{
    public class Events
    {
        Notepad_topMenu control;
        Notepad_TopMenuButtonsClick_Event sendButtonName;
        //Boolean bold = true, italick = true, underline = true;

        public Events(Notepad_topMenu control)
        {
            this.control = control;
            sendButtonName = new Notepad_TopMenuButtonsClick_Event();
        }

        public void SelectEvents()
        {
            NotepadToTopMenu_Event.GetEvent += NotepadToTopMenu_Event_GetEvent;
            control.Button_AddDocument.Click += Button_AddDocument_Click;
            control.GoToCategoriesMenu.Click += GoToCategoriesMenu_Click;
            control.GoToTextDocumentsMenu.Click += GoToTextDocumentsMenu_Click;
            control.Button_BoldFontFamily.Click += Button_Click;
            control.Button_ItalicFontFamily.Click += Button_Click;
            control.Button_Underlining.Click += Button_Click;
        }

        void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Button button = sender as Button;
            //if(button.Name == "Button_BoldFontFamily")
            //{
            //    if (bold)
            //    {
            //        button.Background = Brushes.Gray;
            //        bold = false;
            //    }
            //    else
            //    {
            //        button.Background = Brushes.LightGray;
            //        bold = true;
            //    }

            //}
            //else if (button.Name == "Button_ItalicFontFamily")
            //{
            //    if (italick)
            //    {
            //        button.Background = Brushes.Gray;
            //        italick = false;
            //    }
            //    else
            //    {
            //        button.Background = Brushes.LightGray;
            //        italick = true;
            //    }
            //}
            //else if (button.Name == "Button_Underlining")
            //{
            //    if (underline)
            //    {
            //        button.Background = Brushes.Gray;
            //        underline = false;
            //    }
            //    else
            //    {
            //        button.Background = Brushes.LightGray;
            //        underline = true;
            //    }
            //}
        }

        void GoToTextDocumentsMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            sendButtonName.SendEvent("GoToTextDocumentsMenu");
            control.TextDocuments.Visibility = System.Windows.Visibility.Visible;
            control.Notepad.Visibility = System.Windows.Visibility.Hidden;
        }

        void GoToCategoriesMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            sendButtonName.SendEvent("GoToCategoriesMenu");
            control.TextDocuments.Visibility = System.Windows.Visibility.Hidden;
            control.Categories.Visibility = System.Windows.Visibility.Visible;
        }

        void Button_AddDocument_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            sendButtonName.SendEvent("AddDocument");
        }

/// <summary>
/// 
/// </summary>
/// <param name="e">Value - Categories, TextDocuments, Notepad</param>
        void NotepadToTopMenu_Event_GetEvent(object sender, NotepadToTopMenu e)
        {
            if(e.value == "Categories")
            {
                control.Categories.Visibility = System.Windows.Visibility.Visible;
                control.TextDocuments.Visibility = System.Windows.Visibility.Hidden;
                control.Notepad.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (e.value == "TextDocuments")
            {
                control.Categories.Visibility = System.Windows.Visibility.Hidden;
                control.TextDocuments.Visibility = System.Windows.Visibility.Visible;
                control.Notepad.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (e.value == "Notepad")
            {
                control.Categories.Visibility = System.Windows.Visibility.Hidden;
                control.TextDocuments.Visibility = System.Windows.Visibility.Hidden;
                control.Notepad.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
