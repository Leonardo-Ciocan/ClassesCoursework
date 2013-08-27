using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TimetableApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            this.ManipulationMode = ManipulationModes.All;
            this.ManipulationCompleted += MainPage_ManipulationCompleted;
        }

        void MainPage_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.Cumulative.Scale < 0.7)
            {
                var diag = new MessageDialog("Done");
                diag.ShowAsync();
            }
        }

        public async void Save()
        {
            await CoreData.SaveTimetable(self);
        }

        StudentDataBundle self;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //a StudentDataBundle must be passed in order to reach this page
            if (e.Parameter.GetType() == typeof(StudentDataBundle))
            {
                //self will contain a reference to the current StudentDataBundle
                self = e.Parameter as StudentDataBundle;
                //fill the StudentDataBundle preview with the StudentDataBundle
                table.FillWith(self);


            }
            else
            {

            }
        }



        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            /*
            table.SizeChanged += (es, ev) =>
            {
                tableContainer.Width = table.ActualWidth;
                timetableGrid.Width = table.ActualWidth;
            };*/
            
            //clicking on the eventsButton will go to the class editor
            eventsButton.Click += (es, ev) => Frame.Navigate(typeof(ClassesEditor), self);
            
            
            
            //iterate through each class/course
            foreach (Course model in self.Courses)
            {
                //for each course we will add each type of preview item
                //the preview items will be added to their respective containers
                //the OnOpen event will go to the appropiate editor
                //the Navigation parameters are the current StudentDataBundle and the current Course , passed in an array

                EventPreview pr = new EventPreview(model);
                classes_holder.Children.Add(pr);

                var c = new StructurePreview(model);
                topicsHolder.Children.Add(c);
                c.OnOpen += (mod) => Frame.Navigate(typeof(TopicsEditor), new object[] { self, mod });


                var p = new ModulePreview(model);
                moduleHolder.Children.Add(p);
                p.OnOpen += (m) => Frame.Navigate(typeof(ModulesEditor), new object[] { self, m });


                DefinitionPreview defp = new DefinitionPreview(model);
                definitionsHolder.Children.Add(defp);
                defp.OnOpen += (m) => Frame.Navigate(typeof(DefinitionsEditor), new object []{self,m });

                HomeworkPreview pw = new HomeworkPreview(model);
                homeworkHolder.Children.Add(pw);
                pw.OnOpen += (m) => Frame.Navigate(typeof(HomeworkEditor), new object[] { self, m });

                NotePreview np = new NotePreview(model);
                notesHolder.Children.Add(np);
                np.OnOpen += (m) => Frame.Navigate(typeof(NotesEditor), new object[] { self, m });
            }

            Title.Text = self.Name;

           
            Window.Current.SizeChanged += (a, b) =>
            {
                scrollViewer.VerticalScrollBarVisibility  = ((Windows.UI.ViewManagement.ApplicationView.Value == ApplicationViewState.Snapped)) ? ScrollBarVisibility.Visible:ScrollBarVisibility.Disabled;

                scrollViewer.VerticalScrollMode = ((Windows.UI.ViewManagement.ApplicationView.Value == ApplicationViewState.Snapped)) ? ScrollMode.Enabled : ScrollMode.Disabled;
                upperHolder.Orientation = (Windows.UI.ViewManagement.ApplicationView.Value != ApplicationViewState.Snapped) ? Orientation.Horizontal : Orientation.Vertical;
                upperHolder.Visibility =(Windows.UI.ViewManagement.ApplicationView.Value == ApplicationViewState.Snapped)? Visibility.Collapsed:Visibility.Visible;
                snappedView.Visibility = (Windows.UI.ViewManagement.ApplicationView.Value == ApplicationViewState.Snapped) ? Visibility.Visible : Visibility.Collapsed;
               
                

                for (int x = 0; x < 7; x++)
                {
                    
                    if (Windows.UI.ViewManagement.ApplicationView.Value != ApplicationViewState.Snapped)
                    {
                        UIElement elem = (snappedHolder.Items[x] as FlipViewItem).Content as UIElement;
                        (snappedHolder.Items[x] as FlipViewItem).Content = null;
                        upperHolder.Children.Add(elem);
                    }
                    else
                    {
                        UIElement elem = upperHolder.Children[0];
                        upperHolder.Children.Remove(elem);
                        (snappedHolder.Items[x] as FlipViewItem).Content = elem;
                    }
                }


                (classes_holder.RenderTransform as CompositeTransform).ScaleX = (Windows.UI.ViewManagement.ApplicationView.Value != ApplicationViewState.Snapped) ? 1 : 0.8;
                (definitionsHolder.RenderTransform as CompositeTransform).ScaleX = (Windows.UI.ViewManagement.ApplicationView.Value != ApplicationViewState.Snapped) ? 1 : 0.8;
                (homeworkHolder.RenderTransform as CompositeTransform).ScaleX = (Windows.UI.ViewManagement.ApplicationView.Value != ApplicationViewState.Snapped) ? 1 : 0.8;
                (topicsHolder.RenderTransform as CompositeTransform).ScaleX = (Windows.UI.ViewManagement.ApplicationView.Value != ApplicationViewState.Snapped) ? 1 : 0.8;
                (notesHolder.RenderTransform as CompositeTransform).ScaleX = (Windows.UI.ViewManagement.ApplicationView.Value != ApplicationViewState.Snapped) ? 1 : 0.8;
                //(topicsHolder.RenderTransform as CompositeTransform).ScaleX = (Windows.UI.ViewManagement.ApplicationView.Value != ApplicationViewState.Snapped) ? 1 : 0.8;

                (classes_holder.RenderTransform as CompositeTransform).ScaleY = (Windows.UI.ViewManagement.ApplicationView.Value != ApplicationViewState.Snapped) ? 1 : 0.8;
                (definitionsHolder.RenderTransform as CompositeTransform).ScaleY = (Windows.UI.ViewManagement.ApplicationView.Value != ApplicationViewState.Snapped) ? 1 : 0.8;
                (homeworkHolder.RenderTransform as CompositeTransform).ScaleY = (Windows.UI.ViewManagement.ApplicationView.Value != ApplicationViewState.Snapped) ? 1 : 0.8;
                (topicsHolder.RenderTransform as CompositeTransform).ScaleY = (Windows.UI.ViewManagement.ApplicationView.Value != ApplicationViewState.Snapped) ? 1 : 0.8;
                (notesHolder.RenderTransform as CompositeTransform).ScaleY = (Windows.UI.ViewManagement.ApplicationView.Value != ApplicationViewState.Snapped) ? 1 : 0.8;
                //(topicsHolder.RenderTransform as CompositeTransform).ScaleX = (Windows.UI.ViewManagement.ApplicationView.Value != ApplicationViewState.Snapped) ? 1 : 0.8;
            };
            
        }

        

       
      

        private void backToStartClicked(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            //go back to the previous page
            Frame.Navigate(typeof(StartPage));
            
        }

    }
}
