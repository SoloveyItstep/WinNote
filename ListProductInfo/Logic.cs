using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using IventEnum;
using ObjectCase_M_;
using StandartObjects;

namespace ListProductInfo
{
    class Logic
    {
        #region objects
        private int _index = 0;
        private List<NewMainElement> _listMainElements;
        private StackPanel panelItems;
        private Grid gridDateSelection, gridDeysSelection;
        private TextBox textBoxSize, timeStart, timeEnd;
        private ComboBox typePurseComboBox, typeResizeComboBox;
        private DatePicker dateStart, dateEnd;
        private Style style;
        private static Brush _buttoBackground1 = new SolidColorBrush(Color.FromRgb(3, 169, 243));
        private static Brush _buttoBackground2 = new SolidColorBrush(Color.FromRgb(250, 250, 250));
        private static Brush _buttoTextForeground = new SolidColorBrush(Color.FromRgb(129, 130, 134));
        private static Brush _textForegroundOver = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        private static Brush _textForegroundNotOver = _buttoTextForeground;
        NewMainElement _startElement = new NewMainElement();
        private NewInfoMainElement _mainElement;
        string strMatch = "";
        #endregion
        #region ctor
        public Logic(Style style, TextBox textBoxSize, ComboBox typePurseComboBox, ComboBox typeResizeComboBox, Grid gridDateSelection, Grid gridDeysSelection, DatePicker dateStart, DatePicker dateEnd, TextBox timeStart, TextBox timeEnd)
        {
            this.style = style;
            this.textBoxSize = textBoxSize;
            this.gridDateSelection = gridDateSelection;
            this.gridDeysSelection = gridDeysSelection;
            this.typePurseComboBox = typePurseComboBox;
            this.typeResizeComboBox = typeResizeComboBox;
            this.dateStart = dateStart;
            this.dateEnd = dateEnd;
            this.timeStart = timeStart;
            this.timeEnd = timeEnd;

            IventClass.GetFlagPurse += delegate(object send, FlagGetPurse purse)
            {
                _startElement.ListElements.Add(_mainElement);
                IventClass.SetFlagRefresh(_startElement.ListElements);
            };
            IventClass.GetListCategoriIvent += delegate(object sender, GetListCategori categori)
            {
                foreach (var category in categori.List)
                {
                    typePurseComboBox.Items.Add(category);
                }
            };
            IventClass.SetListCategoriF();
        }
        #endregion
        #region methods

        public void CheckBoxInitialise()
        {
            
            typePurseComboBox.SelectedIndex = 0;
            typeResizeComboBox.SelectedIndex = 0;
        }
        public void SetListElements(StackPanel panelItems, List<NewMainElement> listMainElements)
        {
            this._listMainElements = listMainElements;
            this.panelItems = panelItems;
            Button button;
            Grid grid;
            LitelImg typeImg;
            Brush colorsBrush;
            this.panelItems.Children.Clear();
            for (int i = _index; i < _index + 3; i++)
            {
                if (_listMainElements.Count - 1 >= i && _listMainElements[i] != null)
                {
                    button = new Button() { Style = style, Tag = _listMainElements[i] };
                    button.MouseEnter += ButtonOnMouseEnter;
                    button.MouseLeave += ButtonOnMouseLeave;
                    button.PreviewMouseLeftButtonDown += MouseButtonDown;
                    if (_listMainElements[i].ActivetedButton)
                    {
                        typeImg = LitelImg.White;
                        button.Background = _buttoBackground1;
                        colorsBrush = _textForegroundOver;
                        _startElement = _listMainElements[i];
                    }
                    else
                    {
                        typeImg = LitelImg.Grey;
                        button.Background = _buttoBackground2;
                        colorsBrush = _buttoTextForeground;
                    }
                    grid = new Grid() { Width = 90, Height = 90 };
                    grid.Children.Add(new TextBlock()
                    {
                        Text = _listMainElements[i].NameGroup,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Top,
                        Foreground = colorsBrush,
                        FontWeight = FontWeights.Bold,
                        FontSize = 9,
                        Margin = new Thickness(0, 5, 0, 0)
                    });
                    grid.Children.Add(new Image()
                    {
                        Width = 32,
                        Height = 32,
                        Source = Standart.GetImageSource(new LogicImage().GetLitleImage(_listMainElements[i].LitelListImage, typeImg)),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Tag = _listMainElements[i].FlagImages
                    });
                    button.Content = grid;
                    this.panelItems.Children.Add(button);
                }
            }
        }

        public NewInfoMainElement GetElement()
        {
            double size = 0.00;
            DateTime dateEndTime = DateTime.Now;
            if (textBoxSize.Text != "")
            {
                size = double.Parse(textBoxSize.Text.Replace(".", ","), new CultureInfo(Thread.CurrentThread.CurrentCulture.Name));
            }
            if (dateEnd.Text != "")
            {
                dateEndTime = DateTime.Parse(dateEnd.Text);
            }
            return new NewInfoMainElement()
            {
                DateStart = dateEndTime,//dateStart.Text
                Text = timeStart.Text,
                Size = size,
                Name = "Test",
                TypePurse = typePurseComboBox.SelectedItem.ToString()
            };
        }

