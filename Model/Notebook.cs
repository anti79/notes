using notes.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes.Model
{
	class Notebook : ObservableObject, IEnumerable
	{
		string name;
		public Guid Guid { get; set; }
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
			
		}


		public IEnumerator GetEnumerator()
		{
			return Notes.GetEnumerator();
		}
	}
}
