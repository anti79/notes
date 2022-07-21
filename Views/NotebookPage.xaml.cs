﻿using notes.ViewModel;
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
			((NotebooksViewModel)DataContext).OpenNotebookCommand.Execute(((FrameworkElement)e.OriginalSource).DataContext);
		}
	}
}