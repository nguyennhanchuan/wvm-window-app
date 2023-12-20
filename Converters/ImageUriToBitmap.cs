using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace VendingMachine.Converters
{
    internal class ImageUriToBitmap : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = value as string;

            if (s == null)
                return null;

            using (WebClient client = new WebClient())
            {
                Stream stream = client.OpenRead(s);
                Bitmap bitmap; bitmap = new Bitmap(stream);
                stream.Flush();
                stream.Close();
                client.Dispose();
                return bitmap;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
