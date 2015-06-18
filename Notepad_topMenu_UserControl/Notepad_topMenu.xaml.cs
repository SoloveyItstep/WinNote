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
using Notepad;

namespace Notepad_topMenu_UserControl
{
    /// <summary>
    /// Interaction logic for Notepad_topMenu.xaml
    /// </summary>
    public partial class Notepad_topMenu : UserControl
    {
        public Notepad_topMenu()
        {
            InitializeComponent();
            new Events(this).SelectEvents();
            Notepad_M_.ComboBox_FontStyle = ComboBox_FontStyle;
            Notepad_M_.ComboBox_FontSize = ComboBox_FontSize;
            Notepad_M_.Button_Color = Button_Color;
            Notepad_M_.Button_BoldFontFamily = Button_BoldFontFamily;
            Notepad_M_.Button_ItalicFontFamily = Button_ItalicFontFamily;
            Notepad_M_.Button_Underlining = Button_Underlining;
        }
    }
}
