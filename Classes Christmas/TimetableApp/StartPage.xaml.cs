using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TimetableApp
{
    /// <summary>
    /// This page is shown to the use whenever he opens the app. It shows his timetables.
    /// </summary>
    public sealed partial class StartPage : Page
    {
        public StartPage()
        {
            this.InitializeComponent();
            this.Loaded += StartPage_Loaded;
            //Core.temp = this;
        }

        //there is only one NewTimetableBox and it will be reused
        NewTimetableBox box = new NewTimetableBox();
        async void StartPage_Loaded(object sender, RoutedEventArgs e)
        {
            //calling the fill method if a new StudentDataBundle was created
            box.OnCreate += Fill;
            //load the data folder
            if (await CoreData.Initialize())
            {
                //iterate through the folder and get all timetables
                if (await CoreData.GetTimetables())
                {
                    Fill();
                }
            }
            else
            {
                //if something happened then suggest a fix for the user
                MessageDialog dialog = new MessageDialog("There was a problem initializing the data storage.\nTry closing and reopening the app.", "Oops");
                await dialog.ShowAsync();
            }
            
        }

        /// <summary>
        /// Use this to fill the container with the timetables.
        /// </summary>
        public void Fill()
        {
            //clear the holder so the everything is up to date
            holder.Children.Clear();

            //check if there are any timetables
            if (CoreData.Timetables != null)
            {
                //iterate through the timetables
                foreach (StudentDataBundle table in CoreData.Timetables)
                {
                    //create a TimetableBox for each StudentDataBundle
                    var timetable = table;
                    var bx = new TimetableBox(timetable);
                    //and add it to the holder
                    holder.Children.Add(bx);
                    //if a a box/StudentDataBundle is deleted reload everything
                    bx.OnDeleted += Fill;
                    //if the user clicks open go to MainPage with the StudentDataBundle as a parameter
                    bx.OnOpen += (tb) =>
                    {
                        App.CurrentStudentDataBundle = tb;
                        Frame.Navigate(typeof(MainPage), tb);
                    };
                }
            }
            //at the very end add the new box
            holder.Children.Add(box);
        }
    }
}
