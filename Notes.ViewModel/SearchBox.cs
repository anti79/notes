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
		//MainPage page;
		public SearchBox()
		{
			results = new ObservableCollection<ISearchable>();
		}

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
			results.Clear();
			if (text.Length > 0)
			{
				
				var noteResults = Storage.Instance.GetAllNotes().Where((note) => note.Title.Contains(text));
				var notebookResults = Storage.Instance.Notebooks.Where((nb) => nb.Title.Contains(text));
				Results = new ObservableCollection<ISearchable>(results.Concat(noteResults));
				Results = new ObservableCollection<ISearchable>(results.Concat(notebookResults));
				//NoteResults = new ObservableCollection<Note>(noteResults);
				//var page = (MainPage)((MainViewModel)ParentViewModel).MainPage;
			}
			
		}
		ObservableCollection<ISearchable> results;
		public bool HasResults
		{
			get
			{

				return results.Count > 0;
			}
		}
		public ObservableCollection<ISearchable> Results { 
			get
			{
				return results;
			}
			set
			{
				results = value;
				RaisePropertyChanged();
				RaisePropertyChanged(nameof(HasResults));

			}
		}
		public ICommand openItemCommand;
		public ICommand OpenItemCommand
		{
			get { return GetOpenItemCommand(); }
		}
		public ICommand GetOpenItemCommand()
		{
			if(openItemCommand is null)
			{
				openItemCommand = new ActionCommand<ISearchable>((item) =>
				{

					var mainvm = ParentViewModel as MainViewModel;
					if (item.GetType()==typeof(Note))
					{
						mainvm.SwitchToAll.Execute(null);
						(mainvm.CurrentPage.DataContext as NotesViewModel).OpenNoteCommand.Execute(item);
					}
					else if(item.GetType()==typeof(Notebook))
					{

					}
				});
			}
			return openItemCommand;
		}

		

	}
}
