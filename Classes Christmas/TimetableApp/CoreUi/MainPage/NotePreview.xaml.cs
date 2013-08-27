using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class NotePreview : UserControl
    {
        public delegate void OnOpenHandler(Course model);
        public event OnOpenHandler OnOpen;

        public NotePreview(Course model)
        {
            this.InitializeComponent();

            this.Loaded += (a, b) => { if (model.Modules.Count == 0) (Parent as StackPanel).Children.Remove(this); };
            header.Background = new SolidColorBrush(model.TileColor);
            edit.Tapped += (a, b) => OnOpen(model);
            int c = model.Modules.SelectMany(mod => mod.Notes).Count();
            count.Text = c.ToString() + " notes";
            name.Text = model.Name;
        }
    }
}
