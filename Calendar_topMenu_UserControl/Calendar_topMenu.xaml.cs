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

namespace Calendar_topMenu_UserControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Calendar_topMenu_UserControl : UserControl
    {
        public Events events;
        public Calendar_topMenu_UserControl()
        {
            InitializeComponent();
            events = new Events(this);
            events.SetEvents();
            events.MainStyle();
        }
    }
}
