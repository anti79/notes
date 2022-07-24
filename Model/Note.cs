using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes.Model
{
	class Note
	{
		public string Title { get; set; }
		public bool IsFavorite { get; set; }

		public string Content { get; set; }
		DateTime creationTime;

		public string DateString
		{
			get //todo: extension method
			{
				if (creationTime.Date == DateTime.Today)
				{
					return "Today";
				}
				else if (creationTime.Date == DateTime.Today - TimeSpan.FromDays(1))
				{
					return "Yesterday";
				}
				return creationTime.Date.ToShortDateString();

			}
		}

		public Note()
		{
			creationTime = DateTime.Today;
			Content = "";
		}

	}
}
