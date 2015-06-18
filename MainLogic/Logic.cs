using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;
using ButtonMainManu;
using EntryListControl;
using IventEnum;
using ListProductInfo;
using ObjectCase_M_;
using StandartObjects;

namespace MainLogic
{
    public class Logic
    {
        private List<NewMainElement> ListGroup2 { get; set; }
        private WrapPanel wp1;
        private UserControlMainButton wControl;
        private ProductInfo MenuAddItems;
        public ProductInfo WControl1 { get; set; }

        public EntryListControl.EntryList WControl2 { get; set; }
        public Logic()
        {
            Standart.CreateStandartCategori();

            ListGroup2 = new List<NewMainElement>()
            {
                new NewMainElement(){NameGroup = "Еда", BigListImage = BigImgList.BigVegetarian, LitelListImage = LitelImgList.LitVegetarian, Img = BigImg.Grey, Сurrency = 0, ProgressEnd = 20000, FlagImages = true, ActivetedButton = true, ListElements = new List<NewInfoMainElement>()},
                new NewMainElement(){NameGroup = "Дети", BigListImage = BigImgList.BigBabies, LitelListImage = LitelImgList.LitBabies, Img = BigImg.Grey, Сurrency = 0, ProgressEnd = 20000, FlagImages = true, ActivetedButton = false, ListElements = new List<NewInfoMainElement>()},
                new NewMainElement(){NameGroup = "Поездки на авто", BigListImage = BigImgList.BigCar, LitelListImage = LitelImgList.LitCar, Img = BigImg.Grey, Сurrency = 0, ProgressEnd = 1478, FlagImages = true, ActivetedButton = false, ListElements = new List<NewInfoMainElement>()},
                new NewMainElement(){NameGroup = "Лекарство", BigListImage = BigImgList.BigPharmaceutical, LitelListImage = LitelImgList.LitPharmaceutical, Img = BigImg.Grey, Сurrency = 0, ProgressEnd = 1478, FlagImages = true, ActivetedButton = false, ListElements = new List<NewInfoMainElement>()}
            };

            IventClass.GetFlagMainManuItems += delegate(object sender, FlagGetMainManuItems items)
            {
                Getget(wp1, MenuAddItems);
            };
        }
        public void Getget(WrapPanel wp1, ProductInfo MenuAddItems)
        {
            this.wp1 = wp1;
            this.MenuAddItems = MenuAddItems;
            this.wp1.Children.Clear();
            foreach (var i in ListGroup2)
            {
                double sizeElements = 0.00;
                double fullSizeElements = 0.00;
                wControl = new UserControlMainButton();
                wControl.ContrilsAnimationsAddMenu = WControl1;
                wControl.ContrilsAnimationsListMenu = WControl2;
                fullSizeElements = OptionsControl(i.ListElements);
                if (fullSizeElements > i.ProgressEnd)
                {
                    sizeElements = i.ProgressEnd;
                    i.FlagImages = false;
                    i.Img = BigImg.Red;
                }
                else
                {
                    sizeElements = fullSizeElements;
                    i.FlagImages = true;
                    i.Img = BigImg.Grey;
                }
                wControl.MainButton(i, 135, 135, i.NameGroup, Standart.GetImageSource(new LogicImage().GetBigImage(i.BigListImage, i.Img)), i.FlagImages, i.ProgressEnd, sizeElements, i.ProgressEnd.ToString(), fullSizeElements + " ₴");
                wp1.Children.Add(wControl);
            }
            MenuAddItems.SetList(ListGroup2);
        }

        public double OptionsControl(List<NewInfoMainElement> list)
        {
            double sizeElements = 0.00;
            foreach (var element in list)
            {
                sizeElements += element.Size;
            }
            return sizeElements;
        }

        public string GetSerialization()
        {
            XmlSerializer writer = new XmlSerializer(typeof(List<NewMainElement>));

            using (StringWriter textWriter = new StringWriter())
            {
                writer.Serialize(textWriter, ListGroup2);
                return textWriter.ToString();
            }
        }
        public void GetDeserialization(string str)
        {
            XmlSerializer writer = new XmlSerializer(typeof(List<NewMainElement>));

            using (TextReader textReader = new StringReader(str))
            {
                ListGroup2.Clear();
                ListGroup2 = (List<NewMainElement>)writer.Deserialize(textReader);
                Getget(wp1, MenuAddItems);
            }
        }

    }
}
