using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Vpn.UI.Helper
{
    public static class Base64ImageToImage
    {
        public static BitmapImage ToBitmapImage(byte[] base64ImageBytes)
        {
            var bitmapImmage = new BitmapImage();
            bitmapImmage.BeginInit();
            bitmapImmage.StreamSource = new MemoryStream(base64ImageBytes);
            bitmapImmage.EndInit();

            return bitmapImmage;
        }
    }
}