using EventLibrsries;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using EventLibrsries;

namespace WinNote.Control.Events
{
    public class MyNotifyIcons
    {
        MainWindow window;
        NotifyIcon notifyIcon;
        List<Thread> threadList;
        IEventMainInfo mainInfo;
        IEventInfo info;
        SendEventInfoToChange_Event sendEventToChange;
        EventLibrsries.SendEventAboutChanges_Event sendEventAboutChanges;

        public MyNotifyIcons(MainWindow window)
        {
            this.window = window;
            threadList = new List<Thread>();
            sendEventToChange = new SendEventInfoToChange_Event();
            sendEventAboutChanges = new EventLibrsries.SendEventAboutChanges_Event();
        }

        public void SetEvents()
        {
            notifyIcon.MouseClick += notifyIcon_MouseClick;
            BalloonInfo_Event.GetEvent += BalloonInfo_Event_GetEvent;
            notifyIcon.BalloonTipClicked += notifyIcon_BalloonTipClicked;
        }

        void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            Boolean repeat = false;
            
            if (info.Repeat == 1)
            {
                if (info.enumRepeat == Calendarenums.EnumRepeat.Day)
                {
                    mainInfo.dateFrom.AddDays(1);
                    mainInfo.dateTo.AddDays(1);
                    repeat = true;
                }
                else if (info.enumRepeat == Calendarenums.EnumRepeat.Week)
                {
                    ChangeWeekDayRepeat();
                    repeat = true;
                }
                else if (info.enumRepeat == Calendarenums.EnumRepeat.WorkDay)
                {
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Friday)
                    {
                        mainInfo.dateFrom.AddDays(1);
                        mainInfo.dateTo.AddDays(1);
                    }
                    else
                    {
                        mainInfo.dateFrom.AddDays(3);
                        mainInfo.dateTo.AddDays(3);
                    }
                    repeat = true;
                }
                else if (info.enumRepeat == Calendarenums.EnumRepeat.Month)
                {
                    mainInfo.dateFrom.AddMonths(1);
                    mainInfo.dateTo.AddMonths(1);
                    repeat = true;
                }
                else if(info.enumRepeat == Calendarenums.EnumRepeat.Yaer)
                {
                    mainInfo.dateFrom.AddYears(1);
                    mainInfo.dateTo.AddYears(1);
                    repeat = true;
                }
            }

            if (!repeat)
                info.Reminded = 0;

            sendEventToChange.SendEvent(mainInfo, info);
            sendEventAboutChanges.SendEvent(true);
        }

        void ChangeWeekDayRepeat()
        {
            List<DayOfWeek> dayList = new List<DayOfWeek>();
            foreach(Calendarenums.DayOfWeekEnum day in info.DayOfWeek)
            {
                switch(day)
                {
                    case Calendarenums.DayOfWeekEnum.Monday:
                        dayList.Add(DayOfWeek.Monday);
                        break;
                    case Calendarenums.DayOfWeekEnum.Tuesday:
                        dayList.Add(DayOfWeek.Tuesday);
                        break;
                    case Calendarenums.DayOfWeekEnum.Wednesday:
                        dayList.Add(DayOfWeek.Wednesday);
                        break;
                    case Calendarenums.DayOfWeekEnum.Thirsday:
                        dayList.Add(DayOfWeek.Thursday);
                        break;
                    case Calendarenums.DayOfWeekEnum.Friday:
                        dayList.Add(DayOfWeek.Friday);
                        break;
                    case Calendarenums.DayOfWeekEnum.Saturday:
                        dayList.Add(DayOfWeek.Saturday);
                        break;
                    case Calendarenums.DayOfWeekEnum.Sunday:
                        dayList.Add(DayOfWeek.Sunday);
                        break;
                }
            }

            Int32 index = 0;
            foreach(DayOfWeek day in dayList)
            {
                if (day > DateTime.Now.DayOfWeek)
                {
                    index = day - DateTime.Now.DayOfWeek;
                    break;
                }
            }
            if (index <= 0)
                index = DayOfWeek.Saturday - DateTime.Now.DayOfWeek + 2;


            mainInfo.dateFrom = mainInfo.dateFrom.AddDays(index);
            mainInfo.dateTo = mainInfo.dateTo.AddDays(index);
            
        }

        public void LoadNotifyIcon()
        {
            notifyIcon = new NotifyIcon();

            notifyIcon.Icon = Images.Images.logoWinNote;
            notifyIcon.BalloonTipIcon = ToolTipIcon.None;
            notifyIcon.Visible = true;
        }

        public void WindowHide()
        {
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += (sndr, args) =>
            {
                window.Show();
                window.WindowState = WindowState.Normal;
            };
            window.Hide();
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                System.Windows.Forms.MenuItem[] menuItem = new System.Windows.Forms.MenuItem[3];
                menuItem[0] = new System.Windows.Forms.MenuItem();
                menuItem[1] = new System.Windows.Forms.MenuItem();
                menuItem[2] = new System.Windows.Forms.MenuItem();
                menuItem[0].Click += menuWindow_Click;
                menuItem[1].Click += menuWindow_Click;
                menuItem[2].Click += menuWindow_Click;
                menuItem[0].Text = "Открыть WinNote";
                menuItem[1].Text = "О WinNote";
                menuItem[2].Text = "Выход";
                notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(menuItem);
            }
            else if(e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                window.WindowState = WindowState.Normal;
                window.Show();                
            }
        }

        void menuWindow_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MenuItem menu = (System.Windows.Forms.MenuItem)sender;
            if (menu.Text == "Открыть WinNote")
            {
                window.Show();
                window.WindowState = WindowState.Normal;
            }
            else if (menu.Text == "О WinNote")
            {
                System.Windows.Forms.MessageBox.Show("Test");
            }
            else if (menu.Text == "Выход")
            {
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
                System.Windows.Application.Current.Shutdown();
                //window.Close();
            }
        }

        void BalloonInfo_Event_GetEvent(object sender, BalloonInfo e)
        {
            ShowBalloonMessage(e.mainInfo, e.info);
        }

        void ShowBalloonMessage(IEventMainInfo mainInfo, IEventInfo info)
        {
            if (mainInfo.dateFrom <= DateTime.Now && info.Reminded == 1 && info.Remind == 1)
            {

                this.mainInfo = mainInfo;
                this.info = info;

                String title = mainInfo.EventName;
                String text = String.Format("c: {0}\n по: {1}\nМосто нахождение: {2}\nОписание: {3}", mainInfo.dateFrom.ToLongDateString(),
                                mainInfo.dateTo.ToLongDateString(), info.location, info.description);

                notifyIcon.BalloonTipText = text;
                notifyIcon.BalloonTipTitle = title;
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(6000);
                notifyIcon.GetLifetimeService();
                SoundPlayer player = new SoundPlayer(Media.Sounds.crank_ring);
                player.Play();
            }
        }

    }
}
