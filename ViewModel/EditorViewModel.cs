using notes.Model;
using notes.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Streams;

namespace notes.ViewModel
{
	class EditorViewModel:ViewModel
	{

	
		public Note Note { get; set; }
		IEditorPage page;
		public bool NewNote { get; set; }
		public EditorViewModel(IEditorPage page, Note note)
		{
			this.page = page;
			Note = note;
			Title = note.Title;
			NewNote = false;
			page.SetEditorContent(note.Content);
			
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
					mainVM.UpdatePages();
					mainVM.EditorPage = null;
					string str = "";
					page.GetEditorContent().GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out str);
					Note.Content = str;
					Note.Title = Title;

					if (NewNote)
					{
						((NotesViewModel)mainVM.CurrentPage.DataContext).Notes.Add(Note);
						mainVM.OpenedNotebook.Notes.Add(Note);
					}
					Storage.Instance.SaveNote(Note, mainVM.OpenedNotebook);


				});
			}
			return exitCommand;
		}
	}
}
