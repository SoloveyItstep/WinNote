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

namespace NotePad_UserControl.View
{
    public class ImagesSource
    {
        public ImageSource GetImageSource(Bitmap bitmap)
        {
            ImageBrush imageBrush = new ImageBrush()
            {
                ImageSource = Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions())
            };
            return imageBrush.ImageSource;
        }
    }
}
