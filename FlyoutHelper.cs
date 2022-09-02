using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace notes
{
    public static class FlyoutHelper
    {
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.RegisterAttached(
            "IsOpen", typeof(bool), typeof(FlyoutHelper),
            new PropertyMetadata(true, IsOpenChangedCallback));

        public static readonly DependencyProperty ParentProperty =
            DependencyProperty.RegisterAttached(
            "Parent", typeof(FrameworkElement), typeof(FlyoutHelper), null);

        public static void SetIsOpen(DependencyObject element, bool value)
        {
            element.SetValue(IsVisibleProperty, value);
        }

        public static bool GetIsOpen(DependencyObject element)
        {
            return (bool)element.GetValue(IsVisibleProperty);
        }

        private static void IsOpenChangedCallback(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var fb = d as FlyoutBase;
            if (fb == null)
                return;

            if ((bool)e.NewValue)
            {
                fb.Closed += flyout_Closed;
                //fb.ShowAt(GetParent(d));
                
                fb.ShowMode = FlyoutShowMode.Transient;
                var o = new FlyoutShowOptions();
                o.Placement = FlyoutPlacementMode.Bottom;
                var parent = GetParent(d);
                fb.ShowAt(parent, o);
                
            }
            else
            {
                fb.Closed -= flyout_Closed;
                fb.Hide();
            }
        }

        private static void flyout_Closed(object sender, object e)
        {
            // When the flyout is closed, sets its IsOpen attached property to false.
            SetIsOpen(sender as DependencyObject, false);
        }

        public static void SetParent(DependencyObject element, FrameworkElement value)
        {
            element.SetValue(ParentProperty, value);
        }

        public static FrameworkElement GetParent(DependencyObject element)
        {
            return element.GetValue(ParentProperty) as FrameworkElement;
        }
    }
}
