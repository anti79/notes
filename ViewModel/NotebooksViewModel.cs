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
using Windows.UI.Input;
using Windows.UI.Xaml.Input;

namespace notes.ViewModel
{
	class NotebooksViewModel:ViewModel
	{
		ObservableCollection<Notebook> notebooks;
		public ObservableCollection<Notebook> Notebooks { get
			{
				if(notebooks is null)
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

		private ICommand GetCreateNotebookCommand()
		{
			if (createNotebook is null)
			{
				createNotebook = new Command(()=> {
					Notebooks.Add(new Notebook() { Name = "Untitled" });

				});
			}
			return createNotebook;
		}

		public ICommand GetOpenNotebookCommand()
		{
			if(openNotebook is null)
			{
				openNotebook = new ActionCommand<Notebook>((nb) => {
					Console.WriteLine(nb);
					var page = new NotesPage();
					var vm = new NotesViewModel();
					vm.GetNotesDelegate = () =>
					{
						return nb.Notes;
					};
					vm.Title = nb.Name;
					vm.IsNotebook = true;
					page.DataContext = vm;
					vm.ParentViewModel = ParentViewModel;
					((MainViewModel)ParentViewModel).CurrentPage = page;
					((MainViewModel)ParentViewModel).OpenedNotebook = nb;
				});
				
			}
			return openNotebook;
		}

		
		
		
	}
}
