using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TimetableApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ModulesEditor : Page
    {
        public ModulesEditor()
        {
            this.InitializeComponent();
        }

        //self will hold the current Course
        Course self;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            self = (e.Parameter as object[])[1] as Course;

            title.Foreground = new SolidColorBrush(self.TileColor);
            back.Foreground = new SolidColorBrush(self.TileColor);
            add.Foreground = new SolidColorBrush(self.TileColor);
            edit.Foreground = new SolidColorBrush(self.TileColor);

            title.Text = self.Name + " modules";
           
            back.Tapped += async(a, b) =>{ 
                await CoreData.SaveTimetable((e.Parameter as object[])[0] as StudentDataBundle);
                Frame.Navigate(typeof(MainPage), (e.Parameter as object[])[0]);
            };
            

            foreach (Module mod in self.Modules)
            {
                ModuleBox box = new ModuleBox(mod, self.TileColor, holder.Children, self);
                holder.Children.Add(box);
            }

            add.Tapped += (q, b) =>
            {
                Module m = new Module { Name = "New" };
                self.Modules.Add(m);
                ModuleBox box = new ModuleBox(m,self.TileColor,holder.Children , self);
                
                holder.Children.Add(box);
            };

            edit.Tapped += (a,b) =>
            {
                if ((edit.Foreground as SolidColorBrush).Color == Windows.UI.Colors.Black)
                {
                    foreach (ModuleBox box in holder.Children) box.setEditable(false);
                    edit.Foreground = new SolidColorBrush(self.TileColor);
                }
                else
                {
                    foreach (ModuleBox box in holder.Children) box.setEditable(true);
                    edit.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                }
            };
        }
    }
}
