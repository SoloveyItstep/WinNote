using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalendarAddMenu_UserControl
{
    /// <summary>
    /// Interaction logic for CalendarAddMenu_UserControl.xaml
    /// </summary>
    public partial class CalendarAddMenu_UserControl : UserControl
    {
        public Events events;
        public CalendarAddMenu_UserControl()
        {
            InitializeComponent();
            events = new Events(this);
            events.SetEvents();
        }
    }
}
