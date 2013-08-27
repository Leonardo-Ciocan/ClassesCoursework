using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class HomeworkView : UserControl
    {
        public HomeworkView()
        {
            this.InitializeComponent();
        }

        public void FillWith(Course model)
        {
            /*foreach (HomeworkTask homework in model.Homeworks)
            {
                StackPanel panel = new StackPanel();
                //panel.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
                StackPanel completed = new StackPanel();
                StackPanel uncompleted = new StackPanel();
                Grid info = new Grid();

                panel.Children.Add(completed);
                panel.Children.Add(info);
                panel.Children.Add(uncompleted);

               // info.Children.Add(new 
            }*/
        }
    }
}
