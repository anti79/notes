using notes.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace notes.ViewModel
{
	class NotesViewModel : ViewModel
	{
		public delegate List<Note> GetNotes();
		GetNotes getNotesDelegate;

		public string Title {get;set;}
		public bool IsNotebook { get; set; }
		public NotesViewModel()
		{
			GetNotesDelegate = () =>
			{
				return Storage.Instance.GetAllNotes();
			};
			IsNotebook = false;
		}
		public GetNotes GetNotesDelegate
		{
			get
			{
				return getNotesDelegate;
			}
			set
			{
				getNotesDelegate = value;
			}
		}

		public List<Note> Notes
		{
			get {
				return getNotesDelegate();
			}
		}
		
		public ViewModel MainFrameContext
		{
			get
			{
				return this;
			}
		}

		ICommand goBack;
		public ICommand GetGoBackCommand()
		{
			if (goBack is null)
			{
				goBack = new Command(() =>
				{
					((MainViewModel)ParentViewModel).SwitchToNotebooks.Execute(null);
				});
			}
			return goBack;
		}

		public ICommand GoBackCommand { get { return GetGoBackCommand(); } }

	}
}
