using EventLibrsries;
using MonthUserControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MonthUserControl.Visual
{
    public class VisualStyle
    {
        private static VisualStyle visual { get; set; }
        private MonthControl control;
        public MainVariables variables;
        public MonthUserControl.Model.EventInfo eventInfo;
        GetSelectedDate_Event GetDate;
        public MyPopup mypopup;

        private VisualStyle(MonthControl control)
        {
            this.control = control;
        }

        public static VisualStyle GetInstance(MonthControl control)
        {
            if (visual == null)
                visual = new VisualStyle(control);
            return visual;
        }

        public void InitializeComponents()
        {
            variables = new MainVariables();
            variables.gridList = new List<System.Windows.Controls.Grid>();
            variables.hyperlinkList = new List<System.Windows.Documents.Hyperlink>();
            variables.selectedDate = DateTime.Now;
            variables.labelsDaysNameList = new List<Label>();

            variables.daysHyperlinkDictionary = new Dictionary<string, List<Hyperlink>>();

            eventInfo = new MonthUserControl.Model.EventInfo();
            eventInfo.dictionatyEventInfo = new Dictionary<IInterfaces.IEventMainInfo, IInterfaces.IEventInfo>();
            GetDate = new GetSelectedDate_Event();

            mypopup = new MyPopup();
            mypopup.popupHyperlinkDictionary = new Dictionary<int, Hyperlink>();
            mypopup.hyperlinkIndex = new Dictionary<int, int>();
        }

        public void FillByColumnsAndRows()
        {
            for(Int32 col = 0; col < 7; ++col)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.MinWidth = 200;
                control.MainGrid.ColumnDefinitions.Add(column);
            }

            RowDefinition ro = new RowDefinition();
            ro.Height = new System.Windows.GridLength(40);
            control.MainGrid.RowDefinitions.Add(ro);

            for(Int32 row = 1; row < 6; row++)
            {
                RowDefinition r = new RowDefinition();
                r.Height = new System.Windows.GridLength(150);
                control.MainGrid.RowDefinitions.Add(r);
            }

            RowDefinition row1 = new RowDefinition();
            row1.Height = new System.Windows.GridLength(150);
            row1.Name = "RowSix";
            control.MainGrid.RowDefinitions.Add(row1);
        }

        public void FillByGrids()
        {
            Int32 i = 1;
            for(Int32 r = 1; r < 7; ++r)
            {
                for(Int32 c = 0; c < 7; ++c)
                {
                    Grid grid = new Grid();
                    grid.Name = String.Format("g{0}",i);
                    
                    grid.Background = Brushes.White;
                    variables.gridList.Add(grid);

                    Grid.SetColumn(grid, c);
                    Grid.SetRow(grid, r);
                    control.MainGrid.Children.Add(grid);
                    ++i;
                }
            }

            for (Int32 row = 1; row < 8; ++row)
            {
                for (Int32 column = 0; column < 7; ++column)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Stroke = Brushes.LightGray;
                    rectangle.StrokeThickness = 1;
                    Grid.SetRow(rectangle, row);
                    Grid.SetColumn(rectangle, column);
                    control.MainGrid.Children.Add(rectangle);
                }
            }
        }

        public void FillTopRows()
        {
            List<Label> lst = new List<Label>();
            for(Int32 c = 0; c < 7; ++c)
            {
                Label label = new Label();
                label.FontSize = 15;
                label.Foreground = Brushes.Black;
                label.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                label.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                Grid.SetColumn(label, c);
                Grid.SetRow(label, 0);
                control.MainGrid.Children.Add(label);
                lst.Add(label);
            }

            lst[0].Content = "Понедельник";
            lst[1].Content = "Вторник";
            lst[2].Content = "Среда";
            lst[3].Content = "Четверг";
            lst[4].Content = "Пятница";
            lst[5].Content = "Суббота";
            lst[6].Content = "Воскресенье";
        }

        public void FillGrids()
        {
            Int32 i = 0;
            foreach(Grid grid in variables.gridList)
            {
                ++i;
                Label label = new Label();
                label.FontSize = 14;
                label.Foreground = Brushes.Black;
                label.Name = "l" + grid.Name.Substring(1);

                label.Content = i.ToString();
                label.Margin = new System.Windows.Thickness(10,5,10,0);
                variables.labelsDaysNameList.Add(label);
                grid.Children.Add(label);

                FillGridsByHyperlinks(grid);
            }
        }

        void FillGridsByHyperlinks(Grid grid)
        {
            String name = "d" + grid.Name.Substring(1);
            variables.daysHyperlinkDictionary.Add(name, new List<Hyperlink>());

            for(Int32 j = 0; j < 5; ++j)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.MaxWidth = 175;
                textBlock.Margin = new System.Windows.Thickness(10,(j*20 + 35),10,0);

                Hyperlink hyperlink = new Hyperlink();
                textBlock.Inlines.Add(hyperlink);
                hyperlink.Name = "h" + grid.Name.Substring(1);
                
                variables.daysHyperlinkDictionary[name].Add(hyperlink);

                grid.Children.Add(textBlock);
            }
            
        }

        public void SetDays()
        {
            GetDate.SendEvent(true);
            Thread.Sleep(50);
            Int32[][] monthDays = GetMonthDays();
            Int32 count = monthDays[0].Count();
            if (count == 0)
                count++;
            Int32 currDays = 0, nextDays = 1, countDays = 0;
            for(Int32 i = 0; i < 42; ++i)
            {
                if (i < count - 1)
                {
                    variables.labelsDaysNameList[i].Foreground = Brushes.Gray;
                    variables.labelsDaysNameList[i].Content = monthDays[0][i].ToString();
                    ++countDays;
                }
                else if(i < count + monthDays[1].Count() - 1)
                {
                    variables.labelsDaysNameList[i].Foreground = Brushes.Black;
                    variables.labelsDaysNameList[i].Content = monthDays[1][currDays].ToString();
                    currDays++;
                    ++countDays;
                }
                else
                {
                    if(countDays < 36)
                        control.MainGrid.RowDefinitions[6].Height = new System.Windows.GridLength(0);
                    else
                        control.MainGrid.RowDefinitions[6].Height = new System.Windows.GridLength(150);
                    variables.labelsDaysNameList[i].Foreground = Brushes.Gray;
                    variables.labelsDaysNameList[i].Content = nextDays.ToString();
                    nextDays++;
                }
            }


        }

        Int32[][] GetMonthDays()
        {
            Int32[][] days = new Int32[2][];
            DayOfWeek dayOfWeek = new DateTime(variables.selectedDate.Year, variables.selectedDate.Month, 1).DayOfWeek;
            DateTime prevMonth = variables.selectedDate.AddMonths(-1);
            Int32 daysInPrevMonth = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
            Int32 daysInCurrentMonth = DateTime.DaysInMonth(variables.selectedDate.Year,variables.selectedDate.Month);
            Int32 prevMonthDaysCount = 0;
            if (dayOfWeek == DayOfWeek.Tuesday)
                prevMonthDaysCount = 1;
            else if (dayOfWeek == DayOfWeek.Wednesday)
                prevMonthDaysCount = 2;
            else if (dayOfWeek == DayOfWeek.Thursday)
                prevMonthDaysCount = 3;
            else if (dayOfWeek == DayOfWeek.Friday)
                prevMonthDaysCount = 4;
            else if (dayOfWeek == DayOfWeek.Saturday)
                prevMonthDaysCount = 5;
            else if (dayOfWeek == DayOfWeek.Sunday)
                prevMonthDaysCount = 6;
            if (prevMonthDaysCount > 0)
            {
                days[0] = new Int32[prevMonthDaysCount + 1];
                for (int i = 0; i <= prevMonthDaysCount + 2; ++i)
                {
                    days[0][i] = daysInPrevMonth - prevMonthDaysCount + 1;
                    prevMonthDaysCount--;
                }
            }
            else
                days[0] = new Int32[0];
            days[1] = new Int32[daysInCurrentMonth];

            
            for (Int32 i = 1; i <= daysInCurrentMonth; ++i)
                days[1][i - 1] = i;

            return days;
        }

    }
}
