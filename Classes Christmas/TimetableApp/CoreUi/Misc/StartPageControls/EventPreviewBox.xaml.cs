using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class EventPreviewBox : UserControl
    {
        public EventPreviewBox()
        {
            this.InitializeComponent();
        }

        public void FillWith(StudentDataBundle table)
        {
            nextPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
            description.Text = table.Description;
            Course earliest = new Course {Name="null" };
            Instance earliestInstance = new Instance { StartTime = Time.MaximumTime , EndTime = Time.MaximumTime };
            foreach (Course model in table.Courses)
            {
                EventControl control = new EventControl(model);
                control.Margin = new Thickness(5, 0, 5, 0);
                eventHolder.Children.Add(control);
                foreach (Instance instance in model.Instances)
                {
                    if (instance.Day == (int)DateTime.Now.DayOfWeek-1)
                    {
                        Time now = new Time { Hours = DateTime.Now.Hour, Minutes = DateTime.Now.Minute };
                        if (now < instance.StartTime)
                        {
                            if (earliestInstance.StartTime < now)
                            {
                                earliestInstance = instance;
                                earliest = model;
                            }
                            else
                            {
                                if (instance.StartTime - now < earliestInstance.StartTime - now)
                                {
                                    earliestInstance = instance;
                                    earliest = model;
                                }
                            }
                        }
                    }
                }
            }
            if (earliest.Name != "null")
            {
                eventControl.FillWith(earliest);
                eventControl.setTime(earliestInstance.StartTime, earliestInstance.EndTime);
            }
            else
            {
                nextPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }
    }
}
