using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.System.Threading;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class TimetableView : UserControl
    {
        public TimetableView()
        {
            this.InitializeComponent();
            this.Loaded += (E, EV) =>
            {
                //draw the day name labels
                for (int x = 0; x < 7; x++)
                {
                    TextBlock tb = new TextBlock { FontSize = 15 };
                    switch (x)
                    {
                        case 0:
                            tb.Text = "Monday";
                            break;
                        case 1:
                            tb.Text = "Tuesday";
                            break;
                        case 2:
                            tb.Text = "Wendsday";
                            break;
                        case 3:
                            tb.Text = "Thursday";
                            break;
                        case 4:
                            tb.Text = "Friday";
                            break;
                        case 5:
                            tb.Text = "Saturday";
                            break;
                        case 6:
                            tb.Text = "Sunday";
                            break;
                    }
                    labelContainer.Children.Add(tb);//this holds the labels
                    //place the label at the appropiate position
                    Canvas.SetLeft(tb, x * Constants.EventControlWidth + Constants.EventControlWidth / 2 - tb.ActualWidth);
                    Canvas.SetTop(tb, 0);
                }
            };

            //draw a red line which shows the current time
            var dt = new DispatcherTimer { Interval = TimeSpan.FromMinutes(1) };
            dt.Tick += (a, b) => DrawCurrentTime();

        }

        //line definition
        Rectangle line = new Rectangle { Height = 5, Width = Constants.EventControlWidth, Fill = new SolidColorBrush(Colors.Red) };
        public void DrawCurrentTime()
        {
            //add it if it's not already there
           if(!root.Children.Contains(line)) root.Children.Add(line);

           
           if (lowerBound != null && upperBound != null)
           {
               var time = new Time {Hours = DateTime.Now.Hour, Minutes = DateTime.Now.Minute};
               //if current time is within the bounds of the timetable view , draw it in
               if (lowerBound < time && upperBound > time)
               {
                   double ratio = (time.TotalMinutes - lowerBound.TotalMinutes)/(upperBound.TotalMinutes - lowerBound.TotalMinutes);
                   Canvas.SetLeft(line, DateTime.Now.Day * Constants.EventControlWidth);
                   Canvas.SetTop(line, ratio *  root.ActualHeight);
               }
           }
               
        }


        //list for each day
        List<EventControl>[] instances = new List<EventControl>[]{
        new List<EventControl>(),
        new List<EventControl>(),
        new List<EventControl>(),
        new List<EventControl>(),
        new List<EventControl>(),
         new List<EventControl>(),
        new List<EventControl>()};

        //earliest and latest time
        private Time lowerBound;
        private Time upperBound;
        public void FillWith(StudentDataBundle studentDataBundle)
        {
            timeLabels.Children.Clear();
            root.Children.Clear();
            if (studentDataBundle.Courses.Count > 0 && studentDataBundle.GetTotalInstances() > 0)
            {
                lowerBound = CoreTime.GetEarliest(studentDataBundle);
                upperBound = CoreTime.GetLatest(studentDataBundle);

                //set size based on whether there is a weekedend or not
                labelContainer.Width = ((studentDataBundle.hasWeekend()) ? 7 : 5) * Constants.EventControlWidth;
                this.Width = Constants.EventControlWidth * ((studentDataBundle.hasWeekend()) ? 7 : 5) + 43;
                
                this.Height = (upperBound.TotalMinutes - lowerBound.TotalMinutes) / 60 * Constants.EventControlHeight + 43;


                for (int x = 0; x < studentDataBundle.Courses.Count; x++)
                {
                    Course model = studentDataBundle.Courses[x];
                    foreach (Instance instance in model.Instances)
                    {
                        //foreach instance we
                        EventControl cont = new EventControl(model);//create a control
                        cont.setTime(instance.StartTime, instance.EndTime);//set the text
                        instances[instance.Day].Add(cont);//add it to the appropiate list
                        root.Children.Add(cont);//add it to the view
                        Canvas.SetLeft(cont, Constants.EventControlWidth * instance.Day);//set it at the x coord and y coord
                        Canvas.SetTop(cont, ((instance.StartTime.TotalMinutes - lowerBound.TotalMinutes) / 60.0) * Constants.EventControlHeight);//take lenght in account (i.e 2 hour lesson)
                        double dt = (instance.EndTime.TotalMinutes - instance.StartTime.TotalMinutes);
                        double height = dt / 60;
                        cont.Height = height * Constants.EventControlHeight; //set the height proportionately to the duration
                    }
                }

                //create the time labels and the outline lines
                for (int x = lowerBound.TotalMinutes; x <= upperBound.TotalMinutes; x += 30)
                {
                    TextBlock block = new TextBlock { FontSize = (x % 60 == 0) ? 12 : 10 };
                    //block.Text = string.Format("{0}:{1}", ((int)x / 60).ToString(), ((x % 60)).ToString());
                    // block.Text = (x / 60).ToString() + ":00";
                    double hours = x / 60.0;
                    block.Text = Math.Truncate(hours).ToString() + ":" + ((hours - Math.Truncate(hours)) * 60).ToString();
                    timeLabels.Children.Add(block);
                    Canvas.SetLeft(block, 0);
                    Canvas.SetTop(block, Constants.EventControlHeight * ((x - lowerBound.TotalMinutes) / 60.0) - block.ActualHeight);

                    Line indicator = new Line { Stroke = new SolidColorBrush(Colors.DarkGray), StrokeThickness = 1 };
                    root.Children.Add(indicator);
                    indicator.X1 = 0;
                    indicator.Y1 = Constants.EventControlHeight * ((x - lowerBound.TotalMinutes) / 60.0);
                    indicator.X2 = Constants.EventControlWidth * ((studentDataBundle.hasWeekend()) ? 7 : 5) + timeLabels.ActualWidth;
                    indicator.Y2 = indicator.Y1;
                    Canvas.SetZIndex(indicator, -1);
                }


            }
        }
    }
}
