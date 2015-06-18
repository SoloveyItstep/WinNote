using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NotePad_UserControl.View
{
    public class NotepadColor
    {
        private static NotepadColor color { get; set; }
        private NotepadColor(){}
        public static NotepadColor GetInstance
        {
            get
            {
                if (color == null)
                    color = new NotepadColor();
                return color;
            }
            set
            {
                color = value;
            }
        }

        public SolidColorBrush GetCategoryBackgroundColor()
        {
            return new SolidColorBrush(Color.FromRgb(250,250,250));
        }

        public SolidColorBrush GetCategoryBorderHoverColor()
        {
            return new SolidColorBrush(Color.FromRgb(30, 136, 229));
        }
    }
}
