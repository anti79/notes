﻿using notes.Model;
using notes.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Input;
using Windows.UI.Xaml.Input;

namespace notes.ViewModel
{
	class NotebooksViewModel : ViewModel
	{
		ObservableCollection<Notebook> notebooks;
		public ObservableCollection<Notebook> Notebooks { get
			{
				if (notebooks is null)
				{
					notebooks = new ObservableCollection<Notebook>(Storage.Instance.Notebooks);
				}
				return notebooks;
			}
		}

		public NotebooksViewModel()
		{
		}

		ICommand openNotebook;
		ICommand createNotebook;
		ICommand openEdit;

		public ICommand OpenNotebookCommand { get
			{
				return GetOpenNotebookCommand();
			}
		}
		public ICommand CreateNotebookCommand
		{
			get
			{
				return GetCreateNotebookCommand();
			}
		}

		ICommand GetCreateNotebookCommand()
		{
			if (createNotebook is null)
			{
				createNotebook = new Command(async () => {
					var nb = new Notebook() { Title = "Untitled" };
					nb.CoverImage = await Storage.Instance.GetDefaultCover();
					Notebooks.Add(nb);
					Storage.Instance.SaveNotebook(nb);

				});
			}
			return createNotebook;
		}

		ICommand GetOpenNotebookCommand()
		{
			if (openNotebook is null)
			{
				openNotebook = new ActionCommand<Notebook>((nb) => {
					Console.WriteLine(nb);
					var page = new NotesPage();
					var vm = new NotesViewModel();
					vm.GetNotesDelegate = () =>
					{
						return nb.Notes;
					};
					vm.Title = nb.Title;
					vm.IsNotebook = true;
					page.DataContext = vm;
					vm.ParentViewModel = ParentViewModel;
					((MainViewModel)ParentViewModel).CurrentPage = page;
					((MainViewModel)ParentViewModel).OpenedNotebook = nb;
				});

			}
			return openNotebook;
		}

		bool editingNotebook = false;
		public bool EditingNotebook { 
			get
			{
				return editingNotebook;
			}
			set
			{
				editingNotebook = value;
				RaisePropertyChanged();
			}
		}
		Notebook editedNotebook;
		public Notebook EditedNotebook
		{
			get
			{
				return editedNotebook;
			}
			set
			{
				editedNotebook = value;
				RaisePropertyChanged();
			}
		}

		public ICommand GetOpenEditCommand()
		{
			if(openEdit is null)
			{
				openEdit = new ActionCommand<Notebook>((nb)=> {
					EditedNotebook = nb;
					EditingNotebook = true;
				});
			}
			return openEdit;
		}
		public ICommand OpenEditCommand { get { return GetOpenEditCommand(); } }
		ICommand closeEdit;


		public List<string> DefaultCovers
		{
			get
			{
				return new List<string>()
				{
					"ms-appx:///Assets/cover1.png",
					"ms-appx:///Assets/cover2.png",
					"ms-appx:///Assets/cover3.png",
					"ms-appx:///Assets/cover4.png",
				};
			}
		}

		public ICommand setCover;
		public ICommand GetSetCoverCommand()
		{
			if(setCover is null)
			{
				setCover = new  ActionCommand<StorageFile>(async (file)=> {
					var copy = await Storage.Instance.SaveNotebookCover(EditedNotebook, file);
					EditedNotebook.CoverImage = await copy.OpenAsync(FileAccessMode.Read);
				});
				
			}
			return setCover;
		}
		public ICommand SetCoverCommand { get { return GetSetCoverCommand(); } }

		public ICommand GetCloseEditCommand()
		{
			if(closeEdit is null)
			{
				closeEdit = new Command(()=> {
					EditingNotebook = false;
					Storage.Instance.SaveNotebook(EditedNotebook);
				});
			}
			return closeEdit;
		}
		public ICommand CloseEditCommand { get { return GetCloseEditCommand(); } }
		
	}
}
