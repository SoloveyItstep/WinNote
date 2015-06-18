using Day_UserControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Day_UserControl.VisualStyles
{
    public class ShowSelectingEvent
    {
        Variables variables;
        public ShowSelectingEvent()
        {
            variables = Variables.GetInstance;
        }
        public void FillGrids(Double Y)
        {
            foreach (Grid grid in variables.gridList)
                grid.Background = Brushes.White;

            Double h = Y % 25;
            Int32 currentIndex = Int32.Parse(((Y - 50 - h) / 25).ToString());
            
            if(currentIndex <= variables.FirstSelectedIndex)
            {
                for(Int32 i = 0; i < 48; ++i)
                {
                    if(i >= currentIndex && i <= variables.FirstSelectedIndex)
                    {
                        variables.gridList[i].Background = new SolidColorBrush(Color.FromRgb(128, 203, 196));
                    }
                }
            }
            else
            {
                for (Int32 i = 0; i < 48; ++i)
                {
                    if (i >= variables.FirstSelectedIndex && i <= currentIndex)
                    {
                        variables.gridList[i].Background = new SolidColorBrush(Color.FromRgb(128, 203, 196));
                    }
                }
            }
            variables.LastSelectedIndex = currentIndex;
        }
    }
}
