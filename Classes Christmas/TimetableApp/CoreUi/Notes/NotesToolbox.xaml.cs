using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class NotesToolbox : UserControl
    {

        public delegate void OnChangeHandler();
        public event OnChangeHandler OnChange;

        public NotesToolbox(NoteFragment fragment)
        {
            this.InitializeComponent();
            sizeSlider.Value = fragment.FontSize;
            italic.IsChecked = fragment.Italic;
            bold.IsChecked = fragment.Bold;
            center.IsChecked = fragment.Centered;

            sizeSlider.ValueChanged += (a, b) =>
                { fragment.FontSize = sizeSlider.Value; OnChange(); };
            italic.Tapped += (a, b) =>
                {
                    fragment.Italic = italic.IsChecked.Value;
                    OnChange();
                };
            bold.Tapped += (a, b) => {
                fragment.Bold = bold.IsChecked.Value;
                OnChange();
            } ;
            center.Tapped += (a, b) => {
                fragment.Centered = center.IsChecked.Value;
                OnChange();
            } ;
        }
    }
}
