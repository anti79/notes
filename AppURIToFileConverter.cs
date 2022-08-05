using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Data;

namespace notes
{
	class AppURIToFileConverter:IValueConverter
	{
        public object Convert(object value, Type targetType,
        object parameter, string language)
        {
            return StorageFile.GetFileFromApplicationUriAsync(new Uri((string)value));
        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

