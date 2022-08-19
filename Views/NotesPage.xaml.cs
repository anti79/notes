using notes.Model;
using notes.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace notes.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class NotesPage : Page
	{
		public NotesPage()
		{
			this.InitializeComponent();
			
		}

		private void gridview_Tapped(object sender, TappedRoutedEventArgs e)
		{
			if (e.OriginalSource.GetType() == typeof(Grid)) return;
			(DataContext as NotesViewModel).OpenNoteCommand.Execute(
				(e.OriginalSource as FrameworkElement).DataContext);
		}


		private void RichEditBox_Loaded(object sender, RoutedEventArgs e)
		{
			var noteBox = sender as RichEditBox;
			
			noteBox.Document.SetText(TextSetOptions.FormatRtf, (noteBox.DataContext as Note).Content);
			noteBox.IsReadOnly = true;
			noteBox.Foreground = new SolidColorBrush(Colors.Black);
		}

		private void note_RightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			FrameworkElement senderElement = sender as FrameworkElement;
	
			FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
			var showOptions = new FlyoutShowOptions();
			showOptions.Placement = FlyoutPlacementMode.BottomEdgeAlignedRight;
			flyoutBase.ShowMode = FlyoutShowMode.Transient;
			flyoutBase.ShowAt(senderElement, showOptions);
			
		}
	}
}
