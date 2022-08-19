using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace notes
{
	public class ColorToBrushConverter : IValueConverter
	{
        public object Convert(object value, Type targetType,
        object parameter, string language)
        {
            // The value parameter is the data from the source object.
            Color thisColor = (Color)value;
            return new SolidColorBrush(thisColor);
        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            SolidColorBrush thisBrush = value as SolidColorBrush;
            return thisBrush.Color;
        }
    }
}
