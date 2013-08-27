using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class UniversalButton : UserControl
    {
        public UniversalButton()
        {
            this.InitializeComponent();
            AnimationHelper.AddPointerAnimation(this);
        }

        public string Icon
        {
            get {return root.Content as string; }
            set { root.Content=value;}
        }

        
    }
}
