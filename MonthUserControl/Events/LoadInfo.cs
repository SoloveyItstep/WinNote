using EventLibrsries;
using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MonthUserControl.Events
{
    public class LoadInfo
    {
        Visual.VisualStyle visual;
        MonthControl control;
        

        public LoadInfo(MonthControl control)
        {
            this.control = control;
            visual = Visual.VisualStyle.GetInstance(control);
            GetSelectedDate_Event.GetDate += GetSelectedDate_Event_GetDate;
        }

        void GetSelectedDate_Event_GetDate(object sender, GetSelectedDate e)
        {
            visual.variables.selectedDate = e.date;
        }

        public void UpdateInfo()
        {
            ClearHyperlinks();
            visual.SetDays();
            
            String name = "";
            foreach(IEventMainInfo info in visual.eventInfo.dictionatyEventInfo.Keys)
            {
                name = GetLabelName(info.dateFrom.Day);
                if(name != "")
                    AddEvent(Int32.Parse(name), info.EventName);
            }
        }

        void ClearHyperlinks()
        {
            for(Int32 i = 1; i < 43; ++i)
            {
                for (Int32 h = 0; h < 5; ++h)
                {
                    visual.variables.daysHyperlinkDictionary["d" + i.ToString()][h].Inlines.Clear();
                }
            }
        }

        void AddEvent(Int32 num, String Name)
        {
            
            for(Int32 i = 0; i < 5;++i)
            {
                if (visual.variables.daysHyperlinkDictionary["d" + num.ToString()][i].Inlines.Count == 0)
                {
                    visual.variables.daysHyperlinkDictionary["d" + num.ToString()][i].Inlines.Add(Name);
                    break;
                }
                else if(i == 4)
                {
                    String txt = (visual.variables.daysHyperlinkDictionary["d" + num.ToString()][i].Inlines.FirstOrDefault() as Run).Text;
                    if (txt.Length > 8 && txt.Substring(txt.Length - 8) == " событий")
                    {
                        Int32 count = 0;
                        try
                        {
                            count = Int32.Parse(txt.Substring(0, txt.Length - 8)) + 1;
                        }
                        catch
                        {
                            count = 2;
                        }
                        visual.variables.daysHyperlinkDictionary["d" + num.ToString()][i].Inlines.Clear();
                        visual.variables.daysHyperlinkDictionary["d" + num.ToString()][i].Inlines.Add(String.Format("{0} событий", count));
                    }
                    else
                    {
                        visual.variables.daysHyperlinkDictionary["d" + num.ToString()][i].Inlines.Clear();
                        visual.variables.daysHyperlinkDictionary["d" + num.ToString()][i].Inlines.Add("2 событий");
                    }
                }
            }
        }

        String GetLabelName(Int32 day)
        {
                String name = "";
                foreach (Label label in visual.variables.labelsDaysNameList)
                {
                    if (label.Content.ToString() == day.ToString() && label.Foreground == Brushes.Black)
                        name = label.Name.Substring(1);
                }
            return name;
        }

        public void SetPopupEvents(String name, Int32 day, Int32 index, Boolean key)
        {
            control.dockPanelPopup.Children.Clear();
            Int32 dayCount = 0, count = 0, hyperlinkCount = 0;
            if (key)
            {
                visual.mypopup.popupHyperlinkDictionary.Clear();
                visual.mypopup.hyperlinkDay = 0;
                visual.mypopup.hyperlinkIndex.Clear();
            }

            foreach(var v in visual.eventInfo.dictionatyEventInfo)
            {
                if(v.Key.dateFrom.Day == day && v.Key.EventName == name)
                {
                    if(control.GridButtons.Visibility == System.Windows.Visibility.Hidden)
                        control.GridButtons.Visibility = System.Windows.Visibility.Visible;

                    visual.mypopup.currentEventMainInfo = null;
                    visual.mypopup.currentEventInfo = null;

                    String str = "";
                    str += String.Format("{0}\n\nДата с: {1}\nДата до:{2}\n",v.Key.EventName,v.Key.dateFrom,v.Key.dateTo);
                    str += String.Format("Описание: {0}\n\nМесто: {1}",v.Value.description,v.Value.location);
                    TextBlock txt = new TextBlock();
                    txt.TextWrapping = System.Windows.TextWrapping.Wrap;
                    txt.Text = str;
                    txt.Margin = new System.Windows.Thickness(10,10,10,10);

                    visual.mypopup.currentEventMainInfo = v.Key;
                    visual.mypopup.currentEventInfo = v.Value;

                    control.dockPanelPopup.Children.Add(txt);
                }
                if (name.Length > 8 && name.Substring(name.Length - 8) == " событий" && count >= 4 && v.Key.dateFrom.Day == day)
                {
                    if (control.GridButtons.Visibility == System.Windows.Visibility.Visible)
                        control.GridButtons.Visibility = System.Windows.Visibility.Hidden;

                    TextBlock txt = new TextBlock();
                    txt.TextWrapping = System.Windows.TextWrapping.Wrap;
                    txt.Margin = new System.Windows.Thickness(5, 20 * hyperlinkCount, 5, 5);
                    txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                    Hyperlink hyperlink = new Hyperlink();
                    hyperlink.Name = "h" + hyperlinkCount.ToString();
                    hyperlink.Inlines.Add(v.Key.EventName);
                    txt.Inlines.Add(hyperlink);

                    visual.mypopup.popupHyperlinkDictionary.Add(index + hyperlinkCount, hyperlink);
                    visual.mypopup.hyperlinkDay = day;
                    
                    hyperlink.PreviewMouseLeftButtonDown += hyperlink_MouseLeftButtonDown;
                    hyperlinkCount++;

                    control.dockPanelPopup.Children.Add(txt);
                }
                ++dayCount;
                if(v.Key.dateFrom.Day == day)
                    count++;
            }
        }

        void hyperlink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Hyperlink hyperlink = sender as Hyperlink;
            String name = (hyperlink.Inlines.FirstOrDefault() as Run).Text;
            Int32 countIndex = 0;
            foreach (Hyperlink hyp in visual.mypopup.popupHyperlinkDictionary.Values)
            {
                if (hyp.Name == hyperlink.Name)
                {
                    countIndex++;
                    break;
                }
                countIndex++;
            }

            SetPopupEvents(name, visual.mypopup.hyperlinkDay, countIndex, false);

        }
    }
}
