using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class ModulePreviewComponent : UserControl
    {
        public ModulePreviewComponent(Module m , Windows.UI.Color col)
        {
            this.InitializeComponent();
            name.Foreground = new SolidColorBrush(col);
            name.Text = m.Name;
            homework.Text = m.Homeworks.Count.ToString();
            definitions.Text = m.Definitions.Count.ToString();
            notes.Text = m.Notes.Count.ToString();
            topics.Text = m.StructureBlocks.Count.ToString();
        }
    }
}
