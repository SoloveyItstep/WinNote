using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WeekUserControl
{
    public class VisualStyles
    {
        WeekControl control;
        public VisualVariables variables;
        public HyperKinkLists hyperlinkLists;

        private static VisualStyles visual { get; set; }
        private VisualStyles(WeekControl control)
        {
            this.control = control;
            variables = new VisualVariables();
            hyperlinkLists = new HyperKinkLists();
        }

        public static VisualStyles GetInstance(WeekControl control)
        {
            if (visual == null)
                visual = new VisualStyles(control);
            return visual;
        }

        public void CreateVariables()
        {
            variables.allMiddleGrids = new List<Grid>();
            variables.fridayList = new List<Grid>();
            variables.labelsList = new List<Label>();
            variables.mondayList = new List<Grid>();
            variables.saturdayList = new List<Grid>();
            variables.sundayList = new List<Grid>();
            variables.thirsdayList = new List<Grid>();
            variables.tuesdayList = new List<Grid>();
            variables.wednesdayList = new List<Grid>();

            hyperlinkLists.monday = new List<Hyperlink>();
            hyperlinkLists.tuesday = new List<Hyperlink>();
            hyperlinkLists.wednesday = new List<Hyperlink>();
            hyperlinkLists.thirsday = new List<Hyperlink>();
            hyperlinkLists.friday = new List<Hyperlink>();
            hyperlinkLists.saturday = new List<Hyperlink>();
            hyperlinkLists.sunday = new List<Hyperlink>();
        }

        public void SelectColumnsAndRows()
        {
        //Columns
            ColumnDefinition firstColumn = new ColumnDefinition();
            firstColumn.Width = new System.Windows.GridLength(50);
            control.MainGrid.ColumnDefinitions.Add(firstColumn);

            RowDefinition firstRow = new RowDefinition();
            firstRow.Height = new System.Windows.GridLength(40);
            control.MainGrid.RowDefinitions.Add(firstRow);

            for(int i = 0; i < 7; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.MinWidth = 200;
                control.MainGrid.ColumnDefinitions.Add(column);
            }
            for(int i = 0; i < 48; ++i)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new System.Windows.GridLength(25);
                control.MainGrid.RowDefinitions.Add(row);
            }
        }

        public void SelectBorders()
        {
            //for(int i = 0; i < 8; ++i)
            //{
            //    for(int r = 0; r < 49; ++r)
            //    {
            //        Rectangle rectangle = new Rectangle();
            //        rectangle.Stroke = Brushes.LightGray;
            //        rectangle.StrokeThickness = 1;
            //        Grid.SetRow(rectangle, r);
            //        Grid.SetColumn(rectangle, i);
                    //control.MainGrid.Children.Add(rectangle);
            //    }
            //}
        }

        public void FillColumnsAndRows()
        {
        //Time Grids
            for (int i = 0; i < 48; ++i)
            {
                Label label = new Label();
                label.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                label.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                label.Foreground = Brushes.Gray;
                label.FontSize = 13;

                Int32 hour = i / 2;
                Int32 min = i % 2;
                if (min == 0)
                    label.Content = String.Format("{0}:00",hour);
                else
                    label.Content = String.Format("{0}:30",hour);

                Grid.SetColumn(label, 0);
                Grid.SetRow(label, i + 1);
                control.MainGrid.Children.Add(label);
            }
        }

        public void FillTopRows()
        {
            for(int col = 1; col < 8; ++col)
            {
                Label label = new Label();
                label.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                label.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                label.FontSize = 15;
                label.Foreground = Brushes.Black;
                Grid.SetColumn(label, col);
                Grid.SetRow(label, 0);
                control.MainGrid.Children.Add(label);
                variables.labelsList.Add(label);
            }
        }

        public void FillMiddleRows()
        {
            for(Int32 col = 1; col < 8; col++)
            {
                for(Int32 row = 1; row < 49; ++row)
                {
                    Grid grid = new Grid();
                    grid.Background = Brushes.White;
                    if (row < 10)
                        grid.Name = String.Format("g{0}0{1}",col,row);
                    else
                        grid.Name = String.Format("g{0}{1}", col, row);

                    Grid.SetRow(grid, row);
                    Grid.SetColumn(grid,col);
                    AddToListGrid(col, ref grid);
                    AddTextBlocks(ref grid);

                    Rectangle rectangle = new Rectangle();
                    rectangle.Stroke = Brushes.LightGray;
                    grid.Children.Add(rectangle);

                    control.MainGrid.Children.Add(grid);

                    variables.allMiddleGrids.Add(grid);
                }
            }
        }

        void AddTextBlocks(ref Grid grid)
        {
            TextBlock textBlock = new TextBlock();
            Hyperlink hyperlink = new Hyperlink();

            String name = grid.Name.Substring(1);
            hyperlink.Name = "h" + name;
            //if(name[0] == '1')
            //global::System.Windows.MessageBox.Show(name.Substring(1));

            if (name[0] == '1')
                hyperlinkLists.monday.Add(hyperlink);
            else if(name[0] == '2')
                hyperlinkLists.tuesday.Add(hyperlink);
            else if (name[0] == '3')
                hyperlinkLists.wednesday.Add(hyperlink);
            else if (name[0] == '4')
                hyperlinkLists.thirsday.Add(hyperlink);
            else if (name[0] == '5')
                hyperlinkLists.friday.Add(hyperlink);
            else if (name[0] == '6')
                hyperlinkLists.saturday.Add(hyperlink);
            else if (name[0] == '7')
                hyperlinkLists.sunday.Add(hyperlink);

            textBlock.MaxWidth = 100;
            textBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            textBlock.Margin = new System.Windows.Thickness(0,0,10,0);
            textBlock.Inlines.Add(hyperlink);

            grid.Children.Add(textBlock);
        }

        void AddToListGrid(Int32 column, ref Grid grid)
        {
            switch(column)
            {
                case 1:
                    variables.mondayList.Add(grid);
                    break;
                case 2:
                    variables.tuesdayList.Add(grid);
                    break;
                case 3:
                    variables.wednesdayList.Add(grid);
                    break;
                case 4:
                    variables.thirsdayList.Add(grid);
                    break;
                case 5:
                    variables.fridayList.Add(grid);
                    break;
                case 6:
                    variables.saturdayList.Add(grid);
                    break;
                case 7:
                    variables.sundayList.Add(grid);
                    break;
            }
        }

        public void SelectTopWeekdayNames(Int32[] arr)
        {
            for(Int32 labelNum = 0; labelNum < 7; ++labelNum)
            {
                if (labelNum == 0)
                    variables.labelsList[labelNum].Content = String.Format("Понедельник, {0}",arr[labelNum]);
                else if (labelNum == 1)
                    variables.labelsList[labelNum].Content = String.Format("Вторник, {0}", arr[labelNum]);
                else if (labelNum == 2)
                    variables.labelsList[labelNum].Content = String.Format("Среда, {0}", arr[labelNum]);
                else if (labelNum == 3)
                    variables.labelsList[labelNum].Content = String.Format("Четверг, {0}", arr[labelNum]);
                else if (labelNum == 4)
                    variables.labelsList[labelNum].Content = String.Format("Пятница, {0}", arr[labelNum]);
                else if (labelNum == 5)
                    variables.labelsList[labelNum].Content = String.Format("Суббота, {0}", arr[labelNum]);
                else if (labelNum == 6)
                    variables.labelsList[labelNum].Content = String.Format("Воскресенье, {0}", arr[labelNum]);
            }
        }

        public void ClearHyperlinks()
        {
            foreach(Hyperlink link in hyperlinkLists.monday)
                link.Inlines.Clear();
            foreach (Hyperlink link in hyperlinkLists.tuesday)
                link.Inlines.Clear();
            foreach (Hyperlink link in hyperlinkLists.wednesday)
                link.Inlines.Clear();
            foreach (Hyperlink link in hyperlinkLists.thirsday)
                link.Inlines.Clear();
            foreach (Hyperlink link in hyperlinkLists.friday)
                link.Inlines.Clear();
            foreach (Hyperlink link in hyperlinkLists.saturday)
                link.Inlines.Clear();
            foreach (Hyperlink link in hyperlinkLists.sunday)
                link.Inlines.Clear();
        }

        public void ClearGrids()
        {
            foreach (Grid grid in variables.allMiddleGrids)
                grid.Background = Brushes.White;
        }
    }
}
