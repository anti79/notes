using Windows.Storage.Streams;
using Windows.UI.Text;

namespace notes.Views
{
	internal interface IEditorPage
	{
		void SetEditorContent(string stream);
		ITextDocument GetEditorContent();
	}
}