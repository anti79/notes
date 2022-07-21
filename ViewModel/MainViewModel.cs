using notes.Model;
using notes.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace notes.ViewModel
{
	class MainViewModel:ViewModel
	{
		Page currentPage;
		public Page CurrentPage { get
			{
				return currentPage;
			}
			set
			{
				currentPage = value;
				RaisePropertyChanged(nameof(CurrentPage));

			}
		}
		public MainViewModel()
		{
			CurrentPage = new NotebookPage();
		}



		ICommand switchToAllCommand;
		ICommand switchToNotebooksCommand;
		ICommand switchToFavoritesCommand;



		public ICommand SwitchToAll { get { return GetSwitchToAllCommand(); }}
		public ICommand SwitchToNotebooks { get { return GetSwitchToNotebooksCommand(); }}
		public ICommand SwitchToFavorites { get { return GetSwitchToFavoritesCommand(); }}
		public ICommand GetSwitchToAllCommand()
		{
			if(switchToAllCommand is null)
			{
				switchToAllCommand = new Command(() =>
				{
					CurrentPage = new NotesPage();
					var vm = new NotesViewModel(); ;
					vm.GetNotesDelegate = () =>
					{
						return Storage.Instance.GetAllNotes();
					};


				});
			}
			return switchToAllCommand;
		}
		public ICommand GetSwitchToNotebooksCommand()
		{
			if(switchToNotebooksCommand is null)
			{
				switchToNotebooksCommand = new Command(() =>
				{
					CurrentPage = new NotebookPage();
				});
			}
			return switchToNotebooksCommand;
		}
		public ICommand GetSwitchToFavoritesCommand()
		{
			if(switchToFavoritesCommand is null)
			{
				switchToFavoritesCommand = new Command(() =>
				{
					CurrentPage = new NotesPage();
					var vm = new NotesViewModel();
					vm.GetNotesDelegate = () =>
					{
						return Storage.Instance.GetFavoriteNotes();
					};
					CurrentPage.DataContext = vm;

				}
				);
			}
			return switchToFavoritesCommand;
		}
	
	}
}
