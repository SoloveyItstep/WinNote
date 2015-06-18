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
using ObjectCase_M_;

namespace EntryListControl
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class EntryList : UserControl
    {
        private double Size { get; set; }
        private static Brush _textBrush = new SolidColorBrush(Color.FromRgb(129, 130, 134));
        private static Brush _sizeBrush = new SolidColorBrush(Color.FromRgb(38, 165, 154));
        public EntryList()
        {
            InitializeComponent();
            IventClass.GetTopTextInfo += delegate(object o, InfoGetTopText size)
            {
                ButtonText.Text = size.TextTransaction;
            };
            IventClass.GetFlagRefresh += delegate(object sender, FlagGetRefresh refresh)
            {
                SetLiseElements(refresh.MainElement);
            };
        }

        public void SetLiseElements(List<NewInfoMainElement> list)
        {
            if (list != null)
            {
                ListBoxElements.Items.Clear();
                Size = 0;
                foreach (var element in list)
                {
                    Grid grid = new Grid()
                    {
                        Children =
                        {
                            new TextBlock()
                            {
                                Text =
                                    element.DateStart.Day.ToString("00") + "." + element.DateStart.Month.ToString("00"),
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Foreground = _textBrush
                            },
                            new TextBlock()
                            {
                                Text = "Из : " + element.TypePurse,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                Foreground = _textBrush
                            },
                            new TextBlock()
                            {
                                Text = element.Size + " ₴",
                                HorizontalAlignment = HorizontalAlignment.Right,
                                Foreground = _sizeBrush
                            }
                        },
                        HorizontalAlignment = HorizontalAlignment.Center,
                        MinWidth = 300
                    };
                    ListBoxElements.Items.Add(grid);
                    Size += element.Size;
                }
            }
            TextBlockRight.Text = Size.ToString("F") + " ₴";
            TextBlockLeft.Text = DateTime.Now.Month.ToString("00") + "." + DateTime.Now.Year.ToString("0000");
        }

        private void Reset_OnClick(object sender, RoutedEventArgs e)
        {
            IventClass.SetListOperations();
        }
    }
}
