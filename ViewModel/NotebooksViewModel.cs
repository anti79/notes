using notes.Localization;
using notes.Model;
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
				createNotebook = new AsyncCommand(async () => {
					var nb = new Notebook() { Title = DefaultValuesStrings.DEFAULT_NOTEBOOK_NAME };
					nb.CoverImage = await Storage.Instance.GetDefaultCoverAsync();
					Notebooks.Add(nb);
					await Storage.Instance.SaveNotebookAsync(nb);
					

				});
			}
			return createNotebook;
		}

		ICommand GetOpenNotebookCommand()
		{
			if (openNotebook is null)
			{
				openNotebook = new AsyncActionCommand<Notebook>(async (nb) => {
					var page = new NotesPage();
					var vm = new NotesViewModel();
					vm.GetNotesDelegate = async () =>
					{
						return nb.Notes;
					};
					await vm.LoadNotesAsync();
					vm.Title = nb.Title;
					vm.IsNotebook = true;
					(vm.CreateNoteCommand as Command).RaiseCanExecuteChanged();
					page.DataContext = vm;
					vm.ParentViewModel = ParentViewModel;
					var mainVM = ParentViewModel as MainViewModel;
					mainVM.CurrentPage = page;
					mainVM.OpenedNotebook = nb;
					
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

		const string baseCoverUri = "ms-appx:///Assets/cover";
		const string baseCoverExtension = ".png";
		const int defaultCovers = 4;
        public List<string> DefaultCovers
		{
			get
			{
				var list = new List<string>();
				for(int i=1;i<defaultCovers;i++)
				{
					list.Add(baseCoverUri + i.ToString() + baseCoverExtension);
				}
				return list;
				
			}
		}

		public ICommand setCover;
		public ICommand deleteNotebook;
		public ICommand DeleteNotebookCommand { get { return GetDeleteNotebookCommand(); } }
		ICommand GetDeleteNotebookCommand()
		{
			if(deleteNotebook is null)
			{
				deleteNotebook = new AsyncActionCommand<Notebook>(async (nb) =>
				{
					nb.Notes.Clear();
					Notebooks.Remove(nb);
					
					await Storage.Instance.DeleteNotebookAsync(nb);

				});
			}
			return deleteNotebook;
		}
		public ICommand GetSetCoverCommand()
		{
			if(setCover is null)
			{
				setCover = new AsyncActionCommand<StorageFile>(async (file)=> {

                        var copy = await Storage.Instance.SaveNotebookCoverAsync(EditedNotebook, file);
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
				closeEdit = new AsyncCommand(async ()=> {
					if (EditedNotebook.Title.Length > 1)
					{
						EditingNotebook = false;
						await Storage.Instance.SaveNotebookAsync(EditedNotebook);
					}
					else
					{
						MainPage.ShowMessageBox(PopupStrings.ENTER_TITLE);
					}
				});
			}
			return closeEdit;
		}
		public ICommand CloseEditCommand { get { return GetCloseEditCommand(); } }
		
	}
}
