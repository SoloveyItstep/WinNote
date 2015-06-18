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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IventEnum;
using ObjectCase_M_;
using StandartObjects;

namespace ButtonMainManu
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControlMainButton : UserControl
    {
        public ListProductInfo.ProductInfo ContrilsAnimationsAddMenu { get; set; }
        public EntryListControl.EntryList ContrilsAnimationsListMenu { get; set; }
        public UserControlMainButton()
        {
            InitializeComponent();

            IventClass.GetListOperations += delegate(object o, OperationsGetList size)
            {
                ((Grid)ContrilsAnimationsAddMenu.Parent).ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                ((Grid)ContrilsAnimationsAddMenu.Parent).ColumnDefinitions[1].Width = new GridLength(0);
            };
        }
        public void MainButton(NewMainElement buttonNewElements, double gridWidth, double gridHeight, string topTextBlockText, ImageSource image, bool imageFlag, double maximumProgresBar, double valueProgresBar, string leftTextBlock, string rightTextBlock)
        {
            Button.MouseEnter += ButtonOnMouseEnter;
            Button.MouseLeave += ButtonOnMouseLeave;
            Button.PreviewMouseDown += MouseDowns;
            this.Tag = buttonNewElements;
            MainGrid.Width = gridWidth;
            MainGrid.Height = gridHeight;
            TextBlockTop.Text = topTextBlockText;
            ImageChildren.Source = image;
            ImageChildren.Tag = imageFlag;
            ProgressBarCumm.Maximum = maximumProgresBar;
            ProgressBarCumm.Value = valueProgresBar;
            TextBlockLeft.Text = leftTextBlock;
            TextBlockRight.Text = rightTextBlock;
        }

        private void ButtonOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            Image img = (Image)((Grid)((Button)sender).Content).Children[1];
            if ((bool)img.Tag)
            {
                img.Source = Standart.GetImageSource(new LogicImage().GetBigImage(((NewMainElement)this.Tag).BigListImage, BigImg.Grey));
            }
            else
            {
                img.Source = Standart.GetImageSource(new LogicImage().GetBigImage(((NewMainElement)this.Tag).BigListImage, BigImg.Red));
            }
            ((ProgressBar)((Grid)((Button)sender).Content).Children[2]).BorderBrush = new SolidColorBrush(Color.FromRgb(129, 130, 134));
            ((TextBlock)((Grid)((Button)sender).Content).Children[0]).Foreground = new SolidColorBrush(Color.FromRgb(129, 130, 134));
            ((Grid)((Button)sender).Content).Children[1] = img;
        }

        private void ButtonOnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            Image img = (Image)((Grid)((Button)sender).Content).Children[1];
            img.Source = Standart.GetImageSource(new LogicImage().GetBigImage(((NewMainElement)this.Tag).BigListImage, BigImg.Blue));
            ((Grid)((Button)sender).Content).Children[1] = img;
            ((ProgressBar)((Grid)((Button)sender).Content).Children[2]).BorderBrush = new SolidColorBrush(Color.FromRgb(3, 169, 243));
            ((TextBlock)((Grid)((Button)sender).Content).Children[0]).Foreground = new SolidColorBrush(Color.FromRgb(3, 169, 243));
        }

        public void MouseDowns(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (((Grid)ContrilsAnimationsAddMenu.Parent).ColumnDefinitions[1].Width.Value == 0)
            {
                WrapPanel mainGrid = ((WrapPanel)this.Parent);
                foreach (var result in mainGrid.Children.OfType<UserControlMainButton>())
                {
                    if (((NewMainElement)result.Tag).ActivetedButton)
                    {
                        ((NewMainElement)result.Tag).ActivetedButton = false;
                    }
                }
                ((NewMainElement)this.Tag).ActivetedButton = true;
                List<NewMainElement> list = new List<NewMainElement>();
                foreach (var result in mainGrid.Children.OfType<UserControlMainButton>())
                {
                    list.Add(((NewMainElement)result.Tag));
                }
                ContrilsAnimationsListMenu.SetLiseElements(((NewMainElement)this.Tag).ListElements);
                ContrilsAnimationsAddMenu.SetList(list);
                //TODO Отправка названия категории.
                IventClass.SetTextInfo(((TextBlock)((Grid)((Button)sender).Content).Children[0]).Text);
                //
                //TODO Выводит контрол и все записи в нем.
                ((Grid)ContrilsAnimationsAddMenu.Parent).ColumnDefinitions[0].Width = new GridLength(0);
                ((Grid)ContrilsAnimationsAddMenu.Parent).ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                //
                AnimationsInfoMenu_Opasity(0, 1, 0.2, ContrilsAnimationsListMenu);
            }
        }
        private void AnimationsInfoMenu_Opasity(double startSize, double endSize, double timeAnimations, UserControl control)
        {
            var animation = new DoubleAnimation();
            animation.From = startSize;
            animation.To = endSize;
            animation.Duration = TimeSpan.FromSeconds(timeAnimations);
            control.BeginAnimation(OpacityProperty, animation);
        }
    }
}
