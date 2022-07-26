using notes.Model;
using notes.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace notes.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class EditorPage : Page, IEditorPage
	{
		public EditorPage()
		{
			this.InitializeComponent();
			

		}

		public ITextDocument GetEditorContent()
		{
			return textEditorBox.Document;
			
		}

		public void SetEditorContent(string str)
		{
			textEditorBox.Document.SetText(TextSetOptions.FormatRtf, str);
			//textEditorBox.Document.LoadFromStream(TextSetOptions.FormatRtf, stream);
		}
	}
}
