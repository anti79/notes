﻿using notes.Model;
using notes.ViewModel;
using notes.Views;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace notes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
           
            this.InitializeComponent();
            var vm = new MainViewModel();
            vm.MainPage = this;
            this.DataContext = vm;

        }
        public static void ShowMessageBox(string text)
        {
            MessageDialog md = new MessageDialog(text);
            md.ShowAsync();
        }

		private void searchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
		{
            var chosen = args.SelectedItem as ISearchable;
            var mainVM = DataContext as MainViewModel;
            mainVM.OpenItemCommand.Execute(chosen);
		}
	}
}
