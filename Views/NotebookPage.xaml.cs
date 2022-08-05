using notes.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
	public sealed partial class NotebookPage : Page
	{
		public NotebookPage()
		{
			this.InitializeComponent();
			
		}

		private void gridview_Tapped(object sender, TappedRoutedEventArgs e)
		{
			//((FrameworkElement)e.OriginalSource).DataContext;
			if (e.OriginalSource.GetType() == typeof(Grid)) return;
			((NotebooksViewModel)DataContext).OpenNotebookCommand.Execute(((FrameworkElement)e.OriginalSource).DataContext);
		}

		private void notebook_RightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			FrameworkElement senderElement = sender as FrameworkElement;
			FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
			var showOptions = new FlyoutShowOptions();
			showOptions.Placement = FlyoutPlacementMode.BottomEdgeAlignedRight;
			flyoutBase.ShowMode = FlyoutShowMode.Transient;
			flyoutBase.ShowAt(senderElement, showOptions);
		}

		private async void coverFilePickerBtn_Click(object sender, RoutedEventArgs e)
		{
			var picker = new Windows.Storage.Pickers.FileOpenPicker();
			picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
			picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
			picker.FileTypeFilter.Add(".jpg");
			picker.FileTypeFilter.Add(".jpeg");
			picker.FileTypeFilter.Add(".png");

			Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
			if (file != null)
			{
				((NotebooksViewModel)DataContext).SetCoverCommand.Execute(file);
				
			}
	
		}

		//private void coversGridview_SelectionChanged(object sender, SelectionChangedEventArgs e)
		//{
		//gridview.Items[0]
		//	((Grid)e.AddedItems.FirstOrDefault()).Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255,100,123,0));
		//}
	}
}
