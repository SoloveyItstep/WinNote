using EventLibrsries;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace DayUserControl
{
    public class Events
    {
        private DayControl control;
        public List<Grid> gridList;
        List<RowDefinition> rowList;
        public Boolean mouseMoveKey = false, AddEventMenuVisibility = false;
        public Double currentPositionMouseMove = 0;
        public Int32 StartCountRowHiden = 0, currentCountRowHiden = 0, TopOrUpCount = 0, scrollIndex = 0, mouseMoveY_pixels = 0;
        String clickedGridName = "", gridName_MouseEnter = "";
        List<IDayInfo> dayInfo;
        Dictionary<Double, Double> time;
        DateTime date;
        public List<Hyperlink> hyperlinkList;
        LoadInfo loadInfo;
        public Dictionary<IEventMainInfo, IEventInfo> eventDictionary;

        public Events(DayControl control)
        {
            date = DateTime.Now;
            this.control = control;
            gridList = new List<Grid>();
            rowList = new List<RowDefinition>();
            GetGrids();
            hyperlinkList = new List<Hyperlink>();
            SetHyperlinks();
            loadInfo = new LoadInfo(control,hyperlinkList, this);
            eventDictionary = new Dictionary<IEventMainInfo, IEventInfo>();
        }

        public void SetDate(DateTime date)
        {
            this.date = date;
        }

        public void Event()
        {
            foreach(Grid grid in gridList)
            {
                grid.MouseLeftButtonDown += grid_MouseLeftButtonDown;
                grid.MouseLeftButtonUp += grid_MouseLeftButtonUp;
            }
            control.MouseLeftButtonDown += grid_MouseLeftButtonDown;
            control.MouseLeftButtonUp += control_MouseLeftButtonUp;
            
            setPixelsEvent += Events_setPixelsEvent;
            
            LoadEventInfo_Event.GetEvent += LoadEventInfo_Event_GetEvent;
        }
//TODO: event for reload Info
//TODO: Здесь событие получающее данные для отображения в hyperlink (сдельть для недели и месяца)
        void LoadEventInfo_Event_GetEvent(object sender, LoadEventInfo e)
        {
            if(e.calendarEnum == LoadEventInfo_Event.CalendarEnum.Day)
            {
                loadInfo.SetInfoTexts(e.dictionaryInfo);
            }
        }

        /// <summary>
        /// Добавление  hyperlinks  в  hyperlinkList
        /// </summary>
        public void SetHyperlinks()
        {
            foreach(Grid grid in gridList)
            {
                String name = "h" + grid.Name.Substring(1);
                Hyperlink hyperlink = (Hyperlink)control.FindName(name);
                if (hyperlink != null)
                    hyperlinkList.Add(hyperlink);
            }
        }

        public void SetCurrentDayInfo(List<IDayInfo> info)
        {
            dayInfo = info;
            if (time == null)
                time = new Dictionary<Double, Double>();
        }

        public void ReloadData()
        {
            if (dayInfo == null)
                dayInfo = new List<IDayInfo>();
            if (time == null)
                time = new Dictionary<Double, Double>();

            foreach (Grid grid in gridList)
                grid.Background = Brushes.White;

            foreach(IDayInfo info in dayInfo)
            {
                Double from = 0, to = 0;
                from = info.timeFrom / 0.5;
                to = info.timeTo / 0.5;
                for(int i = 0; i < gridList.Count; ++i)
                {
                    if (i >= from && i <= to && gridList[i].Background != new SolidColorBrush(Color.FromRgb(224, 242, 241)))
                        gridList[i].Background = new SolidColorBrush(Color.FromRgb(224, 242, 241));
                    else
                        gridList[i].Background = Brushes.White;
                }
            }

        }

        void control_MouseMove(object sender, MouseEventArgs e)
        {
            if (gridName_MouseEnter == "")
                return;
        }

        void grid_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid grid = (Grid)sender;
            if (mouseMoveKey)
                gridName_MouseEnter = grid.Name;
            else
                return;

            Grid grid1 = gridList.Where(x => x.Name == clickedGridName).First();
            Grid grid2 = gridList.Where(x => x.Name == gridName_MouseEnter).First();
            Int32 index1 = gridList.IndexOf(grid1);
            Int32 index2 = gridList.IndexOf(grid2);


            for (int i = 0; i < gridList.Count; ++i)
            {
                if (index1 <= index2)
                {
                    if (i >= index1 && i <= index2)
                        gridList[i].Background = new SolidColorBrush(Color.FromRgb(128, 203, 196));
                    else
                        gridList[i].Background = Brushes.White;
                }
                else
                {
                    if (i >= index2 && i <= index1)
                        gridList[i].Background = new SolidColorBrush(Color.FromRgb(128, 203, 196));
                    else
                        gridList[i].Background = Brushes.White;
                }
            }

        }

        void grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!mouseMoveKey)
                return;
            Grid grid = (Grid)sender;
            grid.ReleaseMouseCapture();
            gridName_MouseEnter = "";
            mouseMoveKey = false;
            currentCountRowHiden = 0;
            StartCountRowHiden = 0;

            Boolean key = false;
            Double timefrom = 0, timeto = 0;
            for(int i = 0; i < gridList.Count; ++i)
            {
                if (gridList[i].Background == Brushes.White && key == false)
                    timefrom = i;
                if (gridList[i].Background != Brushes.White)
                {
                    timeto = i;
                    key = true;
                }
            }
            timefrom += 1;
            timeto += 1;
            Int32 hourFrom = 0;
            if (timefrom % 2 == 0)
                hourFrom = Convert.ToInt32(timefrom / 2);
            else
                hourFrom = Convert.ToInt32((timefrom - 1) / 2);
            Double halfHourFrom = timefrom % 2;
            Int32 hourTo = 0;
            if(timeto % 2 == 0)
                hourTo = Convert.ToInt32(timeto / 2);
            else
                hourTo = Convert.ToInt32((timeto - 1) / 2);
            Double halfHoutTo = timeto % 2;
            if (date.Year == 0001 && date.Month == 01 && date.Day == 01)
                date = DateTime.Now;

            
            DateTime dateFrom = new DateTime(date.Year,date.Month,date.Day,hourFrom,(halfHourFrom > 0 ? 30: 0),0);
            DateTime dateTo = new DateTime();
            if (hourTo < 24)
                dateTo = new DateTime(date.Year, date.Month, date.Day, hourTo, (halfHoutTo > 0 ? 30 : 0), 0);
            else
                dateTo = new DateTime(date.Year, date.Month, date.Day, 23, 59, 0);
            SetTime(dateFrom, dateTo);
        }

        void control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mouseMoveKey = false;
            TopOrUpCount = 0;
            StartCountRowHiden = 0;
            currentCountRowHiden = 0;
        }
        
        void Events_setPixelsEvent(object sender, MouseMove_TopBottom e)
        {
            currentCountRowHiden = 0;
            foreach (RowDefinition row in rowList)
            {
                if (row.ActualHeight == 0)
                    currentCountRowHiden += 1;
                else if (row.ActualHeight > 0)
                    break;
            }
            currentCountRowHiden /= 2;
            Int32 startIndex = gridList.FindIndex(x => x.Name == clickedGridName);
            Int32 plusIndex = 0;
            plusIndex = currentCountRowHiden - StartCountRowHiden;
            Int32 endIndex = 0;
            
            if (plusIndex > 0)
                endIndex = Convert.ToInt32((e.pixels) / 25) + plusIndex - 1 + StartCountRowHiden;
            else if (plusIndex < 0)
                endIndex = Convert.ToInt32(e.pixels / 25) - 1 + currentCountRowHiden;
            else
                endIndex = Convert.ToInt32(e.pixels / 25) - 1 + currentCountRowHiden;
            
            for(int i = 0; i < gridList.Count; ++i)
            {
                if(startIndex <= endIndex)
                {
                    if (i >= startIndex && i <= endIndex)
                        gridList[i].Background = new SolidColorBrush(Color.FromRgb(128, 203, 196));
                    else
                        gridList[i].Background = Brushes.White;
                }
                else
                {
                    if (i >= endIndex && i <= startIndex)
                        gridList[i].Background = new SolidColorBrush(Color.FromRgb(128, 203, 196));
                    else
                        gridList[i].Background = Brushes.White;
                }
            }
        }

        void grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
            if (mouseMoveKey)
                return;
            if (sender.GetType() == new Grid().GetType())
            {
                foreach (Grid g in gridList)
                    g.Background = Brushes.White;

                Grid grid = (Grid)sender;
                currentPositionMouseMove = Mouse.GetPosition(control).Y;
                clickedGridName = grid.Name;
                grid.Background = new SolidColorBrush(Color.FromRgb(128, 203, 196));
                grid.CaptureMouse();
                mouseMoveKey = true;
                foreach(RowDefinition row in rowList)
                {
                    if (row.ActualHeight == 0)
                        StartCountRowHiden += 1;
                }
                StartCountRowHiden /= 2;
            }
        }

        void GetGrids()
        {
            foreach (var v1 in control.Grid_Day.Children)
            {
                if (v1.GetType() == new Grid().GetType())
                {
                    Grid grid = v1 as Grid;
                    foreach (var v in grid.Children)
                    {
                        if (v.GetType() == new Border().GetType())
                        {
                            Grid g = (v as Border).Child as Grid;
                            gridList.Add(g);
                        }
                    }
                }
            }

            foreach (Grid grid in gridList)
                grid.Background = Brushes.White;

            foreach(Grid grid in gridList)
            {
                Grid g = ((grid.Parent as Border).Parent as Grid);
                rowList.Add(g.RowDefinitions[0]);
                rowList.Add(g.RowDefinitions[1]);                
            }
        }

        public event EventHandler<MouseMove_TopBottom> setPixelsEvent;

        public void SetPixels(Double pixels)
        {
            setPixelsEvent(this, new MouseMove_TopBottom(pixels));
        }

        public event EventHandler<MouseLeftButtonUp_FromUserControl> SetTimeEvent;

        public void SetTime(DateTime from, DateTime to)
        {
            if (SetTimeEvent != null)
                SetTimeEvent(this, new MouseLeftButtonUp_FromUserControl(from, to));
        }
    }

    public class MouseMove_TopBottom: EventArgs
    {
        public Double pixels { get; private set; }
        public MouseMove_TopBottom(Double pixels)
        {
            this.pixels = pixels;
        }
    }

    public class MouseLeftButtonUp_FromUserControl:EventArgs
    {
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        public MouseLeftButtonUp_FromUserControl(DateTime from, DateTime to)
        {
            this.From = from;
            this.To = to;
        }
    }
}
