using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class ExtendedTextBox : UserControl
    {
        public ExtendedTextBox()
        {
            this.InitializeComponent();
        }

        public string Text
        {
            get { return text_box.Text.Replace(":", ""); }
            set { text_box.Text = value + ":"; }
        }
    }
}
