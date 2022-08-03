using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Text;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace notes.Model
{
	class Storage
	{
	
		private static Storage instance = null;
		private static readonly object _lock = new object();
		StorageFolder folder;
		
		public async void SaveNotebook(Notebook notebook)
		{
			var nbFolder = await folder.CreateFolderAsync(notebook.FolderName, CreationCollisionOption.OpenIfExists);
			var nameFile = await nbFolder.CreateFileAsync("name.txt", CreationCollisionOption.OpenIfExists);
			FileIO.WriteTextAsync(nameFile, notebook.Name);
			
		}

		public async void SaveNote(Note note, Notebook notebook)
		{
			StorageFolder storageFolder;
			try
			{
				storageFolder = await folder.GetFolderAsync(notebook.FolderName);
			}
			catch
			{
				storageFolder = await folder.CreateFolderAsync(notebook.FolderName);
			}

			var file = await storageFolder.CreateFileAsync(note.FileName,
			Windows.Storage.CreationCollisionOption.OpenIfExists);

			await Windows.Storage.FileIO.WriteTextAsync(file, AddRTFMetadata(note).Content + Environment.NewLine);
			



		}

		Note AddRTFMetadata(Note note)
		{
			note.Content = $"{note.Content} \n {{{note.Title}|{note.Color}|{note.IsFavorite}}} ";
			return note;
		}
		Note ParseRTFMetadata(Note note)
		{
			string data = note.Content.Split('{').Last()
				.Replace("}", "")
				.Replace("\r","")
				.Replace("\n","")
				.Replace(" ","");
			note.Title = data.Split('|')[0];
			try
			{
				note.Color = data.Split('|')[1];
			}
			catch (IndexOutOfRangeException)
			{
				note.Color = "#DFA1A1";
			}
			try
			{
				note.IsFavorite = bool.Parse(data.Split('|')[2]);
			}
			catch (IndexOutOfRangeException)
			{
				note.IsFavorite = false;
			}
			return note;
		}
		


		public async Task<IRandomAccessStream> GetStream(Note note)
		{
			return await (
				(await folder.GetFileAsync(note.FileName)).OpenAsync(FileAccessMode.ReadWrite));
		}
		public List<Notebook> Notebooks { get; set; }
		public static Storage Instance
		{
			get
			{
				lock (_lock)
				{
					if (instance == null)
					{
						instance = new Storage();
						
					}
					return instance;
				}
			}
		}

		public List <Note> GetAllNotes ()
		{
				List<Note> list = new List<Note>();
				foreach(Notebook nb in Notebooks)
				{
					foreach(Note n in nb)
					{
						list.Add(n);

					}
				}
				return list;

		} 
		
		public List<Note> GetFavoriteNotes()
		{
			List<Note> list = new List<Note>();
			foreach (Notebook nb in Notebooks)
			{
				foreach (Note n in nb)
				{
					if(n.IsFavorite) list.Add(n);
		
				}
			}
			return list;
		}
		private Storage()
		{
			Notebooks = new List<Notebook>();
			folder = ApplicationData.Current.LocalFolder;
		}
		public async Task Load()
		{
			var subfolders = await folder.GetFoldersAsync();
			foreach (var sf in subfolders)
			{
				var files = await sf.GetFilesAsync();
				StorageFile nameFile;
				try
				{
					 nameFile= await sf.GetFileAsync("name.txt");
				}
				catch (FileNotFoundException)
				{
					continue;
				}
				StorageFile coverFile;
				try
				{
					coverFile = await sf.GetFileAsync("cover");
				}
				catch (FileNotFoundException)
				{
					coverFile = null;
				}
				var nb = new Notebook()
				{
					Name = await FileIO.ReadTextAsync(nameFile),
					Notes = new List<Note>(),
					Guid = new Guid(sf.DisplayName)
				};
				if(!(coverFile is null))
				{
					nb.CoverImagePath = (coverFile.Path);
				}

				foreach (var file in files)
				{
					if(file.Name=="name.txt" || file.Name=="cover")
					{
						continue;
					}
					Note n = new Note();
					n.Guid = new Guid(file.DisplayName);
					n.Content = await FileIO.ReadTextAsync(file);
					n.Notebook = nb;
					
					n = ParseRTFMetadata(n);
					nb.Notes.Add(n);
				}
				Notebooks.Add(nb);

			}
		}
	}
}
