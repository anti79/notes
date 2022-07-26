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
		Page editorPage;

		NotesViewModel allNotesVM;
		NotebooksViewModel nbVM;
		NotesViewModel favVM;

	 


		public bool EditorVisible
		{
		get
			{
				return EditorPage != null;
			}
		}

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
		public Page EditorPage
		{
			get
			{
				return editorPage;

			}
			set
			{
				editorPage = value;
				RaisePropertyChanged();
				RaisePropertyChanged(nameof(EditorVisible));
			}
		}
		public MainViewModel()
		{
			CurrentPage = new NotebookPage();
			var vm = new NotebooksViewModel();
			vm.ParentViewModel = this;
			CurrentPage.DataContext = vm;

			allNotesVM = new NotesViewModel(); ;
			allNotesVM.Title = "All notes";
			allNotesVM.GetNotesDelegate = () =>
			{
				return Storage.Instance.GetAllNotes();
			};
			allNotesVM.ParentViewModel = this;

			nbVM = new NotebooksViewModel();
			nbVM.ParentViewModel = this;

			favVM = new NotesViewModel();
			favVM.Title = "Favorites";
			favVM.GetNotesDelegate = () =>
			{
				return Storage.Instance.GetFavoriteNotes();
			};
			favVM.ParentViewModel = this;

		}



		ICommand switchToAllCommand;
		ICommand switchToNotebooksCommand;
		ICommand switchToFavoritesCommand;
		ICommand createNoteCommand;

		public Notebook OpenedNotebook { get; set; }

		public ICommand SwitchToAll { get { return GetSwitchToAllCommand(); }}
		public ICommand SwitchToNotebooks { get { return GetSwitchToNotebooksCommand(); }}
		public ICommand SwitchToFavorites { get { return GetSwitchToFavoritesCommand(); }}
		public ICommand CreateNote { get { return GetCreateNoteCommand(); }}
		public ICommand GetSwitchToAllCommand()
		{
			if(switchToAllCommand is null)
			{
				switchToAllCommand = new Command(() =>
				{
					CurrentPage = new NotesPage();
					CurrentPage.DataContext = allNotesVM;


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
					CurrentPage.DataContext = nbVM;
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
					CurrentPage.DataContext = favVM;

				}
				);
			}
			return switchToFavoritesCommand;
		}
		public ICommand GetCreateNoteCommand()
		{
			if (createNoteCommand is null)
			{
				createNoteCommand = new Command(() =>
				{
					if(CurrentPage.GetType()==typeof(NotesPage) && ((NotesViewModel)((NotesPage)CurrentPage).DataContext).IsNotebook)
					{
						//OpenedNotebook.Notes.Add(new Note());
						//((NotesViewModel)(CurrentPage.DataContext)).Notes.Add(new Note());
						//((NotesViewModel)(CurrentPage.DataContext)).RaisePropertyChanged("Notes");
					
						EditorPage = new EditorPage();
						var editorVM = new EditorViewModel((IEditorPage)EditorPage);
						editorVM.Note = new Note();
						editorVM.ParentViewModel = this;
						EditorPage.DataContext = editorVM;
						//Storage.Instance.CreateFile()
						


					}
				});
			}
			return createNoteCommand;
		}
		public ICommand CreateNoteCommand { get { return GetCreateNoteCommand(); } }

		//public bool 
	}
}
