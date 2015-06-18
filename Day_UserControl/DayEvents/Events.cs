using Day_UserControl.Model;
using Day_UserControl.VisualStyles;
using EventLibrsries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Day_UserControl.DayEvents
{
    public class Events
    {
        public DayUserControl control;
        VisualStyles.VisualStyle visual;
        Variables variables;
        LoadInfo loadInfo;
        DayUserControl_SendMoveEventAfterClick_Event mouseMoveAfterClick;
        ShowSelectingEvent showSelectingEvent;
        SendDatesTo_AddEventMenu_Event sendDatesToAddEventMenu;
        PopupEvents popupEvents;

        public Events(DayUserControl control)
        {
            this.control = control;
            visual = VisualStyles.VisualStyle.GetInstance(control);
            variables = Variables.GetInstance;
            loadInfo = new LoadInfo(control);
            mouseMoveAfterClick = new DayUserControl_SendMoveEventAfterClick_Event();
            showSelectingEvent = new ShowSelectingEvent();
            sendDatesToAddEventMenu = new SendDatesTo_AddEventMenu_Event();
            popupEvents = new PopupEvents(control);
        }

        /// <summary>
        /// Заполнение контролла полями, Grids, временем и т.д.
        /// </summary>
        public void FillControl()
        {
            visual.FillByColumnsAndRows();
            visual.FillLeftColumnsByTime();
            visual.FillRightColumnByGrids();
            visual.FillGridsByHyperlinks();
        }

        /// <summary>
        /// События...
        /// </summary>
        public void SetEvents()
        {
            foreach(Grid grid in variables.gridList)
                grid.MouseLeftButtonDown += grid_MouseLeftButtonDown;
            LoadEventInfo_Event.GetEvent += LoadEventInfo_Event_GetEvent;
            control.MouseMove += control_MouseMove;
            control.MouseLeftButtonUp += control_MouseLeftButtonUp;
            popupEvents.SetEvents();
            CleanEvent_Event.GetEvent += CleanEvent_Event_GetEvent;
        }

        void CleanEvent_Event_GetEvent(object sender, CleanEvent e)
        {
            variables.currentDate = DateTime.Now;
            variables.eventInfoDictionary.Clear();
            variables.FirstSelectedIndex = 0;
            foreach (Grid grid in variables.gridList)
                grid.Background = Brushes.White;
            foreach (Hyperlink hyperlink in variables.hyperlinkList)
                hyperlink.Inlines.Clear();
            variables.LastSelectedIndex = 0;
            variables.SelectedGrid = null;
        }

        /// <summary>
        /// Событие после которого отправляется время в AddEventMenu
        /// </summary>
        void control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            control.ReleaseMouseCapture();
            CreateDatesAndSendTo_AddEventMenu();
        }

        /// <summary>
        /// Создание и отправка двух дат в контролл AddEventMenu
        /// </summary>
        void CreateDatesAndSendTo_AddEventMenu()
        {
            Int32 firstIndex = 0, lastIndex = 0;
            if (variables.FirstSelectedIndex > variables.LastSelectedIndex)
            {
                firstIndex = variables.LastSelectedIndex;
                lastIndex = variables.FirstSelectedIndex;
            }
            else
            {
                firstIndex = variables.FirstSelectedIndex;
                lastIndex = variables.LastSelectedIndex;
            }

            DateTime dateFrom = DateTime.Now, dateTo = DateTime.Now;
            Int32 hFrom, hTo, mFrom, mTo;
            hFrom = firstIndex / 2;
            hTo = lastIndex / 2;
            mFrom = (firstIndex % 2) == 0 ? 0 : 30;
            mTo = (lastIndex % 2) == 0 ? 30 : 0;

            if (mTo == 0)
                hTo += 1;
            if(hTo == 24)
            {
                hTo = 23;
                mTo = 59;
            }

            dateFrom = new DateTime(variables.currentDate.Year, variables.currentDate.Month, variables.currentDate.Day,hFrom,mFrom,0);
            dateTo = new DateTime(variables.currentDate.Year, variables.currentDate.Month, variables.currentDate.Day, hTo, mTo, 0);

            sendDatesToAddEventMenu.SendEvent(dateFrom, dateTo);
        }

        /// <summary>
        /// Движение мыши в момент выделения нового события (выделение полей)
        /// </summary>
        void control_MouseMove(object sender, MouseEventArgs e)
        {
            if(control.IsMouseCaptured)
            {
                mouseMoveAfterClick.SendEvent(true);
                showSelectingEvent.FillGrids(Mouse.GetPosition(control).Y);
            }
        }

        /// <summary>
/// Событие принимающее EventInfo для отображения на панели
/// </summary>
/// <param name="e"></param>
        void LoadEventInfo_Event_GetEvent(object sender, LoadEventInfo e)
        {
            if (e.calendarEnum != LoadEventInfo_Event.CalendarEnum.Day)
                return;
            
            variables.eventInfoDictionary.Clear();
            variables.eventInfoDictionary = e.dictionaryInfo;
            loadInfo.SelectMainContent();
            loadInfo.LoadEventsInfo();
        }

        /// <summary>
        /// Событие нажатия на Grid
        /// </summary>
        void grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            variables.SelectedGrid = sender as Grid;
            variables.SelectedGrid.Background = new SolidColorBrush(Color.FromRgb(128, 203, 196));
            GetSelectedIndex(Mouse.GetPosition(control).Y);
            control.CaptureMouse();
        }

        /// <summary>
        /// Получение индекса (при выделении) на котором находится мышь
        /// </summary>
        /// <param name="height">Высота координаты мыши ('Y') по отношению к контроллу</param>
        void GetSelectedIndex(Double height)
        {
            Double h = (height - 50) % 25;
            variables.FirstSelectedIndex = Int32.Parse(((height - h - 50) / 25).ToString());
        }
    }
}
