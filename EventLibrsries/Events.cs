using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EventLibrsries
{
    public class ButtonsEvent
    {
        public Button button { get; private set; }
        public DateTime date { get; private set; }
        public ButtonsEvent(Button button, DateTime date)
        {
            this.button = button;
            this.date = date;
        }
    }

    public class Date_CkickEvent
    {
        public void Click(Button button, DateTime date)
        {
            if(buttonsEvent != null)
                buttonsEvent(this, new ButtonsEvent(button, date));
        }
        public static event EventHandler<ButtonsEvent> buttonsEvent;
    }

    public class LoginData
    {
        public String login { get; set; }
        public String password { get; set; }
    }

    public class LoginEvent : EventArgs
    {
        public LoginData data { get; private set; }
        public LoginEvent(LoginData data)
        {
            this.data = data;
        }
    }

    public class GetLoginDataEvent
    {
        public void GetData(LoginData data)
        {
            if (dataEvent != null)
                dataEvent(this, new LoginEvent(data));
        }
        public static event EventHandler<LoginEvent> dataEvent;
    }

    public class RegisterData
    {
        public String Name { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
    }

    public class RegisterEvent : EventArgs
    {
        public RegisterData data { get; private set; }
        public RegisterEvent(RegisterData data)
        {
            this.data = data;
        }
    }

    public class RegisterEventData
    {
        public void GetData(RegisterData data)
        {
            if (RegEvent != null)
                RegEvent(this, new RegisterEvent(data));
        }
        public static event EventHandler<RegisterEvent> RegEvent;
    }

    public class ConnectionExceptionEvent: EventArgs
    {
        public String message { get; private set; }
        public ConnectionExceptionEvent(String message)
        {
            this.message = message;
        }
    }

    public class ConnectionEvent
    {
        public static event EventHandler<ConnectionExceptionEvent> ConnectEvent;
        public void ConnectionException(String message)
        {
            if (ConnectEvent != null)
                ConnectEvent(this, new ConnectionExceptionEvent(message));
        }
    }

    public class TopMenuButtonClick: EventArgs
    {
        public String buttonName { get; private set; }
        public TopMenuButtonClick(String buttonName)
        {
            this.buttonName = buttonName;
        }
    }

    public class TopMenuButtonEvents
    {
        public static event EventHandler<TopMenuButtonClick> buttonEvent;
        public void ButtonClickEvent(String buttonName)
        {
            if(buttonEvent != null)
                buttonEvent(this, new TopMenuButtonClick(buttonName));
        }
    }

    public class ChangeMenu: EventArgs
    {
        public String message { get; private set; }
        public ChangeMenu(String message)
        {
            this.message = message;
        }
    }

    public class ChangeMenuEvent
    {
        public static event EventHandler<ChangeMenu> changeMenuEvent;
        public void ChangeEventMessage(String message)
        {
            if (changeMenuEvent != null)
                changeMenuEvent(this, new ChangeMenu(message));
        }
    }

    public class AfterLogin:EventArgs
    {
        public Boolean logedIn { get; private set; }
        public AfterLogin(Boolean key)
        {
            this.logedIn = key;
        }
    }

    public class AfterLogInEvent
    {
        public static event EventHandler<AfterLogin> logInEvent;
        public void SetEvent(Boolean key)
        {
            if (logInEvent != null)
                logInEvent(this, new AfterLogin(key));
        }
    }

    public class ButtonEvent: EventArgs
    {
        public String buttonName { get; private set;}
        public ButtonEvent(String buttonName)
        {
            this.buttonName = buttonName;
        }
    }

    public class ButtonEventNames
    {
        public static event EventHandler<ButtonEvent> buttonEv;
        public void SetButton(String buttonName)
        {
            if (buttonEv != null)
                buttonEv(this, new ButtonEvent(buttonName));
        }
    }

    public class EventInfo : EventArgs
    {
        public String data { get; private set; }
        public IEventMainInfo mainInfo { get; private set; }
        public EventInfo(IEventMainInfo mainInfo, String data)
        {
            this.data = data;
            this.mainInfo = mainInfo;
        }
    }

    public class EventCalendarInfo
    {
        public static event EventHandler<EventInfo> calendarInfoChanged;

        public void CalendarEventChanged(IEventMainInfo mainInfo, String data)
        {
            if (calendarInfoChanged != null)
                calendarInfoChanged(this, new EventInfo(mainInfo, data));
        }
    }

    public class CalendarEvents: EventArgs
    {
        public IEventInfo info { get; private set; }
        public IEventMainInfo mainInfo { get; private set; }
        public CalendarEvents(IEventMainInfo mainInfo, IEventInfo info)
        {
            this.mainInfo = mainInfo;
            this.info = info;
        }
    }

    public class Calendar_AddEvent
    {
        public static event EventHandler<CalendarEvents> addCalendarEvent;
        public void AddEvent(IEventMainInfo mainInfo, IEventInfo info)
        {
            if (addCalendarEvent != null)
                addCalendarEvent(this, new CalendarEvents(mainInfo, info));
        }
    }

    public class AddEventmenu: EventArgs
    {
        public Double height { get; private set; }
        public AddEventmenu(Double windowHeight)
        {
            this.height = windowHeight;
        }
    }

    public class AddEventMenu_Event
    {
        public static event EventHandler<AddEventmenu> changeHeightEvent;
        public void ChangeMainWindowEvent(Double windowHeight)
        {
            if (changeHeightEvent != null)
            changeHeightEvent(this, new AddEventmenu(windowHeight));
        }
    }

    public class ResetClassEvent: EventArgs
    {
        public Boolean reset { get; private set; }
        public Double height { get; private set; }
        public ResetClassEvent(Boolean reset, Double height)
        {
            this.reset = reset;
            this.height = height;
        }
    }

    public class ResetClass
    {
        public static event EventHandler<ResetClassEvent> resetEvent;

        public void ResetEvent(Boolean reset, Double height)
        {
            if (resetEvent != null)
                resetEvent(this, new ResetClassEvent(reset, height));
        }
    }

    public class CalendarEventData: EventArgs
    {
        public IEventInfo eventInfo { get; private set; }
        public IEventMainInfo eventMainInfo { get; private set; }
        public CalendarEventData(IEventInfo eventInfo, IEventMainInfo eventMainInfo)
        {
            this.eventInfo = eventInfo;
            this.eventMainInfo = eventMainInfo;
        }
    }

    public class CalendarEventInfo
    {
        public static event EventHandler<CalendarEventData> eventData;
        public void SendEvent(IEventInfo eventInfo, IEventMainInfo eventMainInfo)
        {
            if (eventData != null)
                eventData(this, new CalendarEventData(eventInfo, eventMainInfo));
        }
    }

    public class HideCalendarAddEventMenu: EventArgs
    {
        public Boolean key { get; private set; }
        public HideCalendarAddEventMenu(Boolean key)
        {
            this.key = key;
        }
    }

    public class HideCalendarEventAddMenu_Event
    {
        public static event EventHandler<HideCalendarAddEventMenu> hideMenuEvent;
        public void SetHideMenuEvent(Boolean key)
        {
            if (hideMenuEvent != null)
                hideMenuEvent(this, new HideCalendarAddEventMenu(key));
        }
    }

    public class CalendarChangeDays: EventArgs
    {
        public String direction { get; private set; }
        public CalendarChangeDirection_Event.DirectionSize directionSize { get; private set; }
        public CalendarChangeDays(String direction, CalendarChangeDirection_Event.DirectionSize directionSize)
        {
            this.direction = direction;
            this.directionSize = directionSize;
        }
    }

    public class CalendarChangeDirection_Event
    {
        public static event EventHandler<CalendarChangeDays> changeDayEvent;
        public enum DirectionSize { Day, Week, Month, Timetable };
        public void SelectEvent(String direction, DirectionSize directionSize)
        {
            changeDayEvent(this, new CalendarChangeDays(direction, directionSize));
        }
    }

    public class GetCurrentCalendarPanel: EventArgs
    {
        public String panelName { get; private set; }
        public Boolean key { get; private set; }
        public GetCurrentCalendarPanel(String panelName)
        {
            this.panelName = panelName;
        }
        public GetCurrentCalendarPanel(Boolean key)
        {
            this.key = key;
        }
    }

    public class GetCurrentCalendarPanel_Event
    {
        public static event EventHandler<GetCurrentCalendarPanel> GetNameEvent;
        
        public void GetPanelName(Boolean key)
        {
            if (GetNameEvent != null)
                GetNameEvent(this, new GetCurrentCalendarPanel(key));
        }
    }

    public class GetCalendarCurrentPanelName
    {
        public static event EventHandler<GetCurrentCalendarPanel> SendEvent;
        public void SetEvent(String panelName)
        {
            if (SendEvent != null)
                SendEvent(this, new GetCurrentCalendarPanel(panelName));
        }
    }

    public class CalendarSelectWeekBorder: EventArgs
    {
        public Boolean key { get; private set; }
        public CalendarSelectWeekBorder(Boolean key)
        {
            this.key = key;
        }
    }

    public class CalendarSelectWeekBorder_Event
    {
        public static event EventHandler<CalendarSelectWeekBorder> GetEvent;
        public void SetEvent(Boolean key)
        {
            if(GetEvent != null)
                GetEvent(this, new CalendarSelectWeekBorder(key));
        }
    }



}


