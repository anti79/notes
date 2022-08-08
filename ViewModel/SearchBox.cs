using notes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace notes.ViewModel
{
	class SearchBox:ViewModel
	{
		ICommand toggleSearch;
		bool searching;
		public bool Searching
		{
			get
			{
				return searching;
			}
			set
			{
				searching = value;
				RaisePropertyChanged();
			}
		}
		string searchBoxText;
		public string SearchBoxText
		{
			get
			{
				return searchBoxText;
			}
			set
			{
				searchBoxText = value;
				Search(value);
			}
		}
		public ICommand ToggleSearchCommand { get { return GetToggleSearchCommand(); } }
		ICommand GetToggleSearchCommand()
		{
			if (toggleSearch is null)
			{
				toggleSearch = new Command(() => {
					Searching = !searching;
				});
			}
			return toggleSearch;
		}
		public void Search(string text)
		{
			//var mvm = (MainViewModel)ParentViewModel;
			if (text.Length > 0)
			{
				var noteResults = Storage.Instance.GetAllNotes().Where((note) => note.Title.Contains(text));

				if (noteResults.Count() > 0)
				{
					hasResults = true;
				}
				else
				{
					hasResults = false;
				}
				NoteResults = new ObservableCollection<Note>(noteResults);
			}
		}
		ObservableCollection<Note> noteResults;
		ObservableCollection<Note> notebookResults;
		bool hasResults;
		public bool HasResults
		{
			get
			{
				return hasResults;
			}
			set
			{
				hasResults = value;
				RaisePropertyChanged();
			}
		}
		public ObservableCollection<Note> NoteResults { 
			get
			{
				return noteResults;
			}
			set
			{
				noteResults = value;
				RaisePropertyChanged();
			}
		}
		public ObservableCollection<Note> NotebookResults { 
			get
			{
				return notebookResults;
			}
			set
			{
				notebookResults = value;
				RaisePropertyChanged();

			}
		}

	}
}
