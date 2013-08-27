using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class EventControl : UserControl
    {
        public Course self;
        public EventControl(Course model)
        {
            this.InitializeComponent();
            //setScheme(scheme);
            SubjectName.Text = model.Name;
            TeacherName.Text = model.Teacher;
            ClassName.Text = model.Class;
           // root.Stroke = new SolidColorBrush(Colors.Purple);
            self = model;
            root.Fill = new SolidColorBrush(model.TileColor);
        }

        public EventControl() {
            this.InitializeComponent();
        }

        public void FillWith(Course model)
        {
            self = model;
            SubjectName.Text = model.Name;
            TeacherName.Text = model.Teacher;
            ClassName.Text = model.Class;
            root.Fill = new SolidColorBrush(model.TileColor);
        }

        public void setTime(Time start , Time end)
        {
            _time.Text = start.ToString() + " to " + end.ToString();
        }

        bool selected = false;
        public bool isSelected
        {
            get { return selected; }
            set { selected = value;
            if (selected)
            {

                root.StrokeThickness = 3.5;
            }
            else
            {
                root.StrokeThickness = 0;
            }
            }
        }
    }
}
