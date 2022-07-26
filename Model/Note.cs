using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Text;

namespace notes.Model
{
	class Note
	{
		public string Title { get; set; }
		public bool IsFavorite { get; set; }

		public string Content { get; set; }
		DateTime creationDateTime;

		public DateTime CreationDateTime
		{
			get
			{
				return creationDateTime;
			}
		}
		public string FileName
		{
			get
			{
				return Title + " " + creationDateTime.ToUniversalTime().ToString()
					.Replace("/","")
					.Replace(" ", "-")
					.Replace(":","")
					+ ".rtf";
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
		}

	}
}
