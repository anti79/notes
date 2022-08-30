using Microsoft.VisualStudio.PlatformUI;
using notes.Localization;
using notes.Model;
using notes.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;

namespace notes.ViewModel
{
	class EditorViewModel : ViewModel
	{

	

		public Note Note { get; set; }
        string title;
        IEditorPage page;
		ITextDocument document;
		public bool NewNote { get; set; }
		public EditorViewModel(IEditorPage page, Note note)
		{
			this.page = page;
			Note = note;
			Title = note.Title;
			NewNote = false;
			document = page.GetEditorContent();
			page.SetEditorContent(note.Content);

			string hex = note.Color.Replace("#", "");

			Color c = ColorUtils.GetColorFromHex(note.Color);
			ChosenColor = c;
			
			

		}

		
		ICommand saveExit;
		ICommand goBack;
		public ICommand SaveExitCommand
		{
			get { return GetSaveExitCommand(); }
		}
		public ICommand GoBackCommand
		{
			get { return GetGoBackCommand(); }
		}
		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
				RaisePropertyChanged();
			}
		}
		ICommand GetGoBackCommand()
		{
			if(goBack is null)
			{
				goBack = new Command(() =>
				{
                    var mainVM = ParentViewModel as MainViewModel;
                    mainVM.UpdatePages();
                    mainVM.EditorPage = null;

                });
			}
			return goBack;
		}
		ICommand GetSaveExitCommand()
		{
			if (saveExit is null)
			{
				saveExit = new Command(async () =>
				{
                    var mainVM = ParentViewModel as MainViewModel;
                    if (title.Length > 1)
					{

						
						mainVM.UpdatePages();
						mainVM.EditorPage = null;
						string str = "";
						page.GetEditorContent().GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out str);
						Note.Content = str;
						Note.Title = Title;


						Note.Notebook = mainVM.OpenedNotebook;
						
						Note.Color = ChosenColor.ToString();
						if (NewNote)
						{
							(mainVM.CurrentPage.DataContext as NotesViewModel).Notes.Add(Note);
							if (mainVM.OpenedNotebook is null)
							{
								mainVM.OpenedNotebook = Storage.Instance.Notebooks[0];
							}
							//mainVM.OpenedNotebook.Notes.Add(Note);
						}
						await Storage.Instance.SaveNoteAsync(Note, mainVM.OpenedNotebook);
						mainVM.OpenedNotebook.Notes.Add(Note);
					}
					else
					{
						MainPage.ShowMessageBox(PopupStrings.ENTER_TITLE);
					}

				});
			}
			return saveExit;
		}


		//FORMATTING
		ICommand toggleBold;
		ICommand toggleItalic;
		ICommand toggleUnderlined;
		ICommand toggleStrikethrough;
		ICommand setAlignment;
		//COLOR
		ICommand setColor;
		Windows.UI.Color color;
		public Windows.UI.Color ChosenColor
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
		

		public ICommand ToggleBoldCommand
		{
			get { 
				return GetToggleBoldCommand(); 
			}
		}
		ICommand GetToggleBoldCommand()
		{
			if (toggleBold is null)
			{
				toggleBold = new Command(() =>
				{
					document.Selection.CharacterFormat.Bold = FormatEffect.Toggle;
				});
				

			}
			return toggleBold;
		}
		public ICommand ToggleItalicCommand
		{
			get
			{
				return GetToggleItalicCommand();
			}
		}
		ICommand GetToggleItalicCommand()
		{
			if (toggleItalic is null)
			{
				toggleItalic = new Command(() =>
				{
					document.Selection.CharacterFormat.Italic = FormatEffect.Toggle;
				});


			}
			return toggleItalic;
		}

		public ICommand ToggleUnderlinedCommand
		{
			get
			{
				return GetToggleUnderlinedCommand();
			}
		}
		ICommand GetToggleUnderlinedCommand()
		{
			if (toggleUnderlined is null)
			{
				toggleUnderlined = new Command(() =>
				{
					if (document.Selection.CharacterFormat.Underline == UnderlineType.None)
					{
						document.Selection.CharacterFormat.Underline = UnderlineType.Single;
					}
					else
					{
						document.Selection.CharacterFormat.Underline = UnderlineType.None;
					}

				});


			}
			return toggleUnderlined;
		}

		public ICommand ToggleStrikethroughCommand
		{
			get
			{
				return GetToggleStrikethroughCommand();
			}
		}
		ICommand GetToggleStrikethroughCommand()
		{
			if (toggleStrikethrough is null)
			{
				toggleStrikethrough = new Command(() =>
				{
					document.Selection.CharacterFormat.Strikethrough = FormatEffect.Toggle;
				});


			}
			return toggleStrikethrough;
		}

		public ICommand SetAlignmentCommand
		{
			get
			{
				return GetSetAlignmentCommandCommand();
			}
		}
		ICommand GetSetAlignmentCommandCommand()
		{
			if (setAlignment is null)
			{
				setAlignment = new ActionCommand<string>((type) =>
				{

					document.Selection.ParagraphFormat.Alignment = (ParagraphAlignment)Enum.Parse(typeof(ParagraphAlignment), type);
					//document.Selection.ParagraphFormat.ListStyle = MarkerStyle.
				});


			}
			return setAlignment;
		}
		public ICommand SetColorCommand
		{
			get
			{
				return GetSetColorCommand();
			}
		}
		public ICommand GetSetColorCommand()
		{
			if(setColor is null) {
				setColor = new Command(()=> {
					
				});
			}
			return setColor;
		}
	}
}
