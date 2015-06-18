using EventLibrsries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WinNote_LeftMenu
{
    class Events
    {
        LeftMenu leftMenu;
        ChangeMenuEvent changeEv;
        EventAboutNotepadPanelIsOpening_Event notepadGetData;

        public Events(LeftMenu window)
        {
            leftMenu = window;
            changeEv = new ChangeMenuEvent();
            notepadGetData = new EventAboutNotepadPanelIsOpening_Event();
        }

        public void SetEvents()
        {
            leftMenu.Grid_Calendar.MouseLeftButtonDown += Grid_MouseLeftButtonDown;
            leftMenu.Grid_NotePad.MouseDown += Grid_MouseLeftButtonDown;
            leftMenu.Grid_Wallet.MouseDown += Grid_MouseLeftButtonDown;
        }

        void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
                return;
            Grid grid = sender as Grid;
            if (leftMenu.Grid_Calendar.Height > 50 && leftMenu.Grid_Calendar.Height < 300 ||
                leftMenu.Grid_NotePad.Height > 50 && leftMenu.Grid_NotePad.Height < 300 ||
                leftMenu.Grid_Wallet.Height > 50 && leftMenu.Grid_Wallet.Height < 300)
                return;
            if (grid.Height > 50)
                return;
            DoubleAnimation anim_maxHeight_C = new DoubleAnimation(50, 300, TimeSpan.FromMilliseconds(400));
            DoubleAnimation anim_maxHeight_N = new DoubleAnimation(50, 300, TimeSpan.FromMilliseconds(400));
            DoubleAnimation anim_maxHeight_W = new DoubleAnimation(50, 300, TimeSpan.FromMilliseconds(400));
            DoubleAnimation anim_minHeight = new DoubleAnimation(300, 50, TimeSpan.FromMilliseconds(400));
            TranslateTransform trans = new TranslateTransform();

            if(grid.Name == "Grid_Calendar")
            {
                grid.Background = Brushes.White;
                anim_maxHeight_C.Completed += anim_maxHeight_Completed_C;
                grid.BeginAnimation(Rectangle.HeightProperty, anim_maxHeight_C);
                leftMenu.Grid_CalendarLine.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
                leftMenu.Image_Calendar_LeftGrid.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/calendar_blue.png"));
                leftMenu.Label_Calendar_LeftGrid.Foreground = new SolidColorBrush(Color.FromRgb(33, 150, 243));
                //leftMenu.CalendarLibrary.Visibility = System.Windows.Visibility.Visible;
                

                if(leftMenu.Grid_NotePad.Height == 300)
                {
                    leftMenu.Grid_NotePad.BeginAnimation(Rectangle.HeightProperty, anim_minHeight);
                    leftMenu.Grid_NotePad.RenderTransform = trans;
                    DoubleAnimation anim_marginTop = new DoubleAnimation(-250, 0, TimeSpan.FromMilliseconds(400));
                    trans.BeginAnimation(TranslateTransform.YProperty, anim_marginTop);
                    leftMenu.Grid_NotePadLine.Background = Brushes.LightGray;
                    leftMenu.Image_NotePad_LeftGrid.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/notebook_gray.png"));
                    leftMenu.Grid_NotePad.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                    leftMenu.Label_NotePad_LeftGrid.Foreground = new SolidColorBrush(Color.FromRgb(167, 167, 167));
                }
                else if(leftMenu.Grid_Wallet.Height == 300)
                {
                    leftMenu.Grid_WalletLine.Background = Brushes.LightGray;
                    leftMenu.Grid_Wallet.BeginAnimation(Rectangle.HeightProperty, anim_minHeight);
                    leftMenu.Grid_Wallet.RenderTransform = trans;
                    DoubleAnimation anim_marginTop = new DoubleAnimation(-250, 0, TimeSpan.FromMilliseconds(400));
                    trans.BeginAnimation(TranslateTransform.YProperty, anim_marginTop);
                    leftMenu.Grid_NotePad.RenderTransform = trans;
                    trans.BeginAnimation(TranslateTransform.YProperty, anim_marginTop);
                    leftMenu.Grid_Wallet.RenderTransform = trans;
                    trans.BeginAnimation(TranslateTransform.YProperty, anim_marginTop);
                    leftMenu.Image_Wallet_LeftGrid.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/wallet_gray.png"));
                    leftMenu.Grid_Wallet.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                    leftMenu.Label_Wallet_LeftGrid.Foreground = new SolidColorBrush(Color.FromRgb(167, 167, 167));
                }
                changeEv.ChangeEventMessage("calendar");
            }
            else if (grid.Name == "Grid_NotePad")
            {
                grid.Background = Brushes.White;
                anim_maxHeight_N.Completed += anim_maxHeight_Completed_N;
                grid.BeginAnimation(Rectangle.HeightProperty, anim_maxHeight_N);
                leftMenu.Grid_NotePadLine.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
                leftMenu.Image_NotePad_LeftGrid.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/notebook_blue.png"));
                leftMenu.Label_NotePad_LeftGrid.Foreground = new SolidColorBrush(Color.FromRgb(33, 150, 243));
                
                if (leftMenu.Grid_Calendar.Height == 300)
                {
                    leftMenu.Grid_Calendar.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                    leftMenu.Image_Calendar_LeftGrid.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/calendar_gray.png"));
                    leftMenu.Grid_CalendarLine.Background = Brushes.LightGray;
                    leftMenu.Label_Calendar_LeftGrid.Foreground = new SolidColorBrush(Color.FromRgb(167, 167, 167));
                    leftMenu.Grid_Calendar.BeginAnimation(Rectangle.HeightProperty, anim_minHeight);
                    leftMenu.CalendarLibrary.Visibility = System.Windows.Visibility.Hidden;
                    DoubleAnimation anim_marginTop = new DoubleAnimation(0, -250, TimeSpan.FromMilliseconds(400));
                    leftMenu.Grid_NotePad.RenderTransform = trans;
                    trans.BeginAnimation(TranslateTransform.YProperty, anim_marginTop);
                }
                else if (leftMenu.Grid_Wallet.Height == 300)
                {
                    leftMenu.Grid_Wallet.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                    leftMenu.Image_Wallet_LeftGrid.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/wallet_gray.png"));
                    leftMenu.Grid_WalletLine.Background = Brushes.LightGray;
                    leftMenu.Label_Wallet_LeftGrid.Foreground = new SolidColorBrush(Color.FromRgb(167, 167, 167));
                    leftMenu.Grid_Wallet.BeginAnimation(Rectangle.HeightProperty, anim_minHeight);
                    DoubleAnimation anim_marginTop = new DoubleAnimation(-250, 0, TimeSpan.FromMilliseconds(400));
                    leftMenu.Grid_Wallet.RenderTransform = trans;
                    trans.BeginAnimation(TranslateTransform.YProperty, anim_marginTop);
                }
                changeEv.ChangeEventMessage("notepad");
                notepadGetData.SendEvent(true);
            }
            else if (grid.Name == "Grid_Wallet")
            {
                grid.Background = Brushes.White;
                anim_maxHeight_W.Completed += anim_maxHeight_Completed_W;
                grid.BeginAnimation(Rectangle.HeightProperty, anim_maxHeight_W);
                leftMenu.Grid_WalletLine.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
                leftMenu.Image_Wallet_LeftGrid.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/wallet_blue.png"));
                leftMenu.Label_Wallet_LeftGrid.Foreground = new SolidColorBrush(Color.FromRgb(33, 150, 243));

                if (leftMenu.Grid_Calendar.Height == 300)
                {
                    leftMenu.Grid_Calendar.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                    leftMenu.Image_Calendar_LeftGrid.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/calendar_gray.png"));
                    leftMenu.Grid_CalendarLine.Background = Brushes.LightGray;
                    leftMenu.Label_Calendar_LeftGrid.Foreground = new SolidColorBrush(Color.FromRgb(167, 167, 167));
                    leftMenu.Grid_Calendar.BeginAnimation(Rectangle.HeightProperty, anim_minHeight);
                    leftMenu.CalendarLibrary.Visibility = System.Windows.Visibility.Hidden;
                    DoubleAnimation anim_marginTop = new DoubleAnimation(0, -250, TimeSpan.FromMilliseconds(400));
                    //trans.BeginAnimation(TranslateTransform.YProperty, anim_marginTop);
                    grid.RenderTransform = trans;
                    leftMenu.Grid_NotePad.RenderTransform = trans;
                    trans.BeginAnimation(TranslateTransform.YProperty, anim_marginTop);

                }
                else if (leftMenu.Grid_NotePad.Height == 300)
                {
                    leftMenu.Grid_NotePad.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                    leftMenu.Image_NotePad_LeftGrid.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/notebook_gray.png"));
                    leftMenu.Grid_NotePadLine.Background = Brushes.LightGray;
                    leftMenu.Label_NotePad_LeftGrid.Foreground = new SolidColorBrush(Color.FromRgb(167, 167, 167));
                    leftMenu.Grid_NotePad.BeginAnimation(Rectangle.HeightProperty, anim_minHeight);
                    DoubleAnimation anim_marginTop = new DoubleAnimation(0, -250, TimeSpan.FromMilliseconds(400));
                    grid.RenderTransform = trans;
                    trans.BeginAnimation(TranslateTransform.YProperty, anim_marginTop);
                }
                changeEv.ChangeEventMessage("wallet");
            }
        }

        void anim_maxHeight_Completed_C(object sender, EventArgs e)
        {
            if (leftMenu.CalendarLibrary.Visibility == System.Windows.Visibility.Hidden)
            {
                leftMenu.CalendarLibrary.Visibility = System.Windows.Visibility.Visible;
                leftMenu.GoldLeftMenu.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (leftMenu.CalendarLibrary.Visibility == System.Windows.Visibility.Visible)
                leftMenu.CalendarLibrary.Visibility = System.Windows.Visibility.Hidden;
        }
        void anim_maxHeight_Completed_N(object sender, EventArgs e)
        {

        }
        void anim_maxHeight_Completed_W(object sender, EventArgs e)
        {
            if (leftMenu.GoldLeftMenu.Visibility == System.Windows.Visibility.Hidden)
            {
                leftMenu.GoldLeftMenu.Visibility = System.Windows.Visibility.Visible;
                leftMenu.CalendarLibrary.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (leftMenu.GoldLeftMenu.Visibility == System.Windows.Visibility.Visible)
                leftMenu.GoldLeftMenu.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
