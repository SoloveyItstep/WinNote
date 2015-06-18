using EventLibrsries;
using IInterfaces;
using NotePad_UserControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NotePad_UserControl.View
{
    public class ShowData
    {
        Variables variables;
        NotePad_Control control;
        NotepadColor color;
        ImagesSource imagesSource;
        CategoriesVariables categoryVar;
        TextVariables textVar;
        NotepadToTopMenu_Event notepadTopMenu;

        private static ShowData showData { get; set; }
        private ShowData(NotePad_Control control)
        {
            variables = Variables.GetInstance; 
            this.control = control;
            color = NotepadColor.GetInstance;
            imagesSource = new ImagesSource();
            categoryVar = CategoriesVariables.GetInstance;
            textVar = TextVariables.GetInstance;
            notepadTopMenu = new NotepadToTopMenu_Event();
        }

        public static ShowData GetInstance(NotePad_Control control)
        {
            if (showData == null)
                showData = new ShowData(control);
            return showData;
        }

        public void ShowCategories()
        {
            notepadTopMenu.SendEvent("Categories");
            control.MainWrapPanel.Children.Clear();
            categoryVar.borderList.Clear();
            categoryVar.gridList.Clear();
            categoryVar.labelDictionary.Clear();
            
    //Categories
            Int32 count = 0;
            foreach(INotepadCategories category in variables.categoriesList)
            {
                Border border = GetNewBorder();
                categoryVar.borderList.Add(border);

                Image img = GetNewImage(imagesSource.GetImageSource(Images.notebook_icon_grey));
                
                Label label = GetNewLabel(category.CategoryName);
                label.Name = "l" + count;
                categoryVar.labelDictionary.Add(count.ToString(),label);

                Grid grid = GetNewGrid();
                grid.Name = "g" + count;
                categoryVar.gridList.Add(grid);

                grid.Children.Add(label);
                grid.Children.Add(img);
                border.Child = grid;
                control.MainWrapPanel.Children.Add(border);

                ++count;
            }
    //Add Category
            Border bor = GetNewBorder();
            bor.Name = "AddCategory";
            categoryVar.borderList.Add(bor);

            Image im = GetNewImage(imagesSource.GetImageSource(Images.Add_grey));
            
            Label lab = GetNewLabel("Добавить категорию");
            categoryVar.labelDictionary.Add("-1",lab);

            Grid gr = GetNewGrid();
            gr.Name = "AddGrid";
            categoryVar.gridList.Add(gr);

            gr.Children.Add(lab);
            gr.Children.Add(im);
            bor.Child = gr;
            control.MainWrapPanel.Children.Add(bor);
        }

        Grid GetNewGrid()
        {
            Grid grid = new Grid();
            grid.Background = color.GetCategoryBackgroundColor();
            return grid;
        }
        Label GetNewLabel(String content)
        {
            Label label = new Label();
            label.Foreground = Brushes.Gray;
            label.FontWeight = FontWeights.Bold;
            label.FontSize = 13;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.Content = content;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.Margin = new Thickness(0, 5, 0, 0);
            return label;
        }
        Image GetNewImage(ImageSource source)
        {
            Image img = new Image();
            img.Source = source;
            img.Height = 66;
            img.Width = 66;
            img.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            img.Margin = new System.Windows.Thickness(0, 50, 0, 0);
            return img;
        }
        Border GetNewBorder()
        {
            Border border = new Border();
            border.Width = 150;
            border.Height = 150;
            border.BorderThickness = new System.Windows.Thickness(1);
            border.BorderBrush = Brushes.Transparent;
            border.Margin = new System.Windows.Thickness(25);
            border.Padding = new System.Windows.Thickness(2);
            border.CornerRadius = new System.Windows.CornerRadius(5);
            border.Background = color.GetCategoryBackgroundColor();

            return border;
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

    //TODO: текстовые документы
        public void ShowTextDocuments()
        {
            textVar.labelDictionary.Clear();
            textVar.borderList.Clear();
            textVar.gridList.Clear();
            control.Grid_TextPanel.Children.Clear();

            var textDocuments = variables.textDocumentsList.FindAll(x => x.CategoryId == variables.currentCategoryId);
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

            MouseEvents.GetInstance(null).TextDocumentsMouseEnterAndLeave();
        }
    }
}
