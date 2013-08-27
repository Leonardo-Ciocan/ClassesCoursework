using System;
using System.Linq;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimetableApp
{
    public sealed partial class SimpleDatePicker : UserControl
    {
        public delegate void BackPressedEventHandler(DateTime time);
        public event BackPressedEventHandler BackPressed;



        public DateTime PickedTime = new DateTime();
        public SimpleDatePicker()
        {
            this.InitializeComponent();
            this.Loaded += (e, ev) =>
            {
                //Realistically , the user will not set homework that is more than 1 year apart (i.e in december 2013 , he may set January 2014 homework)
                year.Items.Add(new ListBoxItem {Content = "This year ("+ DateTime.Now.Year.ToString() +")"});
                year.Items.Add(new ListBoxItem { Content = "Next year (" + DateTime.Now.Year.ToString() + ")" });
            };

            //fill the month boxes with true for next year or false for last year
            year.SelectionChanged += (e, ev) => fillBoxes((year.SelectedIndex != 0));

            //return chosen date
            backButton.Click += (e, ev) =>
            {
                int yr = (year.SelectedIndex == 0) ? DateTime.Now.Year : DateTime.Now.AddYears(1).Year;
                PickedTime = new DateTime(yr,Month , Day);
                BackPressed(PickedTime);
            };
        }

        public void fillBoxes(bool nextYear)
        {
            month.Items.Clear();
            
                //if next year , start from January , else start from this month
            //add the month labels to the box
                for (int x = (nextYear)?0:DateTime.Now.Month-1; x < 12; x++)
                {
                    month.Items.Add(new ListBoxItem { Content = Constants.Months[x] });
                }
            //when a month is selected
                month.SelectionChanged += (e, ev) =>
                {
                    day.Items.Clear();
                    //if this year , start from tomorrow , else from 1st day of month
                    //go up to the last day in the month
                    for (int y = (Month == DateTime.Now.Month) ? DateTime.Now.Day + 1 : 1; y <= DateTime.DaysInMonth(DateTime.Now.AddYears(1).Year, Month); y++)
                    {
                        day.Items.Add(new ListBoxItem { Content = y.ToString() });
                    }
                };
            
            
        }

        //standard properties for getting values
        int Day
        {
            get { try { return int.Parse((day.SelectedItem as ListBoxItem).Content as string); } catch { return 1; } }
        }

        int Month
        {
            get {
                for(int x= 0; x< Constants.Months.Count();x++)
                {
                    if (Constants.Months[x] == (month.SelectedItem as ListBoxItem).Content as string) return x + 1;
                }
                return 1;
            }
        }
    }
}
