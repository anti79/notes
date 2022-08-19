using notes.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
			
			if (e.OriginalSource.GetType() == typeof(Grid)) return;
			(DataContext as NotebooksViewModel).OpenNotebookCommand.Execute(
				(e.OriginalSource as FrameworkElement).DataContext);
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
				(DataContext as NotebooksViewModel).SetCoverCommand.Execute(file);
				
			}
	
		}

		private async void defaultCover_Tapped(object sender, TappedRoutedEventArgs e)
		{
			string uri = (sender as FrameworkElement).DataContext as string;
			(DataContext as NotebooksViewModel).SetCoverCommand.Execute(
				await StorageFile.GetFileFromApplicationUriAsync(new Uri(uri))
				);
		}
	}
}
