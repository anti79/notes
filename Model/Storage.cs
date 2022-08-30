using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Text;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace notes.Model
{
	class Storage
	{
		const string NOTEBOOK_NAME_FILE = "name.txt";
		const string NOTEBOOK_COVER_FILE = "cover";
		const string DEFAULT_COVER_URI = "ms-appx:///Assets/cover0.png";
		const string DEFAULT_NOTE_COLOR = "#DFA1A1";

		List<StorageFolder> subfolders;

        private static Storage instance = null;
		private static readonly object _lock = new object();
		StorageFolder folder;
		
		public async Task SaveNotebookAsync(Notebook notebook)
		{
			var nbFolder = await folder.CreateFolderAsync(notebook.FolderName, CreationCollisionOption.OpenIfExists);
			var nameFile = await nbFolder.CreateFileAsync(NOTEBOOK_NAME_FILE, CreationCollisionOption.OpenIfExists);
			
			await FileIO.WriteTextAsync(nameFile, notebook.Title);

		}
		public async Task<StorageFile> SaveNotebookCoverAsync(Notebook notebook, StorageFile cover)
		{
			var nbFolder = await folder.GetFolderAsync(notebook.FolderName);
			return await cover.CopyAsync(nbFolder, NOTEBOOK_COVER_FILE, NameCollisionOption.ReplaceExisting);


		}
		public async Task<IRandomAccessStream> GetDefaultCoverAsync()
		{
			return await (await StorageFile.GetFileFromApplicationUriAsync(new Uri(DEFAULT_COVER_URI))).OpenAsync(FileAccessMode.Read);
		}

		public async Task SaveNoteAsync(Note note, Notebook notebook)
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
				note.Color = DEFAULT_NOTE_COLOR;
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
				
				var list = new List<Note>();
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
			var list = new List<Note>();
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
		public async Task DeleteNoteAsync(Note note)
		{
			var subfolder = await folder.GetFolderAsync(note.Notebook.FolderName);
            var file = await subfolder.GetFileAsync(note.FileName);
			await file.DeleteAsync();
		}
        public async Task DeleteNotebookAsync(Notebook nb)
        {
			var subfolder = await folder.GetFolderAsync(nb.FolderName);
			Notebooks.Remove(nb);
			await subfolder.DeleteAsync();
			
        }
        public async Task LoadAsync()
		{
			Notebooks.Clear();
			
			if(subfolders is null) await LoadSubfoldersAsync();
			await LoadNotebooksAsync();
			await LoadNotesAsync();
		}
		
		async Task LoadSubfoldersAsync()
		{
            subfolders = new List<StorageFolder>(await folder.GetFoldersAsync());
        }
		async Task LoadNotebooksAsync()
		{
			/*
			if(subfolders.Count<1)
			{
				var defaultNb = new Notebook();
				defaultNb.Title = DefaultValuesStrings.STARTING_NOTEBOOK_NAME;
				defaultNb.IsDeletable = false;
				defaultNb.Notes = new List<Note>();
				await SaveNotebookAsync(defaultNb);
			}
			*/
			foreach (var sf in subfolders)
			{
				var nb = new Notebook();
				var files = await sf.GetFilesAsync();
				StorageFile nameFile;
				try
				{
					nameFile = await sf.GetFileAsync(NOTEBOOK_NAME_FILE);
				}
				catch (FileNotFoundException)
				{
					continue;
				}
				StorageFile coverFile;
				try
				{
					coverFile = await sf.GetFileAsync(NOTEBOOK_COVER_FILE);
					nb.CoverImage = await coverFile.OpenAsync(FileAccessMode.Read);
				}
				catch (FileNotFoundException)
				{
					coverFile = null;
					nb.CoverImage = await GetDefaultCoverAsync();


				}

				nb.Title = await FileIO.ReadTextAsync(nameFile);
				nb.Notes = new List<Note>();
				nb.Guid = new Guid(sf.DisplayName);
				Notebooks.Add(nb);

			}
		}
		async Task LoadNotesAsync()
		{
			
			foreach (var sf in subfolders)
			{
				var files = await sf.GetFilesAsync();
                for(int i=0;i<files.Count;i++)
                {
					//TODO: catch
					var file = files[i];
                    if (file.Name == NOTEBOOK_NAME_FILE || file.Name == NOTEBOOK_COVER_FILE)
                    {
                        continue;
                    }
                    var n = new Note();
                    n.Guid = new Guid(file.DisplayName);
                    n.Content = await FileIO.ReadTextAsync(file);
                    n.Notebook = Notebooks[i];

                    n = ParseRTFMetadata(n);
					Notebooks[i].Notes.Add(n);
                }
                ;

            }
		}
	}
}
