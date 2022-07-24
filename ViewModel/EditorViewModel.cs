using notes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace notes.ViewModel
{
	class EditorViewModel:ViewModel
	{
		public string Text {

			get
			{
				return Note.Content;
			}

			set
			{
				if (Note != null) {
					Note.Content = value;
					RaisePropertyChanged();
				}
			}
		}
		public Note Note { get; set; }
		public EditorViewModel()
		{
			Text = "aaaaaaa";
		}
		ICommand exitCommand;
		public ICommand ExitCommand
		{
			get { return GetExitCommand(); }
		}
		ICommand GetExitCommand()
		{
			if(exitCommand is null)
			{
				exitCommand = new Command(()=>
				{
					var mainVM = ((MainViewModel)ParentViewModel);
					mainVM.EditorPage = null;
					((NotesViewModel)mainVM.CurrentPage.DataContext).Notes.Add(Note);



				});
			}
			return exitCommand;
		}
	}
}
