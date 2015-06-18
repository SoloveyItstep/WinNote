using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ObjectCase_M_;

namespace StandartObjects
{
    public class Standart
    {
        public static void CreateStandartCategori()
        {
            //Babies Big
            BigImgList.BigBabies.Add(new ImageBig() { Key = BigImg.Grey, Value = "babies_gray" });
            BigImgList.BigBabies.Add(new ImageBig() { Key = BigImg.Blue, Value = "babies_blue" });
            BigImgList.BigBabies.Add(new ImageBig() { Key = BigImg.Red, Value = "babies_red" });
            //
            //Babies Litel
            LitelImgList.LitBabies.Add(new ImageLitel() { Key = LitelImg.Grey, Value = "babies_gray_32px" });
            LitelImgList.LitBabies.Add(new ImageLitel() { Key = LitelImg.White, Value = "babies_white_32px" });
            //
            //Baby Big
            BigImgList.BigCar.Add(new ImageBig() { Key = BigImg.Grey, Value = "car_gray" });
            BigImgList.BigCar.Add(new ImageBig() { Key = BigImg.Blue, Value = "car_blue" });
            BigImgList.BigCar.Add(new ImageBig() { Key = BigImg.Red, Value = "car_red" });
            //
            //Baby Litel
            LitelImgList.LitCar.Add(new ImageLitel() { Key = LitelImg.Grey, Value = "car_gray_32px" });
            LitelImgList.LitCar.Add(new ImageLitel() { Key = LitelImg.White, Value = "car_white_32px" });
            //
            //Pharmaceutical Big
            BigImgList.BigPharmaceutical.Add(new ImageBig() { Key = BigImg.Grey, Value = "pharmaceutical_gray" });
            BigImgList.BigPharmaceutical.Add(new ImageBig() { Key = BigImg.Blue, Value = "pharmaceutical_blue" });
            BigImgList.BigPharmaceutical.Add(new ImageBig() { Key = BigImg.Red, Value = "pharmaceutical_red" });
            //
            //Pharmaceutical Litel
            LitelImgList.LitPharmaceutical.Add(new ImageLitel() { Key = LitelImg.Grey, Value = "pharmaceutical_gray_32px" });
            LitelImgList.LitPharmaceutical.Add(new ImageLitel() { Key = LitelImg.White, Value = "pharmaceutical_white_32px" });
            //
            //Vegetarian Big
            BigImgList.BigVegetarian.Add(new ImageBig() { Key = BigImg.Grey, Value = "vegetarian_gray" });
            BigImgList.BigVegetarian.Add(new ImageBig() { Key = BigImg.Blue, Value = "vegetarian_blue" });
            BigImgList.BigVegetarian.Add(new ImageBig() { Key = BigImg.Red, Value = "vegetarian_red" });
            //
            //Vegetarian Litel
            LitelImgList.LitVegetarian.Add(new ImageLitel() { Key = LitelImg.Grey, Value = "vegetarian_gray_32px" });
            LitelImgList.LitVegetarian.Add(new ImageLitel() { Key = LitelImg.White, Value = "vegetarian_white_32px" });
            //

        }
        public static ImageSource GetImageSource(string str)
        {
            var bitmap = (Bitmap)Images.ResourceManager.GetObject(str);
            if (bitmap != null)
                return new ImageBrush()
                {
                    ImageSource = Imaging.CreateBitmapSourceFromHBitmap(
                        bitmap.GetHbitmap(),
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions())
                }.ImageSource;
            else
                return null;
        }
    }
}
