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

namespace DayUserControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DayControl : UserControl
    {
        public List<RowDefinition> rowList = new List<RowDefinition>();
        public Events events;
        public DayControl()
        {
            InitializeComponent();
            foreach (RowDefinition row in Grid_Day.RowDefinitions)
                rowList.Add(row);
            events = new Events(this);
            events.Event();
        }
    }
}
