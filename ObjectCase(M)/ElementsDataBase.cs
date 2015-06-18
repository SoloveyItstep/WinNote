using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectCase_M_
{
    [Serializable]
    public enum BigImg
    {
        Grey = 0,
        Red = 1,
        Blue = 2
    }
    [Serializable]
    public enum LitelImg
    {
        Grey = 0,
        White = 1
    }
    [Serializable]
    public class ImageBig
    {
        public BigImg Key { get; set; }
        public string Value { get; set; }
    }
    [Serializable]
    public class ImageLitel
    {
        public LitelImg Key { get; set; }
        public string Value { get; set; }
    }
    [Serializable]
    public class NewMainElement
    {
        public string NameGroup { get; set; }
        public BigImg Img { get; set; }
        public double Сurrency { get; set; }
        public double ProgressEnd { get; set; }
        public List<NewInfoMainElement> ListElements { get; set; }
        public bool FlagImages { get; set; }
        public bool ActivetedButton { get; set; }
        public List<ImageBig> BigListImage { get; set; }
        public List<ImageLitel> LitelListImage { get; set; }
    }
    [Serializable]
    public class NewInfoMainElement
    {
        public DateTime DateStart { get; set; }
        public string Name { get; set; }
        public string TypePurse { get; set; }
        public string Text { get; set; }
        public double Size { get; set; }
    }
    public class LogicImage
    {
        public string GetBigImage(List<ImageBig> bigListImage, BigImg img)
        {
            string str = "";
            foreach (var image in bigListImage)
            {
                if (image.Key == img)
                {
                    str = image.Value;
                    break;
                }
            }
            return str;
        }
        public string GetLitleImage(List<ImageLitel> bigListImage, LitelImg img)
        {
            string str = "";
            foreach (var image in bigListImage)
            {
                if (image.Key == img)
                {
                    str = image.Value;
                    break;
                }
            }
            return str;
        }
    }
}
