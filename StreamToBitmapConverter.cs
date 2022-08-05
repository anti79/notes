using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace notes
{
	public class StreamToBitmapConverter:IValueConverter
	{
        public object Convert(object value, Type targetType,
      object parameter, string language)
        {
            BitmapImage img = new BitmapImage();
            img.SetSource((IRandomAccessStream)value);
            return img;
        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
