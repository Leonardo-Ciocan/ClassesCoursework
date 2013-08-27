using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class ModulePreview : UserControl
    {
        public delegate void OnOpenHandler(Course model);
        public event OnOpenHandler OnOpen;

        public ModulePreview(Course model)
        {
            this.InitializeComponent();

            //initial values
            name.Text = model.Name;
            header.Background = new SolidColorBrush(model.TileColor);
            edit.Tapped += (a, b) => OnOpen(model);


            holder.SelectionChanged += (a, b) =>
            {
                count.Text = (holder.SelectedIndex + 1).ToString() + "/" + holder.Items.Count.ToString();
            };

            //add each module to the flipview as a modulepreviewcomponent
            //so the user can flick through them
            foreach (Module m in model.Modules)
            {
                ModulePreviewComponent mpc = new ModulePreviewComponent(m , model.TileColor);
                FlipViewItem item = new FlipViewItem { Content = mpc };
                holder.Items.Add(item);
            }
            count.Text = (holder.SelectedIndex + 1).ToString() + "/" + holder.Items.Count.ToString();
        }
    }
}
