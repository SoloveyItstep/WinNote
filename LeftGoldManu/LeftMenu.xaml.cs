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
using IventEnum;

namespace LeftGoldManu
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class LeftMenu : UserControl
    {
        private static double GroupSize { get; set; }
        private SolidColorBrush _colorTextBrush;
        private SolidColorBrush _colorSizeBrush;
        public List<string> _listCategori = new List<string>();
        public LeftMenu()
        {
            InitializeComponent();

            IventClass.GetListCategoriF += delegate(object sender, GetListCategoriF flag)
            {
                IventClass.SetListCategori(_listCategori);
            };

            AddItems("Общий баланс", 0, true);
            AddItems("Кошелек", 8000, false);
            AddItems("Счет в банке", 12000, false);
            //TODO Подписка на событие
            IventClass.GetSizePurse += delegate(object o, PurseGetSize size)
            {

                if (MainGrid.Children.Count > 1)
                {
                    for (int i = 1; i < MainGrid.Children.Count; i++)
                    {
                        if (((TextBlock)((Grid)MainGrid.Children[i]).Children[0]).Text == size.TextTransaction)
                        {
                            if (double.Parse(((TextBlock)((Grid)MainGrid.Children[i]).Children[1]).Text) >= size.SizeTransaction)
                            {
                                ((TextBlock)((Grid)MainGrid.Children[i]).Children[1]).Text = (double.Parse(((TextBlock)((Grid)MainGrid.Children[i]).Children[1]).Text) - size.SizeTransaction).ToString("F");
                                IventClass.SetFlagPurse();
                            }

                            Refresh();
                            break;
                        }
                    }
                }
            };
            //
        }

        public void AddItems(string text, double size, bool flag)
        {
            MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(75, GridUnitType.Pixel) });
            Grid itemGrid = new Grid();
            if (flag)
            {
                itemGrid.Background = new SolidColorBrush(Color.FromRgb(38, 165, 154));
                _colorTextBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                _colorSizeBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            else
            {
                itemGrid.Background = Brushes.Transparent;
                _colorTextBrush = new SolidColorBrush(Color.FromRgb(129, 130, 134));
                _colorSizeBrush = new SolidColorBrush(Color.FromRgb(38, 165, 154));
            }
            itemGrid.Children.Add(new TextBlock()
            {
                Text = text,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Foreground = _colorTextBrush,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(30, 10, 0, 0),
                FontSize = 22,
                Height = 25
            });
            itemGrid.Children.Add(new TextBlock()
            {
                Text = size.ToString("F"),
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
                Foreground = _colorSizeBrush,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(30, 0, 0, 10),
                FontSize = 35,
                Height = 40
            });
            MainGrid.Children.Add(itemGrid);
            Grid.SetRow(itemGrid, MainGrid.RowDefinitions.Count - 1);
            Refresh();
        }

        private void Refresh()
        {
            if (MainGrid.Children.Count > 1)
            {
                _listCategori.Clear();
                GroupSize = 0;
                for (int i = 1; i < MainGrid.Children.Count; i++)
                {
                    GroupSize += double.Parse(((TextBlock)((Grid)MainGrid.Children[i]).Children[1]).Text);
                    _listCategori.Add(((TextBlock)((Grid)MainGrid.Children[i]).Children[0]).Text);
                }
                ((TextBlock)((Grid)MainGrid.Children[0]).Children[1]).Text = GroupSize.ToString("F");
            }
        }

        public List<string> GetCategoriList()
        {
            return _listCategori;
        }
    }
}
