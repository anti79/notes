using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace notes.Views
{
    public sealed class CustomRichEditBox : RichEditBox
    {
        public CustomRichEditBox()
        {
            this.DefaultStyleKey = typeof(RichEditBox);
            this.TextChanged += CustomRichEditBox_TextChanged;
        }

        private void CustomRichEditBox_TextChanged(object sender, RoutedEventArgs e)
        {
            string value = string.Empty;
            this.Document.GetText(Windows.UI.Text.TextGetOptions.AdjustCrlf, out value);
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            //CustomText = value;
        }

        public string CustomText
        {
            get { return (string)GetValue(CustomTextProperty); }
            set
            {
                SetValue(CustomTextProperty, value);
            }
        }

        public static readonly DependencyProperty CustomTextProperty =
            DependencyProperty.Register("CustomText", typeof(string), typeof(CustomRichEditBox), new PropertyMetadata(null, new PropertyChangedCallback(OnCustomTextChanged)));

        private static void OnCustomTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomRichEditBox rich = d as CustomRichEditBox;
            if (e.NewValue != e.OldValue)
            {
                rich.Document.SetText(Windows.UI.Text.TextSetOptions.None, e.NewValue.ToString());
            }
        }
    }
}
