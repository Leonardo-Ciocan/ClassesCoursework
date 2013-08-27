using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace TimetableApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Do not repeat app initialization when already running, just ensure that
            // the window is active
            
            if (args.PreviousExecutionState == ApplicationExecutionState.Running && string.IsNullOrEmpty(args.Arguments))
            {
                Window.Current.Activate();
                return;
            }

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            // Create a Frame to act navigation context and navigate to the first page
            var rootFrame = new Frame();

            if (string.IsNullOrEmpty(args.Arguments))
            {
                bool first = false;
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("first"))
                {

                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values.Add(new KeyValuePair<string, object>("first", true));
                    first = true;
                }

                Type tp = (first) ? typeof(Intro) : typeof(StartPage);
                if (!rootFrame.Navigate(tp))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            else{
                if (await CoreData.Initialize())
                {
                    //iterate through the folder and get all timetables
                    if (await CoreData.GetTimetables())
                    {
                     
                    }
                }
                else
                {
                    //if something happened then suggest a fix for the user
                    MessageDialog dialog = new MessageDialog("There was a problem initializing the data storage.\nTry closing and reopening the app.", "Oops");
                    await dialog.ShowAsync();
                }
                //if there are aguments go to the appropiate page
                if (args.Arguments.StartsWith("definitions"))
                {
                    string targetTB = args.Arguments.Split(',')[1];//name of target timetable
                    string target = args.Arguments.Split(',')[2];//name of target course
                    //do a linear search
                    foreach (StudentDataBundle tb in CoreData.Timetables)
                    {
                        if (tb.Name == targetTB)
                        {
                            foreach (Course model in tb.Courses)
                            {
                                if(model.Name == target) rootFrame.Navigate(typeof(DefinitionsEditor), new object[2]{tb,model});
                            }
                        }
                    }
                }

                if (args.Arguments.StartsWith("homework"))
                {
                    string targetTB = args.Arguments.Split(',')[1];
                    string target = args.Arguments.Split(',')[2];
                    foreach (StudentDataBundle tb in CoreData.Timetables)
                    {
                        if (tb.Name == targetTB)
                        {
                            foreach (Course model in tb.Courses)
                            {
                                if (model.Name == target) rootFrame.Navigate(typeof(HomeworkEditor), new object[2] { tb, model });
                            }
                        }
                    }
                }

                if (args.Arguments.StartsWith("topics"))
                {
                    string targetTB = args.Arguments.Split(',')[1];
                    string target = args.Arguments.Split(',')[2];
                    foreach (StudentDataBundle tb in CoreData.Timetables)
                    {
                        if (tb.Name == targetTB)
                        {
                            foreach (Course model in tb.Courses)
                            {
                                if (model.Name == target) rootFrame.Navigate(typeof(TopicsEditor), new object[2] { tb, model });
                            }
                        }
                    }
                }

                if (args.Arguments.StartsWith("notes"))
                {
                    string targetTB = args.Arguments.Split(',')[1];
                    string target = args.Arguments.Split(',')[2];
                    foreach (StudentDataBundle tb in CoreData.Timetables)
                    {
                        if (tb.Name == targetTB)
                        {
                            foreach (Course model in tb.Courses)
                            {
                                if (model.Name == target) rootFrame.Navigate(typeof(NotesEditor), new object[2] { tb, model });
                            }
                        }
                    }
                }
            }
            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>

        public static StudentDataBundle CurrentStudentDataBundle;
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
           // if (CurrentStudentDataBundle != null) await CoreData.SaveTimetable(CurrentStudentDataBundle);
            deferral.Complete();
        }

        /// <summary>
        /// handle file double clicking to import
        /// </summary>
        /// <param name="args"></param>
        protected async override void OnFileActivated(FileActivatedEventArgs args)
        {
            bool error = false;
            try
            {
                StudentDataBundle fileTable = new StudentDataBundle();
                StorageFile file = args.Files[0] as Windows.Storage.StorageFile;
                Stream stream = await file.OpenStreamForReadAsync();
                await Task.Run(() =>
                {
                    fileTable = CoreData.Serializer.Deserialize(stream) as StudentDataBundle;
                    CoreData.Timetables.Add(fileTable);
                });
                await CoreData.SaveTimetable(fileTable);

                var rootFrame = new Frame();
                rootFrame.Navigate(typeof(MainPage), fileTable);
                Window.Current.Content = rootFrame;
                MainPage p = rootFrame.Content as MainPage;

                Window.Current.Activate();
            }
            catch
            {
                error = true;
            }
            if (error)
            {
                //cannot await in catch clause , so a variable is needed
                Windows.UI.Popups.MessageDialog dialog = new Windows.UI.Popups.MessageDialog("Something is wrong with this file - or there already is a file with the same name", "We have a problem");
                await dialog.ShowAsync();
            }
        }

        /// <summary>
        /// Invoked when the application is activated to display search results.
        /// </summary>
        /// <param name="args">Details about the activation request.</param>
        protected async override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
        {
            // TODO: Register the Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted
            // event in OnWindowCreated to speed up searches once the application is already running
            if (await CoreData.Initialize())
            {
                //iterate through the folder and get all timetables
                if (await CoreData.GetTimetables())
                {
                    
                }
            }
            else
            {
                //if something happened then suggest a fix for the user
                MessageDialog dialog = new MessageDialog("There was a problem initializing the data storage.\nTry closing and reopening the app.", "Oops");
                await dialog.ShowAsync();
            }
            // If the Window isn't already using Frame navigation, insert our own Frame
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            // If the app does not contain a top-level frame, it is possible that this 
            // is the initial launch of the app. Typically this method and OnLaunched 
            // in App.xaml.cs can call a common method.
            if (frame == null)
            {
                // Create a Frame to act as the navigation context and associate it with
                // a SuspensionManager key
                frame = new Frame();
                
            }

            frame.Navigate(typeof(SearchResultsPage), args.QueryText);
            Window.Current.Content = frame;

            // Ensure the current window is active
            Window.Current.Activate();
        }
    }
}
