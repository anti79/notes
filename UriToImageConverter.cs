using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace notes
{
	class UriToImageConverter:IValueConverter
	{
        public object Convert(object value, Type targetType,
        object parameter, string language)
        {
            // The value parameter is the data from the source object.
            ImageSource image = new BitmapImage((Uri)value);
            return image;
        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
