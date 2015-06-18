using Day_UserControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Day_UserControl.VisualStyles
{
    internal class VisualStyle
    {
        DayUserControl control;
        Variables variables;

        private static VisualStyle visual { get; set; }
        private VisualStyle(DayUserControl control)
        {
            this.control = control;
            variables = Variables.GetInstance;
        }

        public static VisualStyle GetInstance(DayUserControl control)
        {
            if (visual == null)
                visual = new VisualStyle(control);
            return visual;
        }

        public void FillByColumnsAndRows()
        {
            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new System.Windows.GridLength(50);
            ColumnDefinition col2 = new ColumnDefinition();

            control.GridColumnsRows.ColumnDefinitions.Add(col1);
            control.GridColumnsRows.ColumnDefinitions.Add(col2);

            RowDefinition mainRow = new RowDefinition();
            mainRow.Height = new System.Windows.GridLength(50);
            control.GridColumnsRows.RowDefinitions.Add(mainRow);

            for(Int32 count = 0; count < 48; ++count)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new System.Windows.GridLength(25);
                control.GridColumnsRows.RowDefinitions.Add(row);
            }
        }

        public void FillLeftColumnsByTime()
        {
            for(Int32 count = 1; count < 49; ++count)
            {
                Label label = new Label();
                label.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                label.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                label.Foreground = Brushes.Gray;
                label.FontSize = 13;
                label.FontWeight = FontWeight.FromOpenTypeWeight(10);
                if((count- 1) % 2 == 0)
                    label.Content = String.Format("{0}:00",((count - 1) / 2));
                else if((count - 1) % 2 == 1)
                    label.Content = String.Format("{0}:30", ((count - 1) / 2));

                Grid.SetRow(label, count);
                Grid.SetColumn(label,0);
                control.GridColumnsRows.Children.Add(label);
            }
        }

        public void FillRightColumnByGrids()
        {
            for(Int32 row = 1; row < 49; ++row)
            {
                Grid grid = new Grid();
                grid.Background = Brushes.White;
                grid.Name = String.Format("g{0}",row);
                
                Grid.SetColumn(grid, 1);
                Grid.SetRow(grid, row);
                control.GridColumnsRows.Children.Add(grid);
                variables.gridList.Add(grid);

                Rectangle rectangle = new Rectangle();
                rectangle.Stroke = Brushes.LightGray;
                rectangle.StrokeThickness = 1;
                Grid.SetRow(rectangle, row);
                Grid.SetColumn(rectangle, 1);
                control.GridColumnsRows.Children.Add(rectangle);
            }

            Label label = new Label();
            label.Foreground = Brushes.Black;
            label.FontSize = 14;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.Content = "Понедельник, 1";
            variables.TopGridLabel = label;

            Grid.SetRow(label, 0);
            Grid.SetColumn(label,0);
            Grid.SetColumnSpan(label, 2);
            control.GridColumnsRows.Children.Add(label);
        }

        public void FillGridsByHyperlinks()
        {
            foreach(Grid grid in variables.gridList)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.MaxWidth = 150;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Center;

                Hyperlink hyperlink = new Hyperlink();
                hyperlink.Name = "h" + grid.Name.Substring(1);
                textBlock.Inlines.Add(hyperlink);
                variables.hyperlinkList.Add(hyperlink);

                grid.Children.Add(textBlock);
            }
        }
    }
}
