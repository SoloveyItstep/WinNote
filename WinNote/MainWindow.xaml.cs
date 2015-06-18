using EventLibrsries;
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
using MainLogic;
using WinNote.Control;
using WinNote.Control.Events;
using WinNote.View;

namespace WinNote
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        View.View logRegView;
        //Boolean dragKey = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && 
                e.ChangedButton == MouseButton.Left && 
                Grid_firstPage.Visibility == System.Windows.Visibility.Visible)
                this.DragMove();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            logRegView = new View.View(this);
            new AddEventMenu_Event().ChangeMainWindowEvent(this.Height);

            //M
            Logic cl = new Logic();
            cl.WControl1 = UserControl12;
            cl.WControl2 = UserControl123;
            cl.Getget(UserControl1.MainwWrapPanel, UserControl12);
            //MessageBox.Show(cl.GetSerialization());
            //cl.GetDeserialization(cl.GetSerialization());
            //M
        }

        private void Grid_TopPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount >= 2)
            {
               // dragKey = false;
                if (this.WindowState == System.Windows.WindowState.Maximized)
                    this.WindowState = System.Windows.WindowState.Normal;
                else if (this.WindowState == System.Windows.WindowState.Normal)
                {
                    this.MaxHeight = SystemParameters.WorkArea.Height + 13;
                    this.WindowState = System.Windows.WindowState.Maximized;
                }
                return;
            }

            if (e.ButtonState == MouseButtonState.Pressed && e.ChangedButton == MouseButton.Left && Grid_firstPage.Visibility == System.Windows.Visibility.Hidden)
            {
                this.DragMove();
            }
        }

    }
}
