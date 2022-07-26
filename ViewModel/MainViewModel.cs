﻿using notes.Model;
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
	class MainViewModel : ViewModel
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

		public ICommand SwitchToAll { get { return GetSwitchToAllCommand(); } }
		public ICommand SwitchToNotebooks { get { return GetSwitchToNotebooksCommand(); } }
		public ICommand SwitchToFavorites { get { return GetSwitchToFavoritesCommand(); } }
		public ICommand CreateNote { get { return GetCreateNoteCommand(); } }
		ICommand GetSwitchToAllCommand()
		{
			if (switchToAllCommand is null)
			{
				switchToAllCommand = new Command(() =>
				{
					CurrentPage = new NotesPage();
					CurrentPage.DataContext = allNotesVM;


				});
			}
			return switchToAllCommand;
		}
		ICommand GetSwitchToNotebooksCommand()
		{
			if (switchToNotebooksCommand is null)
			{
				switchToNotebooksCommand = new Command(() =>
				{
					CurrentPage = new NotebookPage();
					CurrentPage.DataContext = nbVM;
				});
			}
			return switchToNotebooksCommand;
		}

		ICommand GetSwitchToFavoritesCommand()
		{
			if (switchToFavoritesCommand is null)
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
		ICommand GetCreateNoteCommand()
		{
			if (createNoteCommand is null)
			{
				createNoteCommand = new Command(() =>
				{
					if (CurrentPage.GetType() == typeof(NotesPage) && ((NotesViewModel)((NotesPage)CurrentPage).DataContext).IsNotebook)
					{

						EditorPage = new EditorPage();
						var editorVM = new EditorViewModel((IEditorPage)EditorPage, new Note());
						editorVM.ParentViewModel = this;
						editorVM.NewNote = true;
						EditorPage.DataContext = editorVM;
						//Storage.Instance.CreateFile()



					}
				});
			}
			return createNoteCommand;
		}
		public ICommand CreateNoteCommand { get { return GetCreateNoteCommand(); } }

		ICommand openNote;

		ICommand GetOpenNoteCommand()
		{
			if(openNote is null)
			{
				openNote = new ActionCommand<Note>((note) =>
				{
					EditorPage = new EditorPage();
					var editorVM = new EditorViewModel((IEditorPage)EditorPage, note);
					editorVM.Note = note;
					editorVM.ParentViewModel = this;
					EditorPage.DataContext = editorVM;
				});
			}
			return openNote;
		}
		public ICommand OpenNoteCommand { get { return GetOpenNoteCommand(); } }

		//public bool 
	}
}
