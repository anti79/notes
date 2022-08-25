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
		public void LoadNotes()
		{
            notes = new ObservableCollection<Note>(getNotesDelegate());
			RaisePropertyChanged("Notes");
        }
		ObservableCollection<Note> notes;
		public ObservableCollection<Note> Notes
		{
			get {
				if(notes is null)
				{
					LoadNotes();
                }
				return notes;
			}
			
		}




		ICommand goBack;
		ICommand openNote;
		ICommand deleteNote;

		ICommand GetDeleteNoteCommand()
		{
            if (deleteNote is null)
            {
                deleteNote =  new ActionCommand<Note>((note) =>
                {
					Storage.Instance.DeleteNoteAsync(note);
					notes.Remove(note);
					note.Notebook.Notes.Remove(note);
                });
            }
            return deleteNote;
        }
		public ICommand DeleteNoteCommand
		{
			get{ return GetDeleteNoteCommand(); }
		}
		ICommand GetGoBackCommand()
		{
			if (goBack is null)
			{
				goBack = new Command(() =>
				{
					(ParentViewModel as MainViewModel).SwitchToNotebooks.Execute(null);
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
					(ParentViewModel as MainViewModel).OpenNoteCommand.Execute(note);
				});
			}
			return openNote;
		}
		ICommand toggleFavorite;
		ICommand GetToggleFavoriteCommand()
		{
			if(toggleFavorite is null)
			{
				toggleFavorite = new ActionCommand<Note>((note)=> {
					note.IsFavorite = !note.IsFavorite;
                    notes = new ObservableCollection<Note>(getNotesDelegate());
                    RaisePropertyChanged(nameof(Notes));
					Storage.Instance.SaveNoteAsync(note, note.Notebook);
				}
				);
			}
			return toggleFavorite;
		}

		public ICommand GoBackCommand { get { return GetGoBackCommand(); } }
		public ICommand OpenNoteCommand { get { return GetOpenNoteCommand(); } }

		ICommand createNoteCommand;
		public ICommand ToggleFavoriteCommand { get { return GetToggleFavoriteCommand(); } }
		public ICommand CreateNoteCommand { get { return GetCreateNoteCommand(); } }
		ICommand GetCreateNoteCommand()
		{
			if (createNoteCommand is null)
			{
				createNoteCommand = new Command(() =>
				{
						var editorPage = new EditorPage();
						var editorVM = new EditorViewModel(editorPage as IEditorPage, new Note());
						editorVM.ParentViewModel = ParentViewModel;
						editorVM.NewNote = true;
						editorPage.DataContext = editorVM;
						(ParentViewModel as MainViewModel).EditorPage = editorPage;
						//Storage.Instance.CreateFile()
					
				});
			}
			return createNoteCommand;
		}
	}
}
