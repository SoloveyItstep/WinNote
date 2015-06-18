using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IInterfaces
{
    public interface IDayInfo
    {
        String Name { get; set; }
        Double timeFrom { get; set; }
        Double timeTo { get; set; }
        Boolean IsEvent { get; set; }
    }

    public interface IEventInfo
    {
        int AllDay { get; set; }
        int Repeat { get; set; }
        Calendarenums.EnumRepeat enumRepeat { get; set; }
        List<Calendarenums.DayOfWeekEnum> DayOfWeek { get; set; }
        int Remind { get; set; }
        int Reminded { get; set; }
        Calendarenums.ReminderDate reminderDate { get; set; }
        String location { get; set; }
        String description { get; set; }
    }

    public interface IEventMainInfo
    {
        String EventName { get; set; }
        DateTime dateFrom { get; set; }
        DateTime dateTo { get; set; }
        DateTime created { get; set; }
        String Data { get; set; }
        Int32 ServerId { get; set; }
        String ShareKey { get; set; }
    }

   public class Calendarenums
   {
       public  enum EnumRepeat { Day, WorkDay, Week, Month, Yaer }
       public  enum DayOfWeekEnum { Monday, Tuesday, Wednesday, Thirsday, Friday, Saturday, Sunday }
       public  enum ReminderDate { Day, Hour, fiveMinutes }
   }
}
