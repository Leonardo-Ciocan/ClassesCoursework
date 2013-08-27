using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class EventPreview : UserControl
    {
        public EventPreview(Course model)
        {
            this.InitializeComponent();
            _class.Text = model.Class;
            _teacher.Text = model.Teacher;
            _name.Text = model.Name;
            _notes.Text = model.Notes;
            rect.Fill = new SolidColorBrush(model.TileColor);
            if (model.Notes == "") _notes.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
    }
}