        private string TextSearch(string str)
        {
            try
            {
                Regex regex = new Regex(@"\d+([,.]\d{0,2})?");

                Match match = regex.Match(str);

                if (Thread.CurrentThread.CurrentCulture.Name != "fr-FR")
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("fr-FR");
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("fr-FR");
                }
                return match.Value;
            }
            catch
            {
                return "";
            }
        }

        private string TextTimeSearch(TextBox str)
        {
            try
            {
                if (str.Text != null)
                {
                    Regex regex = new Regex(@"^([0-9]|[0-1][0-9]|2[0-3])?(:)?([0-9]|[0-5][0-9])?$");
                    Match match = regex.Match(str.Text);
                    strMatch = match.Groups[1].Value + ":" + match.Groups[3].Value;
                    return strMatch;
                }
                else
                    throw new Exception();
            }
            catch
            {
                return strMatch;
            }
        }

        private void Refresh()
        {
            textBoxSize.Text = null;
            typePurseComboBox.Text = null;
            typePurseComboBox.SelectedIndex = 0;
            typeResizeComboBox.SelectedIndex = 0;
            foreach (var result in gridDeysSelection.Children.OfType<CheckBox>())
            {
                result.IsChecked = false;
            }
            dateStart.Text = "";
            dateEnd.Text = "";
            timeStart.Text = "00:00";
            timeEnd.Text = "00:00";
        }
        #endregion
        #region ivents
        public void TimesInfoSelectionChanged(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = TextTimeSearch(((TextBox)sender));
        }

        public void TextBoxSizeChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = TextSearch(((TextBox)sender).Text);
            ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length;
        }

        public void TypeResizeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedItem == ((ComboBox)sender).Items[0])
            {
                gridDateSelection.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Pixel);
                gridDateSelection.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);
                gridDeysSelection.Opacity = 0;
                gridDateSelection.Margin = new Thickness(30, 85, 30, 121);
            }
            else if (gridDateSelection.RowDefinitions[0].Height.Equals(new GridLength(0, GridUnitType.Pixel)))
            {
                gridDateSelection.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
                gridDateSelection.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                gridDeysSelection.Opacity = 1;
                gridDateSelection.Margin = new Thickness(30, 145, 30, 61);
            }

            if (((ComboBox)sender).SelectedItem == ((ComboBox)sender).Items[1])
            {
                gridDeysSelection.Opacity = 1;
                gridDateSelection.Margin = new Thickness(30, 196, 30, 10);
            }
            else if (gridDeysSelection.Opacity.Equals(1) && ((ComboBox)sender).SelectedItem != ((ComboBox)sender).Items[0])
            {
                gridDeysSelection.Opacity = 0;
                gridDateSelection.Margin = new Thickness(30, 145, 30, 61);
            }
        }

        private void MouseButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (((Button)sender).IsFocused != true)
            {
                foreach (var result in _listMainElements)
                {
                    if (result.ActivetedButton)
                    {
                        result.ActivetedButton = false;
                    }
                }
                ((NewMainElement)((Button)sender).Tag).ActivetedButton = true;
                IventClass.SetFlagRefresh(((NewMainElement)((Button)sender).Tag).ListElements);
                IventClass.SetTextInfo(((NewMainElement)((Button)sender).Tag).NameGroup);
                SetListElements(panelItems, _listMainElements);
            }
        }

        private void ButtonOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            if (((NewMainElement)((Button)sender).Tag).ActivetedButton != true)
            {
                Image img = (Image)((Grid)((Button)sender).Content).Children[1];
                img.Source = Standart.GetImageSource(new LogicImage().GetLitleImage(((NewMainElement)((Button)sender).Tag).LitelListImage, LitelImg.Grey));
                ((Button)sender).Background = _buttoBackground2;
                ((TextBlock)((Grid)((Button)sender).Content).Children[0]).Foreground = _textForegroundNotOver;
                ((Grid)((Button)sender).Content).Children[1] = img;
            }
        }

        private void ButtonOnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            if (((NewMainElement)((Button)sender).Tag).ActivetedButton != true)
            {
                Image img = (Image)((Grid)((Button)sender).Content).Children[1];
                img.Source = Standart.GetImageSource(new LogicImage().GetLitleImage(((NewMainElement)((Button)sender).Tag).LitelListImage, LitelImg.White));
                ((Button)sender).Background = _buttoBackground1;
                ((TextBlock)((Grid)((Button)sender).Content).Children[0]).Foreground = _textForegroundOver;
                ((Grid)((Button)sender).Content).Children[1] = img;
            }
        }

        public void IndexUp(object sender, RoutedEventArgs e)
        {
            if (_index > 0)
            {
                _index -= 3;
                panelItems.Children.Clear();
                SetListElements(panelItems, _listMainElements);
            }
        }

        public void IndexDown(object sender, RoutedEventArgs e)
        {
            if (_listMainElements != null)
            {
                if (_index < _listMainElements.Count - 3)
                {
                    _index += 3;
                    panelItems.Children.Clear();
                    SetListElements(panelItems, _listMainElements);
                }
            }
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        public void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            //TODO Реализация и отправка данных события
            _mainElement = GetElement();
            if (_mainElement.Size > 0.00)
            {
                IventClass.SetPurse(_mainElement.TypePurse, _mainElement.Size);
                IventClass.SetFlagMainManuItems();
            }
            Refresh();
        }
        #endregion
    }
}
