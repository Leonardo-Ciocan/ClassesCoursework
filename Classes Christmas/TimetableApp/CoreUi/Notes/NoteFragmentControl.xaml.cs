using Callisto.Controls;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class NoteFragmentControl : EditableItem
    {
        public NoteFragment self;
        public NoteFragmentControl(NoteFragment fragment, UIElementCollection container , Note note , Color col , bool shouldShade)
        {
            this.InitializeComponent();
            self = fragment;
            content.Text = fragment.Content;
            editBox.Text = content.Text;
            //back.Background = new SolidColorBrush(fragment.BackColor);
            SetEditableItems(editBox,deleteButton);
            SetStaticItems(content);

            back.Background = new SolidColorBrush(col);
            shade.Visibility = (shouldShade) ? Visibility.Visible : Visibility.Collapsed;

            deleteButton.Tapped += (e, ev) =>
            {
                container.Remove(this);
                note.Fragments.Remove(self);
            };
            editBox.TextChanged += (e, ev) =>
            {
                content.Text = editBox.Text;
                self.Content = editBox.Text;
            };
            
            editBox.SizeChanged += (_e, _ev) =>
            {
                this.Height = editBox.ActualHeight + 10;
            };
            //selector.SizeChanged+= (_e,_ev) => selector.Width = selector.ActualHeight;
            this.Loaded+=(_e,_ev)=>this.Width = (this.Parent as FrameworkElement).ActualWidth;
            //selector.Fill = new SolidColorBrush(col);
            Update();

            edit.Tapped += (a, b) =>
            {
                NotesToolbox ns = new NotesToolbox(self);
                Flyout fl = new Flyout { Placement = PlacementMode.Bottom , PlacementTarget = edit };
                fl.Content = ns;
                fl.IsOpen = true;
                ns.OnChange += () => Update();
            };
        }

        public void Update()
        {
            content.FontSize = self.FontSize;
            content.FontWeight = (self.Bold) ? FontWeights.Bold : FontWeights.Normal;
            content.FontStyle = (self.Italic) ? FontStyle.Italic : FontStyle.Normal;
            content.TextAlignment = (self.Centered) ? TextAlignment.Center : TextAlignment.Left;
            content.Foreground = new SolidColorBrush(self.FrontColor);

            editBox.FontSize = self.FontSize;
            editBox.FontWeight = (self.Bold) ? FontWeights.Bold : FontWeights.Normal;
            editBox.FontStyle = (self.Italic) ? FontStyle.Italic : FontStyle.Normal;
            editBox.TextAlignment = (self.Centered) ? TextAlignment.Center : TextAlignment.Left;
            editBox.Foreground = new SolidColorBrush(self.FrontColor);

            //back.Background = new SolidColorBrush(self.BackColor);
        }

        public bool SelectorVisible
        {
            get
            {
                return (selector.Visibility == Windows.UI.Xaml.Visibility.Visible);
            }
            set
            {
                selector.Visibility = (value) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public TextBlock getVisualRoot()
        {
            return content;
        }
    }
}
