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
	class Note:ObservableObject
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
				isFavorite = true;
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
		Guid guid;
		public string FileName
		{
			get
			{
				/*
				return creationDateTime.ToUniversalTime().ToString()
					.Replace("/","")
					.Replace(" ", "-")
					.Replace(":","")
					+ ".rtf";
				*/
				return guid.ToString();
				
			}
			
		}
		public string DateString
		{
			get //todo: extension method
			{
				if (creationDateTime.Date == DateTime.Today)
				{
					return "Today";
				}
				else if (creationDateTime.Date == DateTime.Today - TimeSpan.FromDays(1))
				{
					return "Yesterday";
				}
				return creationDateTime.Date.ToShortDateString();

			}
		}

		public Note()
		{
			creationDateTime = DateTime.Now;
			guid = new Guid();
			List<string> colors =  new List<string>() { "#DFA1A1", "#FDBFBF", "#FFD380", "#EDF47A", "#80E7E5" };
			color = colors[new Random().Next(colors.Count)];
		}

	}
}
