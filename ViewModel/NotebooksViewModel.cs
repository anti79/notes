using notes.Model;
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
				openNotebook = new ActionCommand<Notebook>((e) => {
					Console.WriteLine(e);
					//((MainViewModel)ParentViewModel).
				});
				
			}
			return openNotebook;
		}

		
		
		
	}
}
