using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
namespace TimetableApp
{
    public sealed partial class PieChart : UserControl
    {
        //the outer rings
        Polyline curve = new Polyline { StrokeThickness =30 };
        Polyline curve2 = new Polyline { StrokeThickness = 30 };

        //small rings to show the outline
        Polyline back1 = new Polyline { StrokeThickness = 2 };
        Polyline back2 = new Polyline { StrokeThickness = 2 };
        Polyline back3 = new Polyline { StrokeThickness = 2 };

        static double unit = 340; // the width /height

        public PieChart()
        {
            this.InitializeComponent();
            //add everything to the canvas
            root.Children.Add(curve);
            root.Children.Add(curve2);
            root.Children.Add(back1);
            root.Children.Add(back2);
            root.Children.Add(back3);

            this.Loaded += (a, b) =>
            {
                //initialize
                back1.Stroke = new SolidColorBrush(Colors.Gray);
                back2.Stroke = new SolidColorBrush(Colors.Gray);
                back3.Stroke = new SolidColorBrush(Colors.Gray);

                //set the outlines in the back
                Canvas.SetZIndex(back1, -3);
                Canvas.SetZIndex(back2, -3);
                Canvas.SetZIndex(back3, -3);
                
                Point center = new Point(unit / 2, unit / 2);

                double radius = 0;
                //go through the whole circle
                for (int i = 0; i <= 360; i++)
                {
                    //convert to radians
                    double angle = i * Math.PI / 180;

                    //use formula y = centerY+rsin(theta) , x = centerX + rcos(theta)
                    radius = unit / 2 -15;
                    Point p = new Point(center.X + radius * Math.Cos(angle), center.Y + radius * Math.Sin(angle));
                    back1.Points.Add(p);

                    radius = unit / 2 - 45;
                    Point p2 = new Point(center.X + radius * Math.Cos(angle), center.Y + radius * Math.Sin(angle));
                    back2.Points.Add(p2);

                    radius = unit / 2 - 75;
                    Point p3 = new Point(center.X + radius * Math.Cos(angle), center.Y + radius * Math.Sin(angle));
                    back3.Points.Add(p3);
                }
            };
        }

        //note:angle 0 is on the right so we will do angleD -90 to start at the top and going clockwise

        //draws the outer circle
        public void DrawWithAngle(double angleD , Color c){
            c.R = (byte)(c.R * 0.8);
            c.B = (byte)(c.B * 0.8);
            c.G = (byte)(c.G * 0.8);
                curve.Stroke = new SolidColorBrush(c);
                Point center = new Point(unit / 2, unit / 2);

                double radius = unit / 2 - 30;

                for (int x = -90; x < angleD-90; x++)
                {
                    double angle = x * Math.PI / 180;
                    Point p = new Point(center.X + radius * Math.Cos(angle), center.Y + radius * Math.Sin(angle));
                    curve.Points.Add(p);

                }
        }

        //draws the inner circle
        public void DrawInnerWithAngle(double angleD, Color c)
        {
            
            curve2.Stroke = new SolidColorBrush(c);
            Point center = new Point(unit / 2, unit / 2);

            double radius = unit / 2-60;

            for (int x = -90; x < angleD - 90; x++)
            {
                double angle = x * Math.PI / 180;
                Point p = new Point(center.X + radius * Math.Cos(angle), center.Y + radius * Math.Sin(angle));
                curve2.Points.Add(p);
            }
        }
    }
}
