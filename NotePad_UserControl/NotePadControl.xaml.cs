using NotePad_UserControl.View;
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
using EventLibrsries;

namespace NotePad_UserControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class NotePad_Control : UserControl
    {
        public NotePad_Control()
        {
            InitializeComponent();
            new VisualStyle(this).SelectEvents();
            Notepad_M_.RichTextBoxElement = richTextBox;
            Notepad_M_ notepad = new Notepad_M_();
        }
    }
}
