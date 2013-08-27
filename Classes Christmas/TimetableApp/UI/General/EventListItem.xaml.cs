using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class EventListItem : UserControl
    {
        public Course self;
        public EventListItem(ref Course model)
        {
            this.InitializeComponent();
            self = model;
            _class.Text = model.Class;
            _teacher.Text = model.Teacher;
            _name.Text = model.Name;
            box.Background = new SolidColorBrush(model.TileColor);
        }

        public void Update()
        {
            _class.Text = self.Class;
            _teacher.Text = self.Teacher;
            _name.Text = self.Name;
            box.Background = new SolidColorBrush(self.TileColor);
        }

        public bool isSelected = false;
        public void SetState(bool visible)
        {
            selectedBox.Visibility = (visible) ? Visibility.Visible : Visibility.Collapsed;
            if (visible)
            {
                selectedAnim.Begin();
                isSelected = true;
            }
            else
            {
                unselectedAnim.Begin();
                isSelected = false;
            }
        }
    }
}
