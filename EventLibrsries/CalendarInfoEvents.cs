using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventLibrsries
{
    public class SendEventAboutChanges: EventArgs
    {
        public Boolean key { get; private set; }
        public SendEventAboutChanges(Boolean key)
        {
            this.key = key;
        }
    }

    /// <summary>
    /// Класс-событие!  Передача уведомления для передачи события в UserControl
    /// </summary>
    public class SendEventAboutChanges_Event
    {
        public static event EventHandler<SendEventAboutChanges> GetEvent;
        public void SendEvent(Boolean key)
        {
            if (GetEvent != null)
                GetEvent(this, new SendEventAboutChanges(key));
        }
    }

    public class SendEventToControl: EventArgs
    {
        public SendEventToControl_Event.ControlNameEnum controlName { get; private set; }
        public SendEventToControl(SendEventToControl_Event.ControlNameEnum controlName)
        {
            this.controlName = controlName;
        }
    }

    /// <summary>
    /// Класс-событие для передачи события в контролы (с определенным ключем) для отправки в Model даты и того же ключа
    /// </summary>
    public class SendEventToControl_Event
    {
        public enum ControlNameEnum { Day, Week, Month, Timetable };

        public static event EventHandler<SendEventToControl> GetEvent;
        public void SendEvent(ControlNameEnum controlName)
        {
            if (GetEvent != null)
                GetEvent(this, new SendEventToControl(controlName));
        }
    }

    public class SendEventToModel: EventArgs
    {
        public SendEventToModel_Event.ControlNameEnum controlName { get; private set; }
        public DateTime date { get; private set; }
        public SendEventToModel(DateTime date, SendEventToModel_Event.ControlNameEnum controlName)
        {
            this.date = date;
            this.controlName = controlName;
        }
    }

    /// <summary>
    /// Класс-событие для отправки в Model имени контрола (день, неделя, месяц, расписание) и даты:
    ///                                                дата недели должна начинаться с понедельника
    /// </summary>
    public class SendEventToModel_Event
    {
        public enum ControlNameEnum { Day, Week, Month, Timetable };
        public static event EventHandler<SendEventToModel> GetEvent;
        public void SendEvent(DateTime date, ControlNameEnum controlName)
        {
            if(GetEvent != null)
                GetEvent(this, new SendEventToModel(date, controlName));
        }
    }

    public class LoadEventInfo : EventArgs
    {
        public Dictionary<IEventMainInfo, IEventInfo> dictionaryInfo { get; private set; }
        public LoadEventInfo_Event.CalendarEnum calendarEnum { get; private set; }
        public LoadEventInfo(Dictionary<IEventMainInfo, IEventInfo> dictionaryInfo, LoadEventInfo_Event.CalendarEnum calendarEnum)
        {
            this.dictionaryInfo = dictionaryInfo;
            this.calendarEnum = calendarEnum;
        }
    }

    /// <summary>
    /// Класс-событие для отправки событий в контролы (Day, Week, Month, Timetable)
    /// </summary>
    public class LoadEventInfo_Event
    {
        public enum CalendarEnum { Day, Week, Month, Timetable };
        public static event EventHandler<LoadEventInfo> GetEvent;
        public void SetEvent(Dictionary<IEventMainInfo, IEventInfo> dictionaryInfo, LoadEventInfo_Event.CalendarEnum calendarEnum)
        {
            if (GetEvent != null)
                GetEvent(this, new LoadEventInfo(dictionaryInfo, calendarEnum));
        }
    }

    public class SendEventWidthIeventInfoToAddEventMenu : EventArgs
    {
        public IEventMainInfo eventMainInfo { get; private set; }
        public IEventInfo eventInfo { get; private set; }
        public SendEventWidthIeventInfoToAddEventMenu(IEventMainInfo eventMainInfo, IEventInfo eventInfo)
        {
            this.eventMainInfo = eventMainInfo;
            this.eventInfo = eventInfo;
        }
    }

    /// <summary>
    /// Передача события (календарное событие с данными) в меню AddEventMenu для изминения
    /// </summary>
    public class SendEventWidthIeventInfoToAddEventMenu_Event
    {
        public static event EventHandler<SendEventWidthIeventInfoToAddEventMenu> GetEvent;
        public void SendEvent(IEventMainInfo eventMainInfo, IEventInfo eventInfo)
        {
            if (GetEvent != null)
                GetEvent(this, new SendEventWidthIeventInfoToAddEventMenu(eventMainInfo, eventInfo));
        }
    }
    
    public class SendEventInfoToChange : EventArgs
    {
        public IEventMainInfo eventMainInfo { get; private set; }
        public IEventInfo eventInfo { get; private set; }
        public SendEventInfoToChange(IEventMainInfo eventMainInfo, IEventInfo eventInfo)
        {
            this.eventMainInfo = eventMainInfo;
            this.eventInfo = eventInfo;
        }
    }

    /// <summary>
    /// Передача события (календарное событие с данными) в Entity class и MySQL class для изминения
    /// </summary>
    public class SendEventInfoToChange_Event
    {
        public static event EventHandler<SendEventInfoToChange> GetEvent;
        public void SendEvent(IEventMainInfo eventMainInfo, IEventInfo eventInfo)
        {
            if (GetEvent != null)
                GetEvent(this, new SendEventInfoToChange(eventMainInfo, eventInfo));
        }
    }

    public class SendEventToDeleteEventInfo: EventArgs
    {
        public IEventMainInfo mainInfo { get; private set; }
        public IEventInfo info { get; private set; }
        public SendEventToDeleteEventInfo(IEventMainInfo mainInfo, IEventInfo info)
        {
            this.mainInfo = mainInfo;
            this.info = info;
        }
    }

    /// <summary>
    /// Передача события (календарное событие с данными) в Entity class и MySQL class для удаления
    /// </summary>
    public class SendEventToDeleteEventInfo_Event
    {
        public static event EventHandler<SendEventToDeleteEventInfo> GetEvent;
        public void Sendinfo(IEventMainInfo mainInfo, IEventInfo info)
        {
            if (GetEvent != null)
                GetEvent(this, new SendEventToDeleteEventInfo(mainInfo,info));

        }
    }

    public class EventToDoActions: EventArgs
    {
        public IEventMainInfo mainInfo { get; set; }
        public IEventInfo info { get; set; }
        public String Value { get; set; }

        public EventToDoActions(IEventMainInfo mainInfo, IEventInfo info, String Value)
        {
            this.mainInfo = mainInfo;
            this.Value = Value;
            this.info = info;
        }
    }

    /// <summary>
    /// Класс событие передающее данные для занесения в таблицу CalendarEventToLoadOnServer (при отсутствии связи интернет м т.д.)
    /// </summary>
    public class EventToDoActions_Event
    {
        public static event EventHandler<EventToDoActions> GetEvent;
        public void SendEvent(IEventMainInfo mainInfo, IEventInfo info, String Value)
        {
            if(GetEvent != null)
                GetEvent(this, new EventToDoActions(mainInfo, info, Value));
        }
    }

    public class SendBackEventAfterDoAction: EventArgs
    {
        public IEventMainInfo mainInfo { get; set; }
        public IEventInfo info { get; set; }
        public String Value { get; set; }
        public SendBackEventAfterDoAction(IEventMainInfo mainInfo, String Value)
        {
            this.mainInfo = mainInfo;
            this.Value = Value;
        }

        public SendBackEventAfterDoAction(IEventMainInfo mainInfo, IEventInfo info, String Value)
        {
            this.mainInfo = mainInfo;
            this.info = info;
            this.Value = Value;
        }
    }

    /// <summary>
    /// Класс событие для отправки обратного сообщения после изминений на сервере (когда EventToDoActions_Event отработан)
    /// </summary>
    public class SendBackEventAfterDoAction_Event
    {
        public static event EventHandler<SendBackEventAfterDoAction> GetEvent;
        public void SendEvent(IEventMainInfo mainInfo, String Value)
        {
            if (GetEvent != null)
                GetEvent(this, new SendBackEventAfterDoAction(mainInfo, Value));
        }

        public void SendEvent(IEventMainInfo mainInfo, IEventInfo info, String Value)
        {
            if (GetEvent != null)
                GetEvent(this, new SendBackEventAfterDoAction(mainInfo,info,Value));
        }
    }

    public class BalloonInfo: EventArgs
    {
        public IEventMainInfo mainInfo { get; private set; }
        public IEventInfo info { get; private set; }
        public BalloonInfo(IEventMainInfo mainInfo, IEventInfo info)
        {
            this.mainInfo = mainInfo;
            this.info = info;
        }
    }

    /// <summary>
    /// Класс событие для Balloon с передачей IEventMainInfo и IEventInfo для отображения
    /// </summary>
    public class BalloonInfo_Event
    {
        public static event EventHandler<BalloonInfo> GetEvent;
        public void SendEvent(IEventMainInfo mainInfo, IEventInfo info)
        {
            if (GetEvent != null)
                GetEvent(this, new BalloonInfo(mainInfo, info));
        }
    }
}
