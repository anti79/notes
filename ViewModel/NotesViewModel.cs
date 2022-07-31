using notes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		ObservableCollection<Note> notes;
		public ObservableCollection<Note> Notes
		{
			get {
				if(notes is null)
				{
					notes = new ObservableCollection<Note>(getNotesDelegate());
				}
				return notes;
			}
			
		}


		ICommand goBack;
		ICommand openNote;
		ICommand GetGoBackCommand()
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
		ICommand GetOpenNoteCommand()
		{
			if(openNote is null)
			{
				openNote = new ActionCommand<Note>((note) =>
				{
					((MainViewModel)ParentViewModel).OpenNoteCommand.Execute(note);
				});
			}
			return openNote;
		}
		ICommand markFavorite;
		ICommand GetMarkFavoriteCommand()
		{
			if(markFavorite is null)
			{
				markFavorite = new ActionCommand<Note>((note)=> {
					note.IsFavorite = true;
					Storage.Instance.SaveNote(note, note.Notebook);
				}
				);
			}
			return markFavorite;
		}

		public ICommand GoBackCommand { get { return GetGoBackCommand(); } }
		public ICommand OpenNoteCommand { get { return GetOpenNoteCommand(); } }

		public ICommand MarkFavoriteCommand { get { return GetMarkFavoriteCommand(); } }
	}
}
