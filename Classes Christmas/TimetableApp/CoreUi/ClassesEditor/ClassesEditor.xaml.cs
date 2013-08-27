using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TimetableApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClassesEditor : Page
    {
        public ClassesEditor()
        {
            this.InitializeComponent();
        }

        StudentDataBundle self;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            self = e.Parameter as StudentDataBundle;//keep a reference to the timetable
            refresh();

            //go back to previous page
            back.Tapped += async (a, b) =>
            {
                await CoreData.SaveTimetable(self);
                Frame.Navigate(typeof(MainPage), self);
            };

            //create new course
            add.Tapped += (a, b) =>
            {
                self.Courses.Add(new Course { Name ="New Class" });
                refresh();
            };
        }

        //redraw all courses
        void refresh()
        {
            holder.Children.Clear();
            foreach (Course model in self.Courses)
            {
                ClassBox box = new ClassBox(model , holder.Children , self);
                box.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                holder.Children.Add(box);
            }
        }
    }
}
