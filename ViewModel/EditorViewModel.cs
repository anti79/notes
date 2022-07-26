using notes.Model;
using notes.Views;
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
		public Note Note { get; set; }
		IEditorPage page;
		public EditorViewModel(IEditorPage page)
		{
			this.page = page;
		}
		ICommand exitCommand;
		public ICommand ExitCommand
		{
			get { return GetExitCommand(); }
		}
		string title;
		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
				RaisePropertyChanged();
			}
		}
		ICommand GetExitCommand()
		{
			if(exitCommand is null)
			{
				exitCommand = new Command(()=>
				{
					var mainVM = ((MainViewModel)ParentViewModel);
					mainVM.EditorPage = null;
					string str = "";
					page.GetEditorContent().GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out str);
					Note.Content = str;
					Note.Title = Title;
					((NotesViewModel)mainVM.CurrentPage.DataContext).Notes.Add(Note);
					mainVM.OpenedNotebook.Notes.Add(Note);
					Storage.Instance.SaveNote(Note, mainVM.OpenedNotebook);


				});
			}
			return exitCommand;
		}
	}
}
