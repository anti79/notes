using notes.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace notes.Model
{
	public class Notebook : ObservableObject, IEnumerable, ISearchable
	{
		string name;
		public Guid Guid { get; set; }
		IRandomAccessStream coverImage;
		public IRandomAccessStream CoverImage { get
			{

				coverImage.Seek(0);
				return coverImage;
			}
			set
			{
				coverImage = value;
				RaisePropertyChanged();
			}
		}

		public string FolderName {
			get
			{
				return Guid.ToString();
			}
		}
		public string Title { get
			{
				return name;
			}
			set
			{
				name = value;
				RaisePropertyChanged();
			}
		}
		private List<Note> notes;
		public bool IsDeletable { get; set; }
		public List<Note> Notes { get
			{

				return notes;
			}
			set
			{
				notes = value;
				RaisePropertyChanged();
			}
		}
		public Notebook()
		{
			Notes = new List<Note>();
			Guid = Guid.NewGuid();
			IsDeletable = true;

		}

		public IEnumerator GetEnumerator()
		{
			return Notes.GetEnumerator();
		}
		public string Type { 
			get { return this.GetType().Name; }
		}
		public override string ToString()
		{
			return Type;
		}
	}
}
