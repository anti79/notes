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
	class Notebook : ObservableObject, IEnumerable
	{
		string name;
		public Guid Guid { get; set; }
		IRandomAccessStream coverImage;
		public IRandomAccessStream CoverImage { get
			{
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
		public string Name { get
			{
				return name;
			}
			set
			{
				name = value;
				RaisePropertyChanged();
			}
		}
		public List<Note> notes;
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
			//CoverImagePath = new Uri("ms-appx:///Assets/cover1.png");
			
		}


		public IEnumerator GetEnumerator()
		{
			return Notes.GetEnumerator();
		}
	}
}
