using notes.Model;
using notes.Views;
using System;
using System.Collections.Generic;
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
		public List<Notebook> Notebooks { get
			{
				return Storage.Instance.Notebooks;
			} 
		}
		
		public NotebooksViewModel()
		{
		}

		ICommand openNotebook;

		public ICommand OpenNotebookCommand { get
			{
				return GetOpenNotebookCommand();
			}
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
					page.DataContext = vm;
					vm.ParentViewModel = ParentViewModel;
					((MainViewModel)ParentViewModel).CurrentPage = page;
				});
				
			}
			return openNotebook;
		}

		
		
		
	}
}
