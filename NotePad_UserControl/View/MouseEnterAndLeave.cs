using EventLibrsries;
using NotePad_UserControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NotePad_UserControl.View
{
    public class MouseEvents
    {
        CategoriesVariables categoryVar;
        ImagesSource imagesSource;
        NotepadColor color;
        NotePad_Control control;
        TextVariables textVar;
        String panel;
        CreateTextDocument_Event sendEventToCreateDocument;
        NotepadToTopMenu_Event notepadTopMenu;
        
        private static MouseEvents enterAndLeave { get; set; }
        private MouseEvents(NotePad_Control control) 
        {
            this.control = control;
            categoryVar = CategoriesVariables.GetInstance;
            imagesSource = new ImagesSource();
            color = NotepadColor.GetInstance;
            textVar = TextVariables.GetInstance;
            notepadTopMenu = new NotepadToTopMenu_Event();
            Notepad_TopMenuButtonsClick_Event.GetEvent += Notepad_TopMenuButtonsClick_Event_GetEvent;
            sendEventToCreateDocument = new CreateTextDocument_Event();
            SendCreatedTextToNotepad_Events.GetEvent += SendCreatedTextToNotepad_Events_GetEvent;
        }

        void SendCreatedTextToNotepad_Events_GetEvent(object sender, SendCreatedTextToNotepad e)
        {
            Variables.GetInstance.textDocumentsList = e.notepadTextDocumentsList;
            ShowData.GetInstance(null).ShowTextDocuments();
        }

        private void Notepad_TopMenuButtonsClick_Event_GetEvent(object sender, Notepad_TopMenuButtonsClick e)
        {
            if(e.name == "AddDocument")
            {
                sendEventToCreateDocument.SendEvent(Variables.GetInstance.currentCategoryName, Variables.GetInstance.currentCategoryId);
            }
            else if(e.name == "GoToCategoriesMenu")
            {
                //control.Grid_Textdocument.Visibility = System.Windows.Visibility.Hidden;
                //control.MainWrapPanel.Visibility = System.Windows.Visibility.Visible;
                Opacityanimation("Categories");
            }
            else if(e.name == "GoToTextDocumentsMenu")
            {
                Opacityanimation("TextPanel");
            }
        }

        public static MouseEvents GetInstance(NotePad_Control control)
        {
            if (enterAndLeave == null)
                enterAndLeave = new MouseEvents(control);
            return enterAndLeave;
        }

        public void CategoriesMouseEnterAndLeave()
        {
            foreach (Grid grid in categoryVar.gridList)
            {
                grid.MouseEnter += grid_MouseEnter;
                grid.MouseLeave += grid_MouseLeave;
                grid.MouseLeftButtonDown += grid_MouseLeftButtonDown;
            }
        }

        public void TextDocumentsMouseEnterAndLeave()
        {
            foreach (Grid grid in textVar.gridList)
            {
                grid.MouseEnter += textDoc_MouseEnter;
                grid.MouseLeave += textDoc_MouseLeave;
                grid.MouseLeftButtonDown += textDoc_MouseLeftButtonDown;
            }
        }

        private void textDoc_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
                return;
            notepadTopMenu.SendEvent("Notepad");
            control.Grid_Textdocument.Visibility = System.Windows.Visibility.Visible;
            control.Grid_TextPanel.Visibility = System.Windows.Visibility.Hidden;

            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200));
            control.Grid_Textdocument.BeginAnimation(Grid.OpacityProperty, anim);
        }

        private void textDoc_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            (grid.Parent as Border).BorderBrush = Brushes.Transparent;
        }

        private void textDoc_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            (grid.Parent as Border).BorderBrush = color.GetCategoryBorderHoverColor();
        }

        void grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
                return;

            Grid grid = sender as Grid;
            if (grid.Name != "AddGrid")
            {
                Opacityanimation("TextPanel");
                var variables = Variables.GetInstance;
                variables.currentCategoryName = grid.Children.OfType<Label>().ToList()[0].Content.ToString();
                variables.currentCategoryId = variables.categoriesList.FirstOrDefault
                                (x => 
                                 x.CategoryName == variables.currentCategoryName)
                                 .CategoryId;
                ShowData.GetInstance(null).ShowTextDocuments();
            }
            else
            {

            }
        }

        Grid GetNewGrid()
        {
            Grid grid = new Grid();
            grid.Background = color.GetCategoryBackgroundColor();
            return grid;
        }
        Border GetTextBorder(Double marginTop)
        {
            Border border = new Border();
            border.CornerRadius = new CornerRadius(5);
            border.Background = Brushes.LightGray;
            border.Height = 50;
            border.VerticalAlignment = VerticalAlignment.Top;
            border.BorderThickness = new Thickness(1);
            border.BorderBrush = Brushes.Transparent;
            border.Margin = new Thickness(25, marginTop, 25, 25);
            border.Padding = new Thickness(2);
            border.MinWidth = 400;
            return border;
        }
        Label GetTextLabel()
        {
            Label label = new Label();
            label.Foreground = Brushes.Gray;
            label.FontWeight = FontWeights.Bold;
            label.FontSize = 14;
            label.HorizontalContentAlignment = HorizontalAlignment.Left;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.Margin = new Thickness(25, 0, 0, 0);
            return label;
        }

        void ShowTextDocuments(Int32 categoryId)
        {
            textVar.labelDictionary.Clear();
            textVar.borderList.Clear();
            textVar.gridList.Clear();
            control.Grid_TextPanel.Children.Clear();

            var textDocuments = Variables.GetInstance.textDocumentsList.FindAll(x => x.CategoryId == categoryId);
            Int32 count = 0;
            foreach (var text in textDocuments)
            {
                Border border = GetTextBorder(50 + (60 * count));

                textVar.borderList.Add(border);

                Grid grid = GetNewGrid();
                grid.Background = Brushes.LightGray;
                grid.Name = "g" + count;
                textVar.gridList.Add(grid);

                Label label = GetTextLabel();
                textVar.labelDictionary.Add(count, label);

                grid.Children.Add(label);
                border.Child = grid;

                control.Grid_TextPanel.Children.Add(border);
                count++;
            }

            TextDocumentsMouseEnterAndLeave();
        }

        public void Opacityanimation(String Panel)
        {
            DoubleAnimation animationOpacity;
            if (Panel == "TextPanel")
            {
                notepadTopMenu.SendEvent("TextDocuments");
                panel = "TextPanel";
                control.Grid_TextPanel.Visibility = System.Windows.Visibility.Visible;
                animationOpacity = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(150));
                control.Grid_TextPanel.BeginAnimation(Grid.OpacityProperty, animationOpacity);
                
                animationOpacity = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(151));
                animationOpacity.Completed += animationOpacity_Completed;
                control.MainWrapPanel.BeginAnimation(Grid.OpacityProperty, animationOpacity);
                ShowData.GetInstance(control).ShowTextDocuments();
            }
            else if(Panel == "Categories")
            {
                notepadTopMenu.SendEvent("Categories");
                panel = "Categories";
                control.MainWrapPanel.Visibility = System.Windows.Visibility.Visible;
                animationOpacity = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(150));
                control.MainWrapPanel.BeginAnimation(Grid.OpacityProperty, animationOpacity);

                animationOpacity = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(151));
                animationOpacity.Completed += animationOpacity_Completed;
                control.Grid_TextPanel.BeginAnimation(Grid.OpacityProperty, animationOpacity);               

            }
            else if(Panel == "Notepad")
            {
                panel = "Notepad";
                control.Grid_Textdocument.Visibility = System.Windows.Visibility.Visible;
                animationOpacity = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(150));
                control.Grid_Textdocument.BeginAnimation(Grid.OpacityProperty, animationOpacity);

                animationOpacity = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(151));
                animationOpacity.Completed += animationOpacity_Completed;
                control.Grid_TextPanel.BeginAnimation(Grid.OpacityProperty, animationOpacity);
            }
        }

        void animationOpacity_Completed(object sender, EventArgs e)
        {
            if (panel == "Categories")
            {
                control.Grid_TextPanel.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (panel == "TextPanel")
            {
                control.MainWrapPanel.Visibility = System.Windows.Visibility.Hidden;
                control.Grid_Textdocument.Visibility = Visibility.Hidden;
            }
            else if(panel == "Notepad")
            {
                control.Grid_TextPanel.Visibility = Visibility.Hidden;
            }
        }
        
        void grid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            
            if (grid.Name != "AddGrid")
                grid.Children.OfType<Image>().ToList()[0].Source = imagesSource.GetImageSource(Images.notebook_icon_grey);
            else
                grid.Children.OfType<Image>().ToList()[0].Source = imagesSource.GetImageSource(Images.Add_grey);

            grid.Children.OfType<Label>().ToList()[0].Foreground = Brushes.Gray;
            (grid.Parent as Border).BorderBrush = Brushes.Transparent;
        }

        void grid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            
            if (grid.Name != "AddGrid")
                grid.Children.OfType<Image>().ToList()[0].Source = imagesSource.GetImageSource(Images.notebook_icon_blue);
            else
                grid.Children.OfType<Image>().ToList()[0].Source = imagesSource.GetImageSource(Images.Add_blue);
            
            grid.Children.OfType<Label>().ToList()[0].Foreground = color.GetCategoryBorderHoverColor();
            (grid.Parent as Border).BorderBrush = color.GetCategoryBorderHoverColor();
        }

    }
}
