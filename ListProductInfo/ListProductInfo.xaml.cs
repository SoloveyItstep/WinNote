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
using ObjectCase_M_;

namespace ListProductInfo
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class ProductInfo : UserControl
    {
        private Logic _logic;
        public ProductInfo()
        {
            InitializeComponent();
            _logic = new Logic(Resources["ButtonListing"] as Style, TextBoxSize, TypePurseComboBox, TypeResizeComboBox,
                GridDateSelection, GridDeysSelection, DataStart, DataEnd, TimeStart, TimeEnd);
            IndexUp.Click += _logic.IndexUp;
            IndexDown.Click += _logic.IndexDown;
            TextBoxSize.TextChanged += _logic.TextBoxSizeChanged;
            TypeResizeComboBox.SelectionChanged += _logic.TypeResizeSelectionChanged;
            TimeStart.SelectionChanged += _logic.TimesInfoSelectionChanged;
            TimeEnd.SelectionChanged += _logic.TimesInfoSelectionChanged;
            Refresh.Click += _logic.Button_Click;
            AddItem.Click += _logic.Button_Click_Add;
            _logic.CheckBoxInitialise();
        }

        public void SetList(List<NewMainElement> listMainElements)
        {
            _logic.SetListElements(PanelItems, listMainElements);
        }
    }
}
