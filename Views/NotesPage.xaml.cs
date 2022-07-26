using notes.Model;
using notes.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

		private void backBtn_Click(object sender, RoutedEventArgs e)
		{
			//((NotesViewModel)DataContext).Ex
		}

		private void gridview_Tapped(object sender, TappedRoutedEventArgs e)
		{
			if (e.OriginalSource.GetType() == typeof(Grid)) return;
			((NotesViewModel)DataContext).OpenNoteCommand.Execute(((FrameworkElement)e.OriginalSource).DataContext);
		}
	}
}
