﻿using notes.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace notes
{
	class ToggleFavoriteStringConverter : IValueConverter
	{
        public object Convert(object value, Type targetType,
       object parameter, string language)
        {
            // The value parameter is the data from the source object.
            bool isFav = (bool)value;
            if (isFav) {
                return MenuStrings.UNMARK_FAV;
            }
            else
			{
                return MenuStrings.MARK_FAV;
			}
        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
