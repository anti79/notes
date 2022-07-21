using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes.Model
{
	class Notebook:IEnumerable
	{
		public string Name { get; set; }
		public List<Note> Notes { get; set; }
		public Notebook()
		{
			Notes = new List<Note>();
		}
		public IEnumerator GetEnumerator()
		{
			return Notes.GetEnumerator();
		}
	}
}
