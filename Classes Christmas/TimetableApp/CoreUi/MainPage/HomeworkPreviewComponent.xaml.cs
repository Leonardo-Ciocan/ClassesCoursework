using System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class HomeworkPreviewComponent : UserControl
    {
        public HomeworkPreviewComponent(HomeworkTask hw,string module_name , Color c)
        {
            this.InitializeComponent();
            //initial values
            module.Text = module_name;
            content.Text = hw.Notes;
            module.Foreground = new SolidColorBrush(c);
            border.Background = new SolidColorBrush(c);
            int left = (hw.DueOn - DateTime.Now).Days;
            if (left == 0) time.Text = "Today";
            else time.Text = left.ToString() + " days left";

            if (left < 0) time.Text = "OVERDUE"; //negative left means due date passed.

            //set colors to indicate proximity of date
            if (left < 5)
            {
                time.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (left < 14)
            {
                time.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                time.Foreground = new SolidColorBrush(Colors.Green);
            }
        }
    }
}
