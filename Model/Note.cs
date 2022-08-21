using notes.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Text;

namespace notes.Model
{
	class Note:ObservableObject,ISearchable
	{
		string title;
		bool isFavorite;
		string content;
		string color;

		public Notebook Notebook { get; set; }
		public string Color
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
				RaisePropertyChanged();
			}
		}

		public string Title { get
			{
				return title;
			}
			set
			{
				title = value;
				RaisePropertyChanged();
			}
		}
		public bool IsFavorite { 
			get
			{
				return isFavorite;
			}
			set
			{
				isFavorite = value;
				RaisePropertyChanged();
			}
		}

		public string Content
		{
			get
			{
				return content;
			}
			set
			{
				content = value;
				RaisePropertyChanged();
			}
		}

		
		DateTime creationDateTime;


		public DateTime CreationDateTime
		{
			get
			{
				return creationDateTime;
			}
			set
			{
				creationDateTime = value;
				RaisePropertyChanged();
			}
		}
		public Guid Guid { get; set; }

		const string extension = ".rtf";
		public string FileName
		{
			get
			{
			
				return Guid.ToString() + extension;
				
			}
			
		}



		public Note()
		{
			creationDateTime = DateTime.Now;
			Guid = Guid.NewGuid();
			var colors =  new List<string>() { "#DFA1A1", "#FDBFBF", "#FFD380", "#EDF47A", "#80E7E5" };
			color = colors[new Random().Next(colors.Count)];
		}
		public override string ToString()
		{
			return Title;
		}
	}
}
